using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVertex
{

    public float x;
    public float y;
    public int layer;
    public List<GridVertex> adjecentVertexList;
    public List<int> adjecentPolygonList;

    public GridVertex(float x, float y, int layer=-1)
    {
        this.x = x;
        this.y = y;
        this.layer = layer;
    }

    public GridVertex(GridVertex other)
    {
        this.x = other.x;
        this.y = other.y;
        this.layer = other.layer;
    }

    public override bool Equals(object other)
    {
        if ((other == null) || !this.GetType().Equals(other.GetType()))
        {
            return false;
        }
        GridVertex vertexOther = (GridVertex)other;
        return this.x == vertexOther.x && this.y == vertexOther.y;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + x.GetHashCode();
            hash = hash * 23 + y.GetHashCode();
            return hash;
        }
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, 0, y);
    }

    public override string ToString()
    {
        return string.Format("Vertex: {0}, {1}", x, y);
    }

}
