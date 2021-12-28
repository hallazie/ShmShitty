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
        int cnt = 0;
        foreach (GraphPolygon polygon in relaxPolygonList)
        {
            for (int i = 0; i < polygon.cornerGraphVertexList.Count; i++)
            {
                int j = (i == polygon.cornerGraphVertexList.Count - 1) ? 0 : i + 1;
                var lineChild = new GameObject();
                lineChild.transform.parent = this.transform;
                lineChild.name = "line" + cnt;
                LineRenderer line = lineChild.AddComponent<LineRenderer>();
                line.material = new Material(Shader.Find("Sprites/Default"));
                line.startColor = new Color32(65, 105, 225, 200);
                line.endColor = new Color32(65, 105, 225, 200);
                line.positionCount = 2;
                line.startWidth = 0.025f;
                line.endWidth = 0.025f;
                line.useWorldSpace = true;
                line.numCapVertices = 10;
                line.SetPosition(0, polygon.cornerGraphVertexList[i].ToVector3());
                line.SetPosition(1, polygon.cornerGraphVertexList[j].ToVector3());
                cnt += 1;
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
