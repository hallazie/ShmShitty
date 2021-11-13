using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridVertex

    /*
     center of a polygon
     */

{

    private float _x;
    private float _y;
    private float _z;
    public int layer;
    public int hexTraverseLayer;
    public string type;
    private List<GridVertex> _adjecentVertexList = new List<GridVertex>();
    private List<GridPolygon> _adjecentPolygonList = new List<GridPolygon>();
    private List<GridCorner> _cornerList = new List<GridCorner>();

    public GridVertex(float x, float y, float z=0, int layer=-1, string type="Grid Vertex")
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.layer = layer;
        this.type = type;
    }

    public float x
    {
        set
        {
            this._x = Mathf.Round(value * 1000f) / 1000f;
        }
        get
        {
            return this._x;
        }
    }

    public float y
    {
        set
        {
            this._y = Mathf.Round(value * 1000f) / 1000f;
        }
        get
        {
            return this._y;
        }
    }

    public float z
    {
        set
        {
            this._z = Mathf.Round(value * 1000f) / 1000f;
        }
        get
        {
            return this._z;
        }
    }

    public List<GridPolygon> adjecentPolygonList
    {
        set
        {
            if(value.GetType() == typeof(List<GridPolygon>) && value.Count > 0)
            {
                this._adjecentPolygonList = value;
                if (this._adjecentPolygonList.Count > 1)
                    ResortAdjecentPolygonList();
            }
        }
        get
        {
            if (this._adjecentPolygonList.Count > 1)
                ResortAdjecentPolygonList();
            return this._adjecentPolygonList;
        }
    }

    private void ResortAdjecentPolygonList()
    {
        try
        {
            this._adjecentPolygonList = this._adjecentPolygonList.OrderBy(x => Mathf.Atan2(x.center.x - this.x, x.center.y - this.y)).ToList();
        }
        catch (System.Exception)
        {
            Debug.LogError("sort polygon vertex error with polygon: " + string.Join("; ", this._adjecentPolygonList.Select(x => x.center.ToString())));
            throw;
        }
    }

    public void AddToAdjcentPolygonList(GridPolygon polygon)
    {
        bool unseen = true;
        foreach (GridPolygon adjecent in adjecentPolygonList)
        {
            if (adjecent == polygon)
            {
                unseen = false;
            }
        }
        if (unseen)
        {
            adjecentPolygonList.Add(polygon);
        }
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
        // return base.GetHashCode();
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
