using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphPolygon
    /*
     the actual polygon
     */
{
    public List<GraphVertex> cornerGraphVertexList = new List<GraphVertex>();
    public string type;

    private int _floor;
    private GraphVertex _center = null;
    private float _sideLength;

    public GraphPolygon(List<GraphVertex> vertexList)
    {
        cornerGraphVertexList = vertexList;
        SortGraphVertexClockwise();
    }

    public GraphPolygon(GraphPolygon other)
    {
        this.cornerGraphVertexList.Clear();
        foreach (GraphVertex vertex in other.cornerGraphVertexList)
        {
            this.cornerGraphVertexList.Add(new GraphVertex(vertex.x, vertex.y, vertex.layer));
        }
        SortGraphVertexClockwise();
    }

    public GraphVertex center
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
            if(_sideLength == 0.0f && cornerGraphVertexList.Count >= 2)
            {
                for(int i=0; i<cornerGraphVertexList.Count; i++)
                {
                    int indexNext = i + 1;
                    if (i == cornerGraphVertexList.Count - 1)
                        indexNext = 0;
                    _sideLength += CommonUtils.GraphVertexEuclideanDistance(cornerGraphVertexList[i], cornerGraphVertexList[indexNext]);
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
         only use inner graph vertex to calc hash
         */

        // TODO use round x, round y to calc hash

        // unchecked
        // {
        //     int hash = 17;
        //     foreach (GraphVertex vertex in graphVertexList)
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
        GraphPolygon other = (GraphPolygon)obj;
        foreach (GraphVertex vertex in cornerGraphVertexList)
        {
            GraphVertex closest = CommonUtils.FindClosestVertexForTarget(other.cornerGraphVertexList, vertex);
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
        foreach (GraphVertex vertex in cornerGraphVertexList)
        {
            sumX += vertex.x;
            sumY += vertex.y;
        }
        float avgX = sumX / (float)cornerGraphVertexList.Count;
        float avgY = sumY / (float)cornerGraphVertexList.Count;
        float avgZ = 0.0f;
        if (this.floor == 0)
        {
            avgZ = GlobalConst.BASEMENT_HEIGHT / 2.0f;
        }
        else if (this.center.upperAdjecentVertex == null)
        {
            avgZ = GlobalConst.ROOF_HEIGHT / 2.0f;
        }
        else
        {
            avgZ = GlobalConst.LAYER_HEIGHT / 2.0f;
        }
        _center = new GraphVertex(avgX, avgY, avgZ);
    }

    private float ClockwiseDistance(GraphVertex x, GraphVertex y)
    {
        return (x.x - center.x) * (y.y - center.y) * (y.x - center.x) * (x.y - center.y);
    }

    private int ClockwiseComparison(GraphVertex x, GraphVertex y)
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

    public void SortGraphVertexClockwise()
    {
        // 好用！
        cornerGraphVertexList = cornerGraphVertexList.OrderBy(x => Mathf.Atan2(x.x - center.x, x.y - center.y)).ToList();
    }

    public List<GraphPolygon> SplitToQuads()
    {
        GraphVertex centeroid = new GraphVertex(this.center);
        List<GraphPolygon> splitList = new List<GraphPolygon>();
        for (int i = 0; i < cornerGraphVertexList.Count; i++)
        {
            int indexNext = i + 1;
            int indexPrev = i - 1;
            if (i == cornerGraphVertexList.Count - 1)
                indexNext = 0;
            if (i == 0)
                indexPrev = cornerGraphVertexList.Count - 1;
            GraphVertex c1 = CommonUtils.FindLineCenter(cornerGraphVertexList[indexPrev], cornerGraphVertexList[i]);
            GraphVertex c2 = CommonUtils.FindLineCenter(cornerGraphVertexList[i], cornerGraphVertexList[indexNext]);
            GraphPolygon polygon = new GraphPolygon(new List<GraphVertex> { c1, c2, cornerGraphVertexList[i], centeroid });
            splitList.Add(polygon);
        }
        return splitList;
    }

}
