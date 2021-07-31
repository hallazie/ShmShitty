using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridPolygon
{

    public List<GridVertex> gridVertexList = new List<GridVertex>();
    public string type;

    private GridVertex _center = null;
    private float _sideLength;

    public GridPolygon(List<GridVertex> vertexList)
    {
        gridVertexList = vertexList;
        SortGridVertexClockwise();
    }

    public GridPolygon(GridPolygon other)
    {
        this.gridVertexList.Clear();
        foreach (GridVertex vertex in other.gridVertexList)
        {
            this.gridVertexList.Add(new GridVertex(vertex.x, vertex.y, vertex.layer));
        }
        SortGridVertexClockwise();
    }

    public GridVertex center
    {
        get
        {
            if(_center == null)
            {
                RecalculateCenter();
            }
            return _center;
        }
        set
        {
            Debug.LogWarning("Polygon not support center assigning");
        }
    }

    public float sideLength
    {
        get
        {
            if(_sideLength == 0.0f && gridVertexList.Count >= 2)
            {
                for(int i=0; i<gridVertexList.Count; i++)
                {
                    int indexNext = i + 1;
                    if (i == gridVertexList.Count - 1)
                        indexNext = 0;
                    _sideLength += CommonUtils.GridVertexEuclideanDistance(gridVertexList[i], gridVertexList[indexNext]);
                }
            }
            return _sideLength;
        }
        set
        {
            Debug.LogWarning("Polygon not support sideLength assigning");
        }
    }

    public void RecalculateCenter()
    {
        float sumX = 0.0f;
        float sumY = 0.0f;
        foreach (GridVertex vertex in gridVertexList)
        {
            sumX += vertex.x;
            sumY += vertex.y;
        }
        float avgX = sumX / (float)gridVertexList.Count;
        float avgY = sumY / (float)gridVertexList.Count;
        _center = new GridVertex(avgX, avgY);
    }

    private float ClockwiseDistance(GridVertex x, GridVertex y)
    {
        return (x.x - center.x) * (y.y - center.y) * (y.x - center.x) * (x.y - center.y);
    }

    private int ClockwiseComparison(GridVertex x, GridVertex y)
    {
        if (x.x - center.x >= 0 && y.x - center.x < 0)
        {
            return 1;
        }
        if (x.x - center.x < 0 && y.x - center.x >= 0)
        {
            return 0;
        }
        if (x.x - center.x == 0 && y.x - center.x == 0)
        {
            if (x.y - center.y >= 0 || y.y - center.y >= 0)
            {
                return x.y > y.y ? 1 : 0;
            }
            return y.y > x.y ? 1 : 0;
        }
        // float det = ClockwiseDistance(x, y);
        float det = (x.x - center.x) * (y.y - center.y) - (y.x - center.x) * (x.y - center.y);
        if (det < 0)
        {
            return 1;
        }
        else if (det > 0)
        {
            return 0;
        }
        float d1 = (x.x - center.x) * (x.x - center.x) + (x.y - center.y) * (x.y - center.y);
        float d2 = (y.x - center.x) * (y.x - center.x) + (y.y - center.y) * (y.y - center.y);
        return d1 > d2 ? 1 : 0;
    }

    public void SortGridVertexClockwise()
    {
        // if (gridVertexList.Count > 3)
        //    Debug.Log("before sort: " + string.Join("; ", gridVertexList.Select(obj => obj.ToString())));
        //    Debug.Log("actan2: " + string.Join("; ", gridVertexList.Select(obj => Mathf.Atan2(obj.x - center.x, obj.y - center.y).ToString())));

        // gridVertexList = gridVertexList.Sort((x, y) => ClockwiseComparison(x, y)).ToList();
        gridVertexList = gridVertexList.OrderBy(x => Mathf.Atan2(x.x - center.x, x.y - center.y)).ToList();

        // if (gridVertexList.Count > 3)
        //    Debug.Log("after sort: " + string.Join("; ", gridVertexList.Select(obj => obj.ToString())));
    }

    public GridVertex FindLineCenter(GridVertex v1, GridVertex v2)
    {
        return new GridVertex((v1.x + v2.x) / 2.0f, (v1.y + v2.y) / 2.0f);
    }

    public List<GridPolygon> SplitToQuads()
    {
        GridVertex centeroid = new GridVertex(this.center);
        List<GridPolygon> splitList = new List<GridPolygon>();
        for (int i = 0; i < gridVertexList.Count; i++)
        {
            int indexNext = i + 1;
            int indexPrev = i - 1;
            if (i == gridVertexList.Count - 1)
                indexNext = 0;
            if (i == 0)
                indexPrev = gridVertexList.Count - 1;
            GridVertex c1 = FindLineCenter(gridVertexList[indexPrev], gridVertexList[i]);
            GridVertex c2 = FindLineCenter(gridVertexList[i], gridVertexList[indexNext]);
            GridPolygon polygon = new GridPolygon(new List<GridVertex> { c1, c2, gridVertexList[i], centeroid });
            splitList.Add(polygon);
        }
        return splitList;
    }

}
