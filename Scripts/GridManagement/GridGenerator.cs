using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DelaunatorSharp;

public class GridGenerator
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

    public List<GridPolygon> GenerateDelaunayGrid(List<GridVertex> vertexList)
    {
        IPoint[] pointList = new IPoint[vertexList.Count];
        List<GridPolygon> polygonList = new List<GridPolygon>();
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
            List<GridVertex> triangleVertexList = new List<GridVertex>();
            foreach (IPoint point in triangle.Points)
            {
                // triangleVertexList.Add(new GridVertex((float)point.X, (float)point.Y));
                foreach (GridVertex vertex in vertexList)
                {
                    if ( vertex.x == point.X && vertex.y == point.Y)
                    {
                        triangleVertexList.Add(vertex);
                        break;
                    }
                }
            }
            GridPolygon polygon = new GridPolygon(triangleVertexList);
            polygonList.Add(polygon);
        }
        Debug.Log("grid-vertex size: " + vertexList.Count);

        return polygonList;

    }
}
