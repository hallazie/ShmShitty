using System.Collections;
using System.Collections.Generic;
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

    public List<GridPolygon> RandomMerge(List<GridPolygon> polygonList, float mergeThreshold)
    {
        this.mergeThreshold = mergeThreshold;
        System.Random random = new System.Random();
        List<GridPolygon> mergedList = new List<GridPolygon>();
        // Queue<GridPolygon> polygonQueue = new Queue<GridPolygon>(polygonList);
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
                HashSet<GridVertex> vertexSet1 = new HashSet<GridVertex>(head.gridVertexList);
                HashSet<GridVertex> vertexSet2 = new HashSet<GridVertex>(other.gridVertexList);
                vertexSet1.IntersectWith(vertexSet2);
                int intersectSize = vertexSet1.Count;
                if (intersectSize != 2)
                {
                    continue;
                }
                // polygonSubList.Add(new GridPolygon(other));
                polygonSubList.Add(other);
            }
            if(polygonSubList.Count == 0)
            {
                mergedList.Add(head);
                continue;
            }
            GridPolygon tail = polygonSubList[random.Next(0, polygonSubList.Count - 1)];
            polygonList.Remove(tail);
            HashSet<GridVertex> headVertex = new HashSet<GridVertex>(head.gridVertexList);
            HashSet<GridVertex> tailVertex = new HashSet<GridVertex>(tail.gridVertexList);
            headVertex.UnionWith(tailVertex);
            GridPolygon mergePolygon = new GridPolygon(new List<GridVertex>(headVertex));
            mergedList.Add(mergePolygon);
        }
        int siz2 = mergedList.Count;
        Debug.Log("Grid modifier random merge finished threshold=" + mergeThreshold.ToString() + ", from size " + siz1.ToString() + " --> " + siz1.ToString());
        return mergedList;
    }

}
