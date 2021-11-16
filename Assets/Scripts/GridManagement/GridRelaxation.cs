using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridRelaxation
{

    private float learningRate;
    private bool alphaFix;
    private Hashtable alphaCache;

    public GridRelaxation(float learningRate, bool alphaFix)
    {
        this.learningRate = learningRate;
        this.alphaFix = alphaFix;
        this.alphaCache = new Hashtable();
    }

    public float? FirstDerivative(GridPolygon polygon, float alpha)
    {
        /*
         first derivative for the loss function
         */

        if (polygon.lowerGridVertexList.Count != 4)
            return null;
        float radius = (polygon.sideLength / 8.0f ) * Mathf.Sqrt(2);
        float derive = 2 * radius * Mathf.Sin(alpha * (polygon.lowerGridVertexList[0].x - polygon.lowerGridVertexList[1].y - polygon.lowerGridVertexList[2].x + polygon.lowerGridVertexList[3].y))
            + 2 * radius * Mathf.Cos(alpha * (-polygon.lowerGridVertexList[0].y - polygon.lowerGridVertexList[1].x + polygon.lowerGridVertexList[2].y + polygon.lowerGridVertexList[3].x));
        return derive;
    }

    public float SecondDerivative(GridPolygon polygon, float alpha, float radius)
    {
        /*
         second derivative for the loss function
         */

        if (polygon.lowerGridVertexList.Count != 4)
            return 0;
        float derive = 2 * radius * Mathf.Cos(alpha * (polygon.lowerGridVertexList[0].x - polygon.lowerGridVertexList[1].y - polygon.lowerGridVertexList[2].x + polygon.lowerGridVertexList[3].y))
            + 2 * radius * Mathf.Sin(alpha * (polygon.lowerGridVertexList[0].y + polygon.lowerGridVertexList[1].x - polygon.lowerGridVertexList[2].y - polygon.lowerGridVertexList[3].x));
        return derive;
    }

    public float ArgumentMinAlpha(GridPolygon polygon, float radius)
    {
        if (polygon.lowerGridVertexList.Count != 4)
            return 0;
        float numerator = (float)(polygon.lowerGridVertexList[0].y + polygon.lowerGridVertexList[1].x - polygon.lowerGridVertexList[2].y - polygon.lowerGridVertexList[3].x);
        float denominator = (float)(polygon.lowerGridVertexList[0].x - polygon.lowerGridVertexList[1].y - polygon.lowerGridVertexList[2].x + polygon.lowerGridVertexList[3].y);
        if (denominator == 0)
        {
            return 0;
        }
        float constant = Mathf.Atan(numerator / denominator);
        float alpha1 = constant;
        float alpha2 = constant + Mathf.PI;
        float secondDerive = SecondDerivative(polygon, alpha1, radius);
        return secondDerive > 0 ? alpha1 : alpha2;
    }

    private List<GridVertex> CornerCoordinates(GridVertex center, float alpha, float radius)
    {
        float sina = Mathf.Sin(alpha);
        float cosa = Mathf.Cos(alpha);
        GridVertex vertex1 = new GridVertex(center.x + radius * cosa, center.y + radius * sina);
        GridVertex vertex2 = new GridVertex(center.x + radius * sina, center.y - radius * cosa);
        GridVertex vertex3 = new GridVertex(center.x - radius * cosa, center.y - radius * sina);
        GridVertex vertex4 = new GridVertex(center.x - radius * sina, center.y + radius * cosa);
        return new List<GridVertex> {vertex1, vertex2, vertex3, vertex4};
    }

    private List<GridVertex> AlignToVertex(GridPolygon polygon, List<GridVertex> quadList)
    {
        if(polygon.lowerGridVertexList.Count < 1)
        {
            return polygon.lowerGridVertexList;
        }
        Hashtable closestTable = new Hashtable();
        foreach (GridVertex vertex in polygon.lowerGridVertexList)
        {
            GridVertex minVertex = CommonUtils.FindClosestVertexForTarget(quadList, vertex);
            closestTable[vertex] = minVertex;
        }
        if(closestTable.Count == 4)
        {
            List<GridVertex> alignedVertexList = new List<GridVertex>();
            foreach (GridVertex vertex in polygon.lowerGridVertexList)
            {
                alignedVertexList.Add((GridVertex)closestTable[vertex]);
            }
            return alignedVertexList;
        }
        else
        {
            return polygon.lowerGridVertexList;
        }
    }

    private void RelaxSingleStepUpdate(List<GridPolygon> polygonList, int epoch)
    {
        Hashtable differentiate = new Hashtable();
        foreach (GridPolygon polygon in polygonList)
        {
            foreach (GridVertex vertex in polygon.lowerGridVertexList)
            {
                differentiate[vertex] = new List<float>();
            }
        }
        foreach (GridPolygon polygon in polygonList)
        {
            if (polygon.lowerGridVertexList.Count < 1)
                continue;
            GridVertex center = polygon.center;
            float radius = CommonUtils.GridVertexEuclideanDistance(CommonUtils.FindClosestVertexForTarget(polygon.lowerGridVertexList, center), center);
            float alpha;
            if (!this.alphaCache.Contains(polygon) || !this.alphaFix)
            {
                alpha = ArgumentMinAlpha(polygon, radius);
                this.alphaCache[polygon] = alpha;
            }
            alpha = (float)this.alphaCache[polygon];
            // Debug.Log("current alpha=" + alpha.ToString());
            List<GridVertex> cornerCoordinateList = CornerCoordinates(center, alpha, radius);
            List<GridVertex> alignedList = AlignToVertex(polygon, cornerCoordinateList);
            for(int i = 0; i < polygon.lowerGridVertexList.Count; i++)
            {
                float xDiff = (alignedList[i].x - polygon.lowerGridVertexList[i].x) * this.learningRate;
                float yDiff = (alignedList[i].y - polygon.lowerGridVertexList[i].y) * this.learningRate;
                // Debug.Log("total sum diff: " + xDiff.ToString() + ", " + yDiff.ToString());
                var v = (List<float>)differentiate[polygon.lowerGridVertexList[i]];
                v.Add(xDiff);
                v.Add(yDiff);
                differentiate[polygon.lowerGridVertexList[i]] = v;
            }
        }
        Hashtable averageDifferentiate = new Hashtable();
        foreach (DictionaryEntry kvPair in differentiate)
        {
            GridVertex key = (GridVertex)kvPair.Key;
            List<float> val = (List<float>)kvPair.Value;
            float sumX = 0.0f;
            float sumY = 0.0f;
            for (int j = 0; j < val.Count; j++){
                if (j % 2 == 0)
                {
                    sumX += val[j];
                }
                else
                {
                    sumY += val[j];
                }
            }
            float avgX = sumX / (float)(val.Count / 2);
            float avgY = sumY / (float)(val.Count / 2);
            averageDifferentiate[key] = new List<float>{ avgX, avgY };
        }
        float totalLoss = 0;
        foreach (GridPolygon polygon in polygonList)
        {
            for(int i = 0; i < polygon.lowerGridVertexList.Count; i++)
            {
                if (!averageDifferentiate.ContainsKey(polygon.lowerGridVertexList[i]))
                {
                    continue;
                }
                List<float> avg = (List<float>)averageDifferentiate[polygon.lowerGridVertexList[i]];
                if (avg.Count != 2)
                    continue;
                polygon.lowerGridVertexList[i].x += avg[0];
                polygon.lowerGridVertexList[i].y += avg[1];
                totalLoss += avg[0];
                totalLoss += avg[1];
            }
            polygon.RecalculateCenter();
        }
        Debug.Log("Epoch=" + epoch.ToString() + ", Loss=" + totalLoss.ToString());
    }

    public List<GridPolygon> Relaxation(List<GridPolygon> polygonList, int epoches = 10, float? learningRate = null)
    {
        if(learningRate != null)
        {
            this.learningRate = (float)learningRate;
        }
        for(int ep = 0; ep < epoches; ep++)
        {
            RelaxSingleStepUpdate(polygonList, ep);
            int vertexNum = polygonList.Sum(obj => obj.lowerGridVertexList.Count);
            Debug.Log("polygon list size=" + polygonList.Count.ToString() + " with total vertex num=" + vertexNum);
        }
        return polygonList;
    }

}
