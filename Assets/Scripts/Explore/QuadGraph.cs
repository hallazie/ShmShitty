using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadGraph : MonoBehaviour
{
    System.Random random;

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
    public Line[] renderLineList;
    public Line lineRenderer;

    public List<GraphPolygon> polygonList
    {
        get
        {
            return relaxPolygonList;
        }
    }

    public List<GraphVertex> vertexList
    {
        get
        {
            return graphVertixList;
        }
    }

  
    List<Color> colorList;

    HexagonGraphVertexSampler graphVertexSampler;
    GraphGenearator graphGenerator;
    GraphModifier graphModifier;
    GraphRelaxation graphRelaxation;

    public Material lineMat;

    private void Start()
    {

        random = new System.Random();
        graphVertexSampler = new HexagonGraphVertexSampler();
        graphGenerator = new GraphGenearator();
        graphModifier = new GraphModifier();
        graphRelaxation = new GraphRelaxation(learningRate, fixAlpha);

        colorList = new List<Color>{
            Color.blue,
            Color.white,
            Color.red,
            Color.green,
            Color.yellow,
            Color.cyan
        };

        InitQuadGraph();
        DrawGraphByRenderer();
    }

    private void DrawGraphByRenderer()
    {

        renderLineList = new Line[relaxPolygonList.Count];

        foreach (GraphPolygon polygon in relaxPolygonList)
        {
            for (int i = 0; i < polygon.cornerGraphVertexList.Count; i++)
            {
                int j = (i == polygon.cornerGraphVertexList.Count - 1) ? 0 : i + 1;
                // LineRenderer line = new GameObject("line" + i).AddComponent<LineRenderer>();
                Line line = Instantiate(lineRenderer, Vector3.zero, Quaternion.identity, this.transform);
                line.name = "line" + i;
                line.renderer.material = new Material(Shader.Find("Sprites/Default"));
                line.renderer.startColor = new Color32(65, 105, 225, 200);
                line.renderer.endColor = new Color32(65, 105, 225, 200);
                line.renderer.positionCount = 2;
                line.renderer.startWidth = 0.025f;
                line.renderer.endWidth = 0.025f;
                line.renderer.useWorldSpace = true;
                line.renderer.numCapVertices = 10;
                line.renderer.SetPosition(0, polygon.cornerGraphVertexList[i].ToVector3());
                line.renderer.SetPosition(1, polygon.cornerGraphVertexList[j].ToVector3());

                renderLineList[i] = line;
            }
        }
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

    private void InitQuadGraph()
    {
        graphVertixList = graphVertexSampler.Generate(this.layerNumber);
        graphPolygonList = graphGenerator.GenerateDelaunayGraph(graphVertixList);
        mergedPolygonList = graphModifier.RandomMerge(graphPolygonList, this.mergeProb);
        quadPolygonList = graphModifier.SplitToQuads(mergedPolygonList, graphVertixList);
        relaxPolygonList = graphRelaxation.Relaxation(quadPolygonList, this.learningEpoch, this.learningRate);
        AddAdjecentPolygonToVertex();
    }

}
