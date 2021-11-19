using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommonUtils : MonoBehaviour
{

    public static float GraphVertexEuclideanDistance(GraphVertex vertex1, GraphVertex vertex2)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(vertex1.x - vertex2.x, 2) + Mathf.Pow(vertex1.y - vertex2.y, 2));
        return distance;
    }

    public static GraphVertex FindLineCenter(GraphVertex v1, GraphVertex v2)
    {
        return new GraphVertex((v1.x + v2.x) / 2.0f, (v1.y + v2.y) / 2.0f);
    }

    public static List<GraphVertex> Deduplicate(List<GraphVertex> list)
    {
        List<GraphVertex> deduplicatedList = new List<GraphVertex>();
        foreach (GraphVertex vertex in list)
        {
            bool unseen = true;
            foreach (GraphVertex dedup in deduplicatedList)
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

    public static List<GraphVertex> SortVertexListAroundPivotClockwise(List<GraphVertex> vertexList, GraphVertex pivot)
    {
        return vertexList.OrderBy(x => Mathf.Atan2(x.x - pivot.x, x.y - pivot.y)).ToList();
    }

    public static GraphVertex FindClosestVertexForTarget(List<GraphVertex> vertexList, GraphVertex traget)
    {
        GraphVertex minVertex = vertexList[0];
        float minDistance = CommonUtils.GraphVertexEuclideanDistance(minVertex, traget);
        for (int i = 1; i < vertexList.Count; i++)
        {
            float currentDistance = CommonUtils.GraphVertexEuclideanDistance(vertexList[i], traget);
            if (currentDistance <= minDistance)
            {
                minDistance = currentDistance;
                minVertex = vertexList[i];
            }
        }
        return minVertex;
    }

    public static List<GraphVertex> GetSharedVertexBetweenPolygon(GraphPolygon polygon1, GraphPolygon polygon2)
    {
        List<GraphVertex> sharedList = new List<GraphVertex>();
        foreach (GraphVertex v1 in polygon1.cornerGraphVertexList)
        {
            foreach (GraphVertex v2 in polygon2.cornerGraphVertexList)
            {
                if (v1.x == v2.x && v1.y == v2.y)
                {
                    sharedList.Add(v1);
                }
            }
        }
        return sharedList;
    }

    public static GraphPolygon GetEncircledPolygonOfVertex(GraphVertex vertex)
    {
        List<GraphVertex> centerList = vertex.adjecentPolygonList.Select(obj => obj.center).ToList();
        for (int i = 0; i < vertex.adjecentPolygonList.Count; i++)
        {
            int j = i == vertex.adjecentPolygonList.Count - 1 ? 0 : i + 1;
            List<GraphVertex> sharedList = GetSharedVertexBetweenPolygon(vertex.adjecentPolygonList[i], vertex.adjecentPolygonList[j]);
            if (sharedList.Count != 2)
                continue;
            GraphVertex lineCenter = FindLineCenter(sharedList[0], sharedList[1]);
            centerList.Add(lineCenter);
        }
        return new GraphPolygon(centerList);
    }

}



