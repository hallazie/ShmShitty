using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex: MonoBehaviour
{

    private float _x;
    private float _y;
    private string _vertexType;                  // only surpport center / innerVertex / outterVertex
    private List<Vertex> _adjecentVertex;
    private List<Vertex> _adjecentPolygon;
    private Polygon _circumPolygon;

    public float x
    {
        set
        {
            this._x = Mathf.Round(value * 100f) / 100f;
        }
        get
        {
            return _x;
        }
    }
    
    public float y
    {
        set
        {
            this._y = Mathf.Round(value * 100f) / 100f;
        }
        get
        {
            return _y;
        }
    }
    
    public Vertex(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
    
    
    
}