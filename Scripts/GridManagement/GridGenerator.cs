using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelaunatorSharp;

public class GridGenerator : MonoBehaviour
{

    List<GridPolygon> GenerateDelaunayGrid(List<GridVertex> vertexList)
    {
        DelaunatorSharp.Point[] pointList = new DelaunatorSharp.Point[vertexList.Count];
        for(int i = 0; i < vertexList.Count; i++)
        {
            pointList[i] = new DelaunatorSharp.Point(vertexList[i].x, vertexList[i].y);
        }
        Delaunator delaunator = new Delaunator(pointList);
    }

}
