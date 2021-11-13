using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridPolygon
    /*
     the actual polygon
     */
{
    public List<GridVertex> lowerGridVertexList = new List<GridVertex>();
    public List<GridVertex> upperGridVertexList = new List<GridVertex>();
    public string type;
    private Collider collider;
    private Renderer renderer;

    private int _floor;
    private GridVertex _center = null;
    private float _sideLength;

    public GridPolygon(List<GridVertex> vertexList)
    {
        lowerGridVertexList = vertexList;
        SortGridVertexClockwise();
    }

    public GridPolygon(GridPolygon other)
    {
        this.lowerGridVertexList.Clear();
        foreach (GridVertex vertex in other.lowerGridVertexList)
        {
            this.lowerGridVertexList.Add(new GridVertex(vertex.x, vertex.y, vertex.layer));
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

    public int floor
    {
        get
        {
            return this._floor;
        }
        set
        {
            this._floor = value;
        }
    }

    public float sideLength
    {
        get
        {
            if(_sideLength == 0.0f && lowerGridVertexList.Count >= 2)
            {
                for(int i=0; i<lowerGridVertexList.Count; i++)
                {
                    int indexNext = i + 1;
                    if (i == lowerGridVertexList.Count - 1)
                        indexNext = 0;
                    _sideLength += CommonUtils.GridVertexEuclideanDistance(lowerGridVertexList[i], lowerGridVertexList[indexNext]);
                }
            }
            return _sideLength;
        }
        set
        {
            Debug.LogWarning("Polygon not support sideLength assigning");
        }
    }

    public override int GetHashCode()
    {
        /*
         only use inner grid vertex to calc hash
         */

        // TODO use round x, round y to calc hash

        // unchecked
        // {
        //     int hash = 17;
        //     foreach (GridVertex vertex in gridVertexList)
        //     {
        //         hash = hash * 23 + vertex.x.GetHashCode();
        //         hash = hash * 23 + vertex.y.GetHashCode();
        //     }
        //     return hash;
        // }
        return base.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        GridPolygon other = (GridPolygon)obj;
        foreach (GridVertex vertex in lowerGridVertexList)
        {
            GridVertex closest = CommonUtils.FindClosestVertexForTarget(other.lowerGridVertexList, vertex);
            if (vertex.x != closest.x || vertex.y != closest.y)
            {
                return false;
            }
        }
        return true;
    }

    public void RecalculateCenter()
    {
        float sumX = 0.0f;
        float sumY = 0.0f;
        foreach (GridVertex vertex in lowerGridVertexList)
        {
            sumX += vertex.x;
            sumY += vertex.y;
        }
        float avgX = sumX / (float)lowerGridVertexList.Count;
        float avgY = sumY / (float)lowerGridVertexList.Count;
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
        lowerGridVertexList = lowerGridVertexList.OrderBy(x => Mathf.Atan2(x.x - center.x, x.y - center.y)).ToList();
    }

    public List<GridPolygon> SplitToQuads()
    {
        GridVertex centeroid = new GridVertex(this.center);
        List<GridPolygon> splitList = new List<GridPolygon>();
        for (int i = 0; i < lowerGridVertexList.Count; i++)
        {
            int indexNext = i + 1;
            int indexPrev = i - 1;
            if (i == lowerGridVertexList.Count - 1)
                indexNext = 0;
            if (i == 0)
                indexPrev = lowerGridVertexList.Count - 1;
            GridVertex c1 = CommonUtils.FindLineCenter(lowerGridVertexList[indexPrev], lowerGridVertexList[i]);
            GridVertex c2 = CommonUtils.FindLineCenter(lowerGridVertexList[i], lowerGridVertexList[indexNext]);
            GridPolygon polygon = new GridPolygon(new List<GridVertex> { c1, c2, lowerGridVertexList[i], centeroid });
            splitList.Add(polygon);
        }
        return splitList;
    }

}
