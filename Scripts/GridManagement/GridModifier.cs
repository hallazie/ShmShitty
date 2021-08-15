using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridModifier
{

    public float _mergeThreshold;

    public float mergeThreshold
    {

        get
        {
            return _mergeThreshold;
        }

        set
        {
            float thresh = (float)value;
            if(thresh > 1 || thresh < 0)
            {
                Debug.LogError("Merge threshold in grid modifier only accept float in range [0, 1]");
                return;
            }
            _mergeThreshold = thresh;
        }
    }

    public GridModifier(float mergeThreshold=0.66f)
    {
        this.mergeThreshold = mergeThreshold;
    }

    public void FiltInvalidQuads(List<GridPolygon> polygonList)
    {

    }


    public List<GridPolygon> SplitToQuads(List<GridPolygon> polygonList, List<GridVertex> vertexList)
    {
        List<GridPolygon> splitedList = new List<GridPolygon>();
        List<GridVertex> lineCenterList = new List<GridVertex>();
        foreach (GridPolygon polygon in polygonList)
        {
            for (int i = 0; i < polygon.gridVertexList.Count; i++)
            {
                int indexNext = i + 1;
                int indexPrev = i - 1;
                if (i == polygon.gridVertexList.Count - 1)
                    indexNext = 0;
                if (i == 0)
                    indexPrev = polygon.gridVertexList.Count - 1;
                GridVertex c1 = CommonUtils.FindLineCenter(polygon.gridVertexList[indexPrev], polygon.gridVertexList[i]);
                GridVertex c2 = CommonUtils.FindLineCenter(polygon.gridVertexList[i], polygon.gridVertexList[indexNext]);
                GridVertex l1 = null;
                GridVertex l2 = null;
                foreach (GridVertex v in lineCenterList)
                {
                    if (v.x == c1.x && v.y == c1.y)
                    {
                        l1 = v;
                    }
                    if (v.x == c2.x && v.y == c2.y)
                    {
                        l2 = v;
                    }
                }
                if (l1 == null)
                {
                    lineCenterList.Add(c1);
                    l1 = c1;
                }
                if (l2 == null)
                {
                    lineCenterList.Add(c2);
                    l2 = c2;
                }
                GridPolygon splitedPolygon = new GridPolygon(new List<GridVertex> { l1, l2, polygon.gridVertexList[i], polygon.center });
                if (!vertexList.Contains(l1))
                    vertexList.Add(l1);
                if (!vertexList.Contains(l2))
                    vertexList.Add(l2);
                if (!vertexList.Contains(polygon.gridVertexList[i]))
                    vertexList.Add(polygon.gridVertexList[i]);
                if (!vertexList.Contains(polygon.center))
                    vertexList.Add(polygon.center);
                splitedList.Add(splitedPolygon);
            }
        }
        return splitedList;
    }

    private int IntersectVertexCountBetweenTwoPolygon(GridPolygon p1, GridPolygon p2)
    {
        int intersectCount = 0;
        foreach (GridVertex v1 in p1.gridVertexList)
        {
            foreach (GridVertex v2 in p2.gridVertexList)
            {
                if (v1.x == v2.x && v1.y == v2.y)
                {
                    intersectCount += 1;
                }
            }
        }
        return intersectCount;
    }

    public List<GridPolygon> RandomMerge(List<GridPolygon> polygonList, float mergeThreshold)
    {
        this.mergeThreshold = mergeThreshold;
        System.Random random = new System.Random();
        List<GridPolygon> mergedList = new List<GridPolygon>();
        int siz1 = polygonList.Count;
        while (polygonList.Count > 0)
        {
            int randomIndex = random.Next(0, polygonList.Count-1);
            GridPolygon head = polygonList[randomIndex];
            polygonList.Remove(head);
            List<GridPolygon> polygonSubList = new List<GridPolygon>();
            if ((float)random.NextDouble() > this.mergeThreshold)
            {
                // mergedList.Add(new GridPolygon(head));
                mergedList.Add(head);
                continue;
            }
            foreach (GridPolygon other in polygonList)
            {
                int intersectSize = IntersectVertexCountBetweenTwoPolygon(head, other);
                float centroidDistance = CommonUtils.GridVertexEuclideanDistance(head.center, other.center);
                if (intersectSize == 2 && centroidDistance > 1 && centroidDistance < 2)
                {
                    polygonSubList.Add(other);
                }
            }
            if(polygonSubList.Count == 0)
            {
                mergedList.Add(head);
                continue;
            }
            GridPolygon tail = polygonSubList[random.Next(0, polygonSubList.Count - 1)];
            polygonList.Remove(tail);

            float distance = CommonUtils.GridVertexEuclideanDistance(head.center, tail.center);

            HashSet<GridVertex> headVertex = new HashSet<GridVertex>(head.gridVertexList);
            HashSet<GridVertex> tailVertex = new HashSet<GridVertex>(tail.gridVertexList);
            headVertex.UnionWith(tailVertex);
            GridPolygon mergePolygon = new GridPolygon(new List<GridVertex>(headVertex));
            if (!ValidNeighbor(mergePolygon) && false)
            {
                mergedList.Add(head);
                mergedList.Add(tail);
            }
            else
            {
                mergedList.Add(mergePolygon);
            }
        }
        int siz2 = mergedList.Count;

        HashSet<GridVertex> vertexSet = new HashSet<GridVertex>();
        foreach (GridPolygon polygon in mergedList)
        {
            foreach (GridVertex vertex in polygon.gridVertexList)
            {
                vertexSet.Add(vertex);
            }
        }
        Debug.Log("grid-vertex hash set size: " + vertexSet.Count);

        Debug.Log("Grid modifier random merge finished threshold=" + mergeThreshold.ToString() + ", from size " + siz1.ToString() + " --> " + siz2.ToString());
        return mergedList;
    }

    private bool ValidNeighbor(GridPolygon mergePolygon)
    {
        List<float> adjecentDistanceList = new List<float>();
        for(int i = 0; i < mergePolygon.gridVertexList.Count; i++)
        {
            int next = i + 1;
            if (i == mergePolygon.gridVertexList.Count - 1)
            {
                next = 0;
            }
            adjecentDistanceList.Add(CommonUtils.GridVertexEuclideanDistance(mergePolygon.gridVertexList[i], mergePolygon.gridVertexList[next]));
        }
        if(adjecentDistanceList.Count < 2)
        {
            return false;
        }
        adjecentDistanceList = adjecentDistanceList.OrderBy(obj => obj).ToList();
        if(Mathf.Abs(adjecentDistanceList[0] - adjecentDistanceList[adjecentDistanceList.Count-1]) < 0.001f)
        {
            return true;
        }
        return false;
    }
}
