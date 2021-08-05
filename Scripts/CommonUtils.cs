using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommonUtils : MonoBehaviour
{

    public static float GridVertexEuclideanDistance(GridVertex vertex1, GridVertex vertex2)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(vertex1.x - vertex2.x, 2) + Mathf.Pow(vertex1.y - vertex2.y, 2));
        return distance;
    }

    public static GridVertex FindLineCenter(GridVertex v1, GridVertex v2)
    {
        return new GridVertex((v1.x + v2.x) / 2.0f, (v1.y + v2.y) / 2.0f);
    }

    public static List<GridVertex> Deduplicate(List<GridVertex> list)
    {
        List<GridVertex> deduplicatedList = new List<GridVertex>();
        foreach (GridVertex vertex in list)
        {
            bool unseen = true;
            foreach (GridVertex dedup in deduplicatedList)
            {
                if (dedup.x == vertex.x && dedup.y == vertex.y)
                {
                    unseen = false;
                    break;
                }
            }
            if (unseen)
            {
                deduplicatedList.Add(vertex);
            }
        }
        Debug.Log("Grid generator deduplicate from size: " + list.Count + " --> size: " + deduplicatedList.Count);
        return deduplicatedList;
    }

    public static List<GridVertex> SortVertexListAroundPivotClockwise(List<GridVertex> vertexList, GridVertex pivot)
    {
        return vertexList.OrderBy(x => Mathf.Atan2(x.x - pivot.x, x.y - pivot.y)).ToList();
    }

    public static GridVertex FindClosestVertexForTarget(List<GridVertex> vertexList, GridVertex traget)
    {
        GridVertex minVertex = vertexList[0];
        float minDistance = CommonUtils.GridVertexEuclideanDistance(minVertex, traget);
        for (int i = 1; i < vertexList.Count; i++)
        {
            float currentDistance = CommonUtils.GridVertexEuclideanDistance(vertexList[i], traget);
            if (currentDistance <= minDistance)
            {
                minDistance = currentDistance;
                minVertex = vertexList[i];
            }
        }
        return minVertex;
    }

}



