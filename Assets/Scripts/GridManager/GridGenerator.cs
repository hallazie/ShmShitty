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

    private List<CornerElement> cornerElementList = new List<CornerElement>();
    private List<GridElement> gridElementList = new List<GridElement>();

    public int layerNumber;
    public float mergeProb;
    public int learningEpoch;
    public float learningRate;
    public bool fixAlpha;

    HexagonGraphVertexSampler graphVertexSampler;
    GraphGenearator graphGenerator;
    GraphModifier graphModifier;
    GraphRelaxation graphRelaxation;

    public int floorNumber;
    public float floorHeight = 1.0f;

    void Start()
    {
        InitQuadGraph();
    }

    void Update()
    {
        
    }

    private void InitQuadGrid()
    {
        foreach (GraphVertex vertex in graphVertixList)
        {
            GraphPolygon encircledPolygon = CommonUtils.GetEncircledPolygonOfVertex(vertex);
            for (int floor = 0; floor < floorNumber; floor++)
            {
                Vector3 coord = new Vector3(vertex.x, floor * floorHeight, vertex.y);
                GridElement gridElement = new GridElement(floor, coord);
                foreach (GraphVertex corner in encircledPolygon.cornerGraphVertexList)
                {
                    CornerElement cornerElement = new CornerElement();
                    gridElement.cornerList.Add(cornerElement);
                }
            }
        }
    }

    private void InitQuadGraph()
    {
        graphVertixList = graphVertexSampler.Generate(this.layerNumber);
        graphPolygonList = graphGenerator.GenerateDelaunayGraph(graphVertixList);
        mergedPolygonList = graphModifier.RandomMerge(graphPolygonList, this.mergeProb);
        quadPolygonList = graphModifier.SplitToQuads(mergedPolygonList, graphVertixList);
        relaxPolygonList = graphRelaxation.Relaxation(quadPolygonList, this.learningEpoch, this.learningRate);
    }
}
