using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DelaunatorSharp;

public class GraphGenearator
{

    private bool ValidTriangle(Triangle triangle)
    {
        float minDist = 999.0f;
        float maxDist = 0.0f;
        List<IPoint> pointList = triangle.Points.ToList();
        for (int i = 0; i < pointList.Count; i++)
        {
            int j = (i == pointList.Count - 1) ? 0 : i + 1;
            float currDist = Mathf.Pow((float)pointList[i].X - (float)pointList[j].X, 2) + Mathf.Pow((float)pointList[i].Y - (float)pointList[j].Y, 2);
            minDist = currDist < minDist ? currDist : minDist;
            maxDist = currDist > maxDist ? currDist : maxDist;
        }
        return Mathf.Abs(minDist - maxDist) < 0.5;
    }

    public List<GraphPolygon> GenerateDelaunayGraph(List<GraphVertex> vertexList)
    {
        IPoint[] pointList = new IPoint[vertexList.Count];
        List<GraphPolygon> polygonList = new List<GraphPolygon>();
        for(int i = 0; i < vertexList.Count; i++)
        {
            pointList[i] = new DelaunatorSharp.Point(vertexList[i].x, vertexList[i].y);
        }
        Delaunator delaunator = new Delaunator(pointList);
        if (delaunator == null)
            return polygonList;

        foreach (Triangle triangle in delaunator.GetTriangles())
        {
            if (!ValidTriangle(triangle))
                continue;
            List<GraphVertex> triangleVertexList = new List<GraphVertex>();
            foreach (IPoint point in triangle.Points)
            {
                foreach (GraphVertex vertex in vertexList)
                {
                    if ( vertex.x == point.X && vertex.y == point.Y)
                    {
                        triangleVertexList.Add(vertex);
                        break;
                    }
                }
            }
            GraphPolygon polygon = new GraphPolygon(triangleVertexList);
            polygonList.Add(polygon);
        }
        Debug.Log("graph-vertex size: " + vertexList.Count);

        return polygonList;

    }
}
