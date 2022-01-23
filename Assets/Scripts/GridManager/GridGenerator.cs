using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{

    private List<GraphPolygon> graphPolygonList = new List<GraphPolygon>();
    private List<GraphVertex> graphVertixList = new List<GraphVertex>();
    private List<GraphPolygon> quadPolygonList = new List<GraphPolygon>();
    private List<GraphPolygon> mergedPolygonList = new List<GraphPolygon>();
    private List<GraphPolygon> relaxPolygonList = new List<GraphPolygon>();

    public CornerElement cornerElement;
    public GridElement gridElement;

    // public Dictionary<Vector3, CornerElement> cornerElementDict = new Dictionary<Vector3, CornerElement>();
    public List<CornerElement> cornerElementList = new List<CornerElement>();
    public Dictionary<Vector3, GridElement> gridElementDict = new Dictionary<Vector3, GridElement>();

    public int layerNumber = 10;
    public float mergeProb = 0.8f;
    public int learningEpoch = 100;
    public float learningRate = 0.5f;
    public bool fixAlpha = true;

    HexagonGraphVertexSampler graphVertexSampler;
    GraphGenearator graphGenerator;
    GraphModifier graphModifier;
    GraphRelaxation graphRelaxation;

    public int floorNumber = 5;
    public float floorHeight = 1.0f;

    public bool showCorners = true;

    void Start()
    {
        graphVertexSampler = new HexagonGraphVertexSampler();
        graphGenerator = new GraphGenearator();
        graphModifier = new GraphModifier();
        graphRelaxation = new GraphRelaxation(learningRate, fixAlpha);

        InitQuadGraph();
        AddAdjecentPolygonToVertex();
        RemoveEdgingVertex();
        InitQuadGrid();
    }

    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (GridElement elem in gridElementDict.Values)
        {
            Gizmos.DrawSphere(elem.transform.position, 0.05f);
        }
        if (showCorners)
        {
            Gizmos.color = Color.red;
            foreach (CornerElement elem in cornerElementList)
            {
                Gizmos.DrawSphere(elem.transform.position, 0.025f);
                Gizmos.DrawLine(elem.transform.position, elem.prevCoord);
                Gizmos.DrawLine(elem.transform.position, elem.nextCoord);
                if(elem.floor < floorNumber-1)
                {
                    Gizmos.DrawLine(elem.transform.position, new Vector3(elem.transform.position.x, elem.transform.position.y+1, elem.transform.position.z));
                }
            }
        }

    }

    private void InitQuadGrid()
    {
        foreach (GraphVertex vertex in graphVertixList)
        {
            for (int floor = 0; floor < floorNumber; floor++)
            {
                Vector3 coord = new Vector3(vertex.x, floor * floorHeight, vertex.y);
                GridElement gridElementInst = Instantiate(gridElement, Vector3.zero, Quaternion.identity, this.transform);
                gridElementInst.Instantiate(floor, coord);
                if (!gridElementDict.ContainsKey(coord))
                {
                    gridElementDict.Add(coord, gridElementInst);
                }

                foreach (GraphPolygon adjPolygon in relaxPolygonList)
                {
                    if (adjPolygon.cornerGraphVertexList.Contains(vertex))
                    {
                        Vector3 prevCoord = Vector3.zero;
                        Vector3 nextCoord = Vector3.zero;
                        adjPolygon.SortGraphVertexClockwise();
                        for (int i = 0; i < adjPolygon.cornerGraphVertexList.Count; i++)
                        {
                            if (adjPolygon.cornerGraphVertexList[i] == vertex)
                            {
                                int prevIndex = i == 0 ? adjPolygon.cornerGraphVertexList.Count - 1 : i - 1;
                                int nextIndex = i == adjPolygon.cornerGraphVertexList.Count - 1 ? 0 : i + 1;
                                prevCoord = new Vector3((adjPolygon.cornerGraphVertexList[prevIndex].x + vertex.x) / 2.0f, floor, (adjPolygon.cornerGraphVertexList[prevIndex].y + vertex.y) / 2.0f);
                                nextCoord = new Vector3((adjPolygon.cornerGraphVertexList[nextIndex].x + vertex.x) / 2.0f, floor, (adjPolygon.cornerGraphVertexList[nextIndex].y + vertex.y) / 2.0f);
                            }
                        }
                        Vector3 cornerCoord = new Vector3(adjPolygon.center.x, floor * floorHeight, adjPolygon.center.y);
                        CornerElement cornerElementInst = Instantiate(cornerElement, Vector3.zero, Quaternion.identity, this.transform);
                        cornerElementInst.Instantiate(floor, cornerCoord);
                        cornerElementInst.prevCoord = prevCoord;
                        cornerElementInst.nextCoord = nextCoord;
                        gridElementInst.cornerList.Add(cornerElementInst);
                        cornerElementList.Add(cornerElementInst);
                    }
                }
            }
        }
        Debug.Log("grid element list init finished with size: " + gridElementDict.Count + " and corner element list init finished with size: " + cornerElementList.Count);
    }

    private void InitQuadGraph()
    {
        graphVertixList = graphVertexSampler.Generate(this.layerNumber);
        graphPolygonList = graphGenerator.GenerateDelaunayGraph(graphVertixList);
        mergedPolygonList = graphModifier.RandomMerge(graphPolygonList, this.mergeProb);
        quadPolygonList = graphModifier.SplitToQuads(mergedPolygonList, graphVertixList);
        relaxPolygonList = graphRelaxation.Relaxation(quadPolygonList, this.learningEpoch, this.learningRate);
        Debug.Log("polygon init finished with size: " + relaxPolygonList.Count);
    }

    private void AddAdjecentPolygonToVertex()
    {
        foreach (GraphVertex vertex in graphVertixList)
        {
            foreach (GraphPolygon polygon in relaxPolygonList)
            {
                if (polygon.cornerGraphVertexList.Contains(vertex))
                {
                    vertex.AddToAdjcentPolygonList(polygon);
                }
            }
        }
    }

    private void RemoveEdgingVertex()
    {
        List<GraphVertex> removeList = new List<GraphVertex>();
        foreach (GraphVertex vertex in graphVertixList)
        {
            float angleSum = 0.0f;
            if (vertex.adjecentPolygonList.Count < 2)
            {
                removeList.Add(vertex);
                continue;
            }
            for(int i = 0; i < vertex.adjecentPolygonList.Count; i++)
            {
                int j = i != vertex.adjecentPolygonList.Count - 1 ? i + 1 : 0;
                Vector3 v1 = new Vector3(vertex.adjecentPolygonList[i].center.x - vertex.x, 0, vertex.adjecentPolygonList[i].center.y - vertex.y);
                Vector3 v2 = new Vector3(vertex.adjecentPolygonList[j].center.x - vertex.x, 0, vertex.adjecentPolygonList[j].center.y - vertex.y);
                float angle = Vector3.Angle(v1, v2);
                angleSum += angle;
            }
            if (Mathf.Abs(angleSum - 360) > 0.01)
            {
                Debug.Log("angle sum for vertex (" + vertex.x + ", " + vertex.y + "): " + angleSum);
                removeList.Add(vertex);
            }
        }
        foreach (GraphVertex removeVertex in removeList)
        {
            if (graphVertixList.Contains(removeVertex))
            {
                graphVertixList.Remove(removeVertex);
            }
        }    
    }
}



