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



    public List<GridPolygon> SplitToQuads(List<GridPolygon> polygonList)
    {
        List<GridPolygon> splitedList = new List<GridPolygon>();
        foreach (GridPolygon polygon in polygonList)
        {
            splitedList.AddRange(polygon.SplitToQuads());
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
            GridPolygon head = polygonList[0];
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
            // Debug.Log("merging center: " + head.center.ToString() + " and " + tail.center.ToString() + " with distance=" + distance.ToString());

            HashSet<GridVertex> headVertex = new HashSet<GridVertex>(head.gridVertexList);
            HashSet<GridVertex> tailVertex = new HashSet<GridVertex>(tail.gridVertexList);
            headVertex.UnionWith(tailVertex);
            GridPolygon mergePolygon = new GridPolygon(new List<GridVertex>(headVertex));
            if (!ValidNeighbor(mergePolygon))
            {
                mergedList.Add(head);
                mergedList.Add(tail);
                Debug.LogWarning("distance error while merging: ==========================");
                Debug.LogWarning("head = " + string.Join("; ", head.gridVertexList.Select(x => x.ToString())) + " with");
                Debug.LogWarning("tail = " + string.Join("; ", tail.gridVertexList.Select(x => x.ToString())));
            }
            else
            {
                mergedList.Add(mergePolygon);
            }
        }
        int siz2 = mergedList.Count;
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
