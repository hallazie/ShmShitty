using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelaunatorSharp;

public class GridGenerator
{

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
            List<GridVertex> triangleVertexList = new List<GridVertex>();
            foreach (IPoint point in triangle.Points)
            {
                triangleVertexList.Add(new GridVertex((float)point.X, (float)point.Y));
            }
            GridPolygon polygon = new GridPolygon(triangleVertexList);
            polygonList.Add(polygon);
        }
        return polygonList;

    }

}
