using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphRelaxation
{

    private float learningRate;
    private bool alphaFix;
    private Hashtable alphaCache;

    public GraphRelaxation(float learningRate, bool alphaFix)
    {
        this.learningRate = learningRate;
        this.alphaFix = alphaFix;
        this.alphaCache = new Hashtable();
    }

    public float? FirstDerivative(GraphPolygon polygon, float alpha)
    {
        /*
         first derivative for the loss function
         */

        if (polygon.cornerGraphVertexList.Count != 4)
            return null;
        float radius = (polygon.sideLength / 8.0f ) * Mathf.Sqrt(2);
        float derive = 2 * radius * Mathf.Sin(alpha * (polygon.cornerGraphVertexList[0].x - polygon.cornerGraphVertexList[1].y - polygon.cornerGraphVertexList[2].x + polygon.cornerGraphVertexList[3].y))
            + 2 * radius * Mathf.Cos(alpha * (-polygon.cornerGraphVertexList[0].y - polygon.cornerGraphVertexList[1].x + polygon.cornerGraphVertexList[2].y + polygon.cornerGraphVertexList[3].x));
        return derive;
    }

    public float SecondDerivative(GraphPolygon polygon, float alpha, float radius)
    {
        /*
         second derivative for the loss function
         */

        if (polygon.cornerGraphVertexList.Count != 4)
            return 0;
        float derive = 2 * radius * Mathf.Cos(alpha * (polygon.cornerGraphVertexList[0].x - polygon.cornerGraphVertexList[1].y - polygon.cornerGraphVertexList[2].x + polygon.cornerGraphVertexList[3].y))
            + 2 * radius * Mathf.Sin(alpha * (polygon.cornerGraphVertexList[0].y + polygon.cornerGraphVertexList[1].x - polygon.cornerGraphVertexList[2].y - polygon.cornerGraphVertexList[3].x));
        return derive;
    }

    public float ArgumentMinAlpha(GraphPolygon polygon, float radius)
    {
        if (polygon.cornerGraphVertexList.Count != 4)
            return 0;
        float numerator = (float)(polygon.cornerGraphVertexList[0].y + polygon.cornerGraphVertexList[1].x - polygon.cornerGraphVertexList[2].y - polygon.cornerGraphVertexList[3].x);
        float denominator = (float)(polygon.cornerGraphVertexList[0].x - polygon.cornerGraphVertexList[1].y - polygon.cornerGraphVertexList[2].x + polygon.cornerGraphVertexList[3].y);
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

    private List<GraphVertex> CornerCoordinates(GraphVertex center, float alpha, float radius)
    {
        float sina = Mathf.Sin(alpha);
        float cosa = Mathf.Cos(alpha);
        GraphVertex vertex1 = new GraphVertex(center.x + radius * cosa, center.y + radius * sina);
        GraphVertex vertex2 = new GraphVertex(center.x + radius * sina, center.y - radius * cosa);
        GraphVertex vertex3 = new GraphVertex(center.x - radius * cosa, center.y - radius * sina);
        GraphVertex vertex4 = new GraphVertex(center.x - radius * sina, center.y + radius * cosa);
        return new List<GraphVertex> {vertex1, vertex2, vertex3, vertex4};
    }

    private List<GraphVertex> AlignToVertex(GraphPolygon polygon, List<GraphVertex> quadList)
    {
        if(polygon.cornerGraphVertexList.Count < 1)
        {
            return polygon.cornerGraphVertexList;
        }
        Hashtable closestTable = new Hashtable();
        foreach (GraphVertex vertex in polygon.cornerGraphVertexList)
        {
            GraphVertex minVertex = CommonUtils.FindClosestVertexForTarget(quadList, vertex);
            closestTable[vertex] = minVertex;
        }
        if(closestTable.Count == 4)
        {
            List<GraphVertex> alignedVertexList = new List<GraphVertex>();
            foreach (GraphVertex vertex in polygon.cornerGraphVertexList)
            {
                alignedVertexList.Add((GraphVertex)closestTable[vertex]);
            }
            return alignedVertexList;
        }
        else
        {
            return polygon.cornerGraphVertexList;
        }
    }

    private void RelaxSingleStepUpdate(List<GraphPolygon> polygonList, int epoch)
    {
        Hashtable differentiate = new Hashtable();
        foreach (GraphPolygon polygon in polygonList)
        {
            foreach (GraphVertex vertex in polygon.cornerGraphVertexList)
            {
                differentiate[vertex] = new List<float>();
            }
        }
        foreach (GraphPolygon polygon in polygonList)
        {
            if (polygon.cornerGraphVertexList.Count < 1)
                continue;
            GraphVertex center = polygon.center;
            float radius = CommonUtils.GraphVertexEuclideanDistance(CommonUtils.FindClosestVertexForTarget(polygon.cornerGraphVertexList, center), center);
            float alpha;
            if (!this.alphaCache.Contains(polygon) || !this.alphaFix)
            {
                alpha = ArgumentMinAlpha(polygon, radius);
                this.alphaCache[polygon] = alpha;
            }
            alpha = (float)this.alphaCache[polygon];
            // Debug.Log("current alpha=" + alpha.ToString());
            List<GraphVertex> cornerCoordinateList = CornerCoordinates(center, alpha, radius);
            List<GraphVertex> alignedList = AlignToVertex(polygon, cornerCoordinateList);
            for(int i = 0; i < polygon.cornerGraphVertexList.Count; i++)
            {
                float xDiff = (alignedList[i].x - polygon.cornerGraphVertexList[i].x) * this.learningRate;
                float yDiff = (alignedList[i].y - polygon.cornerGraphVertexList[i].y) * this.learningRate;
                // Debug.Log("total sum diff: " + xDiff.ToString() + ", " + yDiff.ToString());
                var v = (List<float>)differentiate[polygon.cornerGraphVertexList[i]];
                v.Add(xDiff);
                v.Add(yDiff);
                differentiate[polygon.cornerGraphVertexList[i]] = v;
            }
        }
        Hashtable averageDifferentiate = new Hashtable();
        foreach (DictionaryEntry kvPair in differentiate)
        {
            GraphVertex key = (GraphVertex)kvPair.Key;
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
        foreach (GraphPolygon polygon in polygonList)
        {
            for(int i = 0; i < polygon.cornerGraphVertexList.Count; i++)
            {
                if (!averageDifferentiate.ContainsKey(polygon.cornerGraphVertexList[i]))
                {
                    continue;
                }
                List<float> avg = (List<float>)averageDifferentiate[polygon.cornerGraphVertexList[i]];
                if (avg.Count != 2)
                    continue;
                polygon.cornerGraphVertexList[i].x += avg[0];
                polygon.cornerGraphVertexList[i].y += avg[1];
                totalLoss += avg[0];
                totalLoss += avg[1];
            }
            polygon.RecalculateCenter();
        }
        Debug.Log("Epoch=" + epoch.ToString() + ", Loss=" + totalLoss.ToString());
    }

    public List<GraphPolygon> Relaxation(List<GraphPolygon> polygonList, int epoches = 10, float? learningRate = null)
    {
        if(learningRate != null)
        {
            this.learningRate = (float)learningRate;
        }
        for(int ep = 0; ep < epoches; ep++)
        {
            RelaxSingleStepUpdate(polygonList, ep);
            int vertexNum = polygonList.Sum(obj => obj.cornerGraphVertexList.Count);
            // Debug.Log("polygon list size=" + polygonList.Count.ToString() + " with total vertex num=" + vertexNum);
        }
        return polygonList;
    }

}
