using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphVisualizeOperator : MonoBehaviour
{

    public QuadGraph quadGrid;
    public Material lineMat;

    private LineRenderer line;
    private GraphVertex currentHoverOverVertex = null;

    public void Start()
    {
        // quadGrid = GetComponent<QuadGrid>();
    }

    private void MakeHighlightQuad(GraphVertex vertex)
    {

        List<GameObject> formerQuadList = GameObject.FindGameObjectsWithTag("VirtualPolygonLine").ToList();
        foreach (GameObject formerQuad in formerQuadList)
        {
            Destroy(formerQuad);
        }

        // Debug.Log("adjecent polygons: " + string.Join("; ", vertex.adjecentPolygonList.Select(obj => obj.center.ToString())));

        GraphPolygon virtualPolygon = CommonUtils.GetEncircledPolygonOfVertex(vertex);
        for (int i = 0; i < virtualPolygon.cornerGraphVertexList.Count; i++)
        {
            int j = (i == virtualPolygon.cornerGraphVertexList.Count - 1) ? 0 : i + 1;
            line = new GameObject("virtualPolygonLine").AddComponent<LineRenderer>();
            line.transform.parent = this.transform;
            line.tag = "VirtualPolygonLine";
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.startColor = new Color32(105, 65, 225, 200);
            line.endColor = new Color32(105, 65, 225, 200);
            line.positionCount = 2;
            line.startWidth = 0.025f;
            line.endWidth = 0.025f;
            line.useWorldSpace = true;
            line.numCapVertices = 10;
            line.SetPosition(0, virtualPolygon.cornerGraphVertexList[i].ToVector3());
            line.SetPosition(1, virtualPolygon.cornerGraphVertexList[j].ToVector3());
            line = null;
        }
    }

    public void HighlightHoverVertex(Vector3 position)
    {
        GraphVertex positionVertex = new GraphVertex(position.x, position.z);
        float closestDistance = 99999;
        GraphVertex closestVertex = null;
        foreach (GraphVertex vertex in quadGrid.vertexList)
        {
            float currentDistance = CommonUtils.GraphVertexEuclideanDistance(vertex, positionVertex);
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
