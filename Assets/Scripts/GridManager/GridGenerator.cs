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

    public Dictionary<Vector3, CornerElement> cornerElementDict = new Dictionary<Vector3, CornerElement>();
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
            foreach (CornerElement elem in cornerElementDict.Values)
            {
                Gizmos.DrawSphere(elem.transform.position, 0.025f);
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
                        Vector3 cornerCoord = new Vector3(adjPolygon.center.x, floor * floorHeight, adjPolygon.center.y);
                        CornerElement cornerElementInst = Instantiate(cornerElement, Vector3.zero, Quaternion.identity, this.transform);
                        cornerElementInst.Instantiate(floor, cornerCoord);
                        gridElementInst.cornerList.Add(cornerElementInst);
                        if (!cornerElementDict.ContainsKey(cornerCoord))
                        {
                            cornerElementDict.Add(cornerCoord, cornerElementInst);
                        }
                    }
                }
            }
        }
        Debug.Log("grid element list init finished with size: " + gridElementDict.Count + " and corner element list init finished with size: " + cornerElementDict.Count);
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
}



