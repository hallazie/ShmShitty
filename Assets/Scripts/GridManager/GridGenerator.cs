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

    void Start()
    {
        graphVertexSampler = new HexagonGraphVertexSampler();
        graphGenerator = new GraphGenearator();
        graphModifier = new GraphModifier();
        graphRelaxation = new GraphRelaxation(learningRate, fixAlpha);

        InitQuadGraph();
        InitQuadGrid();
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
                    cornerElementList.Add(cornerElement);
                }
                gridElementList.Add(gridElement);
            }
        }
        Debug.Log("grid element list init finished with size: " + gridElementList.Count + " and corner element list init finished with size: " + cornerElementList.Count);
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
}
