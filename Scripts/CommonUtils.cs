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

    public static List<GridVertex> GetSharedVertexBetweenPolygon(GridPolygon polygon1, GridPolygon polygon2)
    {
        List<GridVertex> sharedList = new List<GridVertex>();
        foreach (GridVertex v1 in polygon1.gridVertexList)
        {
            foreach (GridVertex v2 in polygon2.gridVertexList)
            {
                if (v1.x == v2.x && v1.y == v2.y)
                {
                    sharedList.Add(v1);
                }
            }
        }
        return sharedList;
    }

    public static GridPolygon GetEncircledPolygonOfVertex(GridVertex vertex)
    {
        List<GridVertex> centerList = vertex.adjecentPolygonList.Select(obj => obj.center).ToList();
        for (int i = 0; i < vertex.adjecentPolygonList.Count; i++)
        {
            int j = i == vertex.adjecentPolygonList.Count - 1 ? 0 : i + 1;
            List<GridVertex> sharedList = GetSharedVertexBetweenPolygon(vertex.adjecentPolygonList[i], vertex.adjecentPolygonList[j]);
            if (sharedList.Count != 2)
                continue;
            GridVertex lineCenter = FindLineCenter(sharedList[0], sharedList[1]);
            centerList.Add(lineCenter);
        }
        return new GridPolygon(centerList);
    }

}



