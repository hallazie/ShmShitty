using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{

    public int hexGridSize = 10;
    private List<Vector2> offsetList = new List<Vector2>{
        new Vector2(0, 2),
        new Vector2(2*Mathf.Sqrt(3), 1),
        new Vector2(2*Mathf.Sqrt(3), -1),
        new Vector2(0, -2),
        new Vector2(-2*Mathf.Sqrt(3), -1),
        new Vector2(-2*Mathf.Sqrt(3), 1)
    };

    [Range(0, 1)]
    public float triangleMergeProb = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        InitHexGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitHexGrid()
    {

    }

    private void InitTriaGrid()
    {
        Vertex origin = new Vertex(new Vector2(0, 0));

    }

    private void TraverseHex(Vertex origin, List<Vertex> vertexList, int steps)
    {
        if (steps == 0) {
            return;
        }
        
    }

    private void OnDrawGizmos()
    {
        Vector3 v1 = new Vector3(0, 1, 0);
        Vector3 v2 = new Vector3(9, 8, -5);
        Gizmos.DrawLine(v1, v2);
    }

}


public class Vertex
{

    public Vector2 coordinate;

    public Vertex(Vector2 coord)
    {
        this.coordinate = coord;
    }
}

public class GridBasic
{
    public List<Vector2> vertexCoordList;

    public GridBasic() { }

    public GridBasic(List<Vector2> vertexCoordList)
    {
        this.vertexCoordList = vertexCoordList;
    }

    public Vector2 GetCenter()
    {
        float x = 0.0f; 
        float y = 0.0f;
        foreach (Vector2 coord in vertexCoordList)
        {
            x += coord.x;
            y += coord.y;
        }
        x /= vertexCoordList.Count;
        y /= vertexCoordList.Count;
        Vector2 ret = new Vector2(x, y);
        return ret;
    }

}

public class GridQuad : GridBasic
{

}

public class GridTiad: GridBasic
{

}

public class HexagonGridCoord
{
    /*
     constrain: x + y + z = 0
     */
    public int x;
    public int y;
    public int z;

    public Vector2 GetCartesianCoordinates()
    {
        float cX = 0.0f;
        float cY = 0.0f;
        return new Vector2(cX, cY);
    }
}
