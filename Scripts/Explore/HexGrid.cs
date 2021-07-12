using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{

    public int hexGridSize = 3;

    private List<Vector3> offsetList = new List<Vector3>{
        new Vector3(0, 0, 2),
        new Vector3(Mathf.Sqrt(3), 0, 1),
        new Vector3(Mathf.Sqrt(3), 0, -1),
        new Vector3(0, 0, -2),
        new Vector3(-Mathf.Sqrt(3), 0, -1),
        new Vector3(-Mathf.Sqrt(3), 0, 1)
    };
    [Range(0, 1)]
    public float triangleMergeProb = 0.5f;

    private List<Vertex> vertexList = new List<Vertex>();

    private List<GridTriad> gridTriadList = new List<GridTriad>();

    // Start is called before the first frame update
    void Start()
    {
        InitTriaGrid();
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
        Vertex origin = new Vertex(new Vector3(0, 0, 0), 0);
        Queue<Vertex> queue = new Queue<Vertex>();
        queue.Enqueue(origin);
        TraverseHex(queue, vertexList);
        ConstructTriangleList();
        Debug.Log("basic hex grid finished with size: " + vertexList.Count.ToString() + " and grid-triad size: " + gridTriadList.Count.ToString());

    }

    private void TraverseHex(Queue<Vertex> queue, List<Vertex> vertexList)
    {
        if(queue.Count <= 0)
        {
            return;
        }
        Vertex origin = queue.Dequeue();
        Debug.Log("current origin: " + origin.coordinate.x + ", " + origin.coordinate.z + ", with step: " + origin.layer + " and vertex size: " + vertexList.Count);
        if (origin.layer >= hexGridSize) {
            return;
        }
        foreach (Vector3 offset in offsetList)
        {
            Vertex newVertex = new Vertex(new Vector3(origin.coordinate.x + offset.x, 0, origin.coordinate.z + offset.z), origin.layer+1);
            bool flag = true;
            foreach (Vertex vertex in vertexList)
            {
                if(vertex.coordinate.x == newVertex.coordinate.x && vertex.coordinate.z == newVertex.coordinate.z)
                {
                    flag = false;
                    break;
                }
            }
            if(flag)
            {
                vertexList.Add(newVertex);
                queue.Enqueue(newVertex);
            }
        }
        TraverseHex(queue, vertexList);

    }

    private void OnDrawGizmos()
    {
        foreach (Vertex vertex in vertexList)
        {
            Gizmos.DrawSphere(new Vector3(vertex.coordinate.x, 0, vertex.coordinate.z), 0.1f);
        }
        foreach (GridTriad gridTriad in gridTriadList)
        {
            Gizmos.DrawLine(gridTriad.vertexCoordList[0].coordinate, gridTriad.vertexCoordList[1].coordinate);
            Gizmos.DrawLine(gridTriad.vertexCoordList[1].coordinate, gridTriad.vertexCoordList[2].coordinate);
            Gizmos.DrawLine(gridTriad.vertexCoordList[2].coordinate, gridTriad.vertexCoordList[0].coordinate);
        }

    }

    private void ConstructTriangleList()
    {
        foreach (Vertex vertex in vertexList)
        {
            float nb1x = vertex.coordinate.x + offsetList[0].x;
            float nb1z = vertex.coordinate.z + offsetList[0].z;
            float nb2x = vertex.coordinate.x + offsetList[1].x;
            float nb2z = vertex.coordinate.z + offsetList[1].z;
            Vertex neighbor1 = null;
            Vertex neighbor2 = null;
            foreach (Vertex otherVertex in vertexList)
            {
                if (otherVertex.coordinate.x == nb1x && otherVertex.coordinate.z == nb1z)
                    neighbor1 = otherVertex;
                if (otherVertex.coordinate.x == nb2x && otherVertex.coordinate.z == nb2z)
                    neighbor2 = otherVertex;
            }
            if (neighbor1 != null && neighbor2 != null)
            {
                GridTriad gridTiad = new GridTriad();
                gridTiad.vertexCoordList.Add(vertex);
                gridTiad.vertexCoordList.Add(neighbor1);
                gridTiad.vertexCoordList.Add(neighbor2);
                gridTriadList.Add(gridTiad);
            }
        }
    }

}


public class Vertex
{

    public Vector3 coordinate;
    public int layer;

    public Vertex(Vector3 coord)
    {
        this.coordinate = coord;
    }

    public Vertex(Vector3 coord, int layer)
    {
        this.coordinate = coord;
        this.layer = layer;
    }
}

public class GridBasic
{
    public List<Vertex> vertexCoordList;

    public GridBasic() {
        this.vertexCoordList = new List<Vertex>();
    }

    public GridBasic(List<Vertex> vertexCoordList)
    {
        this.vertexCoordList = vertexCoordList;
    }

    public Vector2 GetCenter()
    {
        float x = 0.0f; 
        float y = 0.0f;
        foreach (Vertex coord in vertexCoordList)
        {
            x += coord.coordinate.x;
            y += coord.coordinate.y;
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

public class GridTriad: GridBasic
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
