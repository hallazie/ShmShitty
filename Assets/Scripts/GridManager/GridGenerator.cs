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

    void Start()
    {
        InitQuadGraph();
    }

    void Update()
    {
        
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
