using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisualizeOperator : MonoBehaviour
{

    public QuadGrid quadGrid;
    public Material lineMat;

    private LineRenderer line;
    private GridVertex currentHoverOverVertex = null;

    public void Start()
    {
        // quadGrid = GetComponent<QuadGrid>();
    }

    public void OnDrawGizmos()
    {
        return;
        foreach (GridVertex vertex in quadGrid.vertexList)
        {
            Gizmos.DrawSphere(new Vector3(vertex.x, 0, vertex.y), 0.1f);
        }
    }

    private void MakeHighlightQuad(GridVertex vertex)
    {

        List<GameObject> formerQuadList = GameObject.FindGameObjectsWithTag("VirtualPolygonLine").ToList();
        foreach (GameObject formerQuad in formerQuadList)
        {
            Destroy(formerQuad);
        }

        // Debug.Log("adjecent polygons: " + string.Join("; ", vertex.adjecentPolygonList.Select(obj => obj.center.ToString())));

        GridPolygon virtualPolygon = CommonUtils.GetEncircledPolygonOfVertex(vertex);
        for (int i = 0; i < virtualPolygon.gridVertexList.Count; i++)
        {
            int j = (i == virtualPolygon.gridVertexList.Count - 1) ? 0 : i + 1;
            line = new GameObject("virtualPolygonLine").AddComponent<LineRenderer>();
            line.tag = "VirtualPolygonLine";
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.startColor = new Color32(105, 65, 225, 200);
            line.endColor = new Color32(105, 65, 225, 200);
            line.positionCount = 2;
            line.startWidth = 0.025f;
            line.endWidth = 0.025f;
            line.useWorldSpace = true;
            line.numCapVertices = 10;
            line.SetPosition(0, virtualPolygon.gridVertexList[i].ToVector3());
            line.SetPosition(1, virtualPolygon.gridVertexList[j].ToVector3());
            line = null;
        }
    }

    public void HighlightHoverVertex(Vector3 position)
    {
        GridVertex positionVertex = new GridVertex(position.x, position.z);
        float closestDistance = 99999;
        GridVertex closestVertex = null;
        foreach (GridVertex vertex in quadGrid.vertexList)
        {
            float currentDistance = CommonUtils.GridVertexEuclideanDistance(vertex, positionVertex);
            if (currentDistance <= closestDistance)
            {
                closestDistance = currentDistance;
                closestVertex = vertex;
            }
        }
        if (closestVertex == null)
            return;
        if (currentHoverOverVertex == null)
        {
            currentHoverOverVertex = closestVertex;
        }
        if (currentHoverOverVertex != closestVertex)
        {
            Debug.Log("current hover over polygon change from " + currentHoverOverVertex.ToString() + " ---> " + currentHoverOverVertex.ToString());
            currentHoverOverVertex = closestVertex;
            MakeHighlightQuad(currentHoverOverVertex);
        }
        
    }

}
