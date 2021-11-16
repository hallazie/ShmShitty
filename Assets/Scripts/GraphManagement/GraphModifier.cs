using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphModifier
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
                Debug.LogError("Merge threshold in graph modifier only accept float in range [0, 1]");
                return;
            }
            _mergeThreshold = thresh;
        }
    }

    public GraphModifier(float mergeThreshold=0.66f)
    {
        this.mergeThreshold = mergeThreshold;
    }

    public void FiltInvalidQuads(List<GraphPolygon> polygonList)
    {

    }


    public List<GraphPolygon> SplitToQuads(List<GraphPolygon> polygonList, List<GraphVertex> vertexList)
    {
        List<GraphPolygon> splitedList = new List<GraphPolygon>();
        List<GraphVertex> lineCenterList = new List<GraphVertex>();
        foreach (GraphPolygon polygon in polygonList)
        {
            for (int i = 0; i < polygon.graphVertexList.Count; i++)
            {
                int indexNext = i + 1;
                int indexPrev = i - 1;
                if (i == polygon.graphVertexList.Count - 1)
                    indexNext = 0;
                if (i == 0)
                    indexPrev = polygon.graphVertexList.Count - 1;
                GraphVertex c1 = CommonUtils.FindLineCenter(polygon.graphVertexList[indexPrev], polygon.graphVertexList[i]);
                GraphVertex c2 = CommonUtils.FindLineCenter(polygon.graphVertexList[i], polygon.graphVertexList[indexNext]);
                GraphVertex l1 = null;
                GraphVertex l2 = null;
                foreach (GraphVertex v in lineCenterList)
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
                GraphPolygon splitedPolygon = new GraphPolygon(new List<GraphVertex> { l1, l2, polygon.graphVertexList[i], polygon.center });
                if (!vertexList.Contains(l1))
                    vertexList.Add(l1);
                if (!vertexList.Contains(l2))
                    vertexList.Add(l2);
                if (!vertexList.Contains(polygon.graphVertexList[i]))
                    vertexList.Add(polygon.graphVertexList[i]);
                if (!vertexList.Contains(polygon.center))
                    vertexList.Add(polygon.center);
                splitedList.Add(splitedPolygon);
            }
        }
        return splitedList;
    }

    private int IntersectVertexCountBetweenTwoPolygon(GraphPolygon p1, GraphPolygon p2)
    {
        int intersectCount = 0;
        foreach (GraphVertex v1 in p1.graphVertexList)
        {
            foreach (GraphVertex v2 in p2.graphVertexList)
            {
                if (v1.x == v2.x && v1.y == v2.y)
                {
                    intersectCount += 1;
                }
            }
        }
        return intersectCount;
    }

    public List<GraphPolygon> RandomMerge(List<GraphPolygon> polygonList, float mergeThreshold)
    {
        this.mergeThreshold = mergeThreshold;
        System.Random random = new System.Random();
        List<GraphPolygon> mergedList = new List<GraphPolygon>();
        int siz1 = polygonList.Count;
        while (polygonList.Count > 0)
        {
            int randomIndex = random.Next(0, polygonList.Count-1);
            GraphPolygon head = polygonList[randomIndex];
            polygonList.Remove(head);
            List<GraphPolygon> polygonSubList = new List<GraphPolygon>();
            if ((float)random.NextDouble() > this.mergeThreshold)
            {
                mergedList.Add(head);
                continue;
            }
            foreach (GraphPolygon other in polygonList)
            {
                int intersectSize = IntersectVertexCountBetweenTwoPolygon(head, other);
                float centroidDistance = CommonUtils.GraphVertexEuclideanDistance(head.center, other.center);
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
            GraphPolygon tail = polygonSubList[random.Next(0, polygonSubList.Count - 1)];
            polygonList.Remove(tail);

            float distance = CommonUtils.GraphVertexEuclideanDistance(head.center, tail.center);

            HashSet<GraphVertex> headVertex = new HashSet<GraphVertex>(head.graphVertexList);
            HashSet<GraphVertex> tailVertex = new HashSet<GraphVertex>(tail.graphVertexList);
            headVertex.UnionWith(tailVertex);
            GraphPolygon mergePolygon = new GraphPolygon(new List<GraphVertex>(headVertex));
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

        HashSet<GraphVertex> vertexSet = new HashSet<GraphVertex>();
        foreach (GraphPolygon polygon in mergedList)
        {
            foreach (GraphVertex vertex in polygon.graphVertexList)
            {
                vertexSet.Add(vertex);
            }
        }
        Debug.Log("graph-vertex hash set size: " + vertexSet.Count);

        Debug.Log("Graph modifier random merge finished threshold=" + mergeThreshold.ToString() + ", from size " + siz1.ToString() + " --> " + siz2.ToString());
        return mergedList;
    }

    private bool ValidNeighbor(GraphPolygon mergePolygon)
    {
        List<float> adjecentDistanceList = new List<float>();
        for(int i = 0; i < mergePolygon.graphVertexList.Count; i++)
        {
            int next = i + 1;
            if (i == mergePolygon.graphVertexList.Count - 1)
            {
                next = 0;
            }
            adjecentDistanceList.Add(CommonUtils.GraphVertexEuclideanDistance(mergePolygon.graphVertexList[i], mergePolygon.graphVertexList[next]));
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
