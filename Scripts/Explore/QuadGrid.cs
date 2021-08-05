using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadGrid : MonoBehaviour
{
    System.Random random;

    private List<GridPolygon> gridPolygonList = new List<GridPolygon>();
    private List<GridVertex> gridVertixList = new List<GridVertex>();
    private List<GridPolygon> quadPolygonList = new List<GridPolygon>();
    private List<GridPolygon> mergedPolygonList = new List<GridPolygon>();
    private List<GridPolygon> relaxPolygonList = new List<GridPolygon>();

    private int layerNumber;
    private float mergeProb;
    private int learningEpoch;
    private float learningRate;
    private bool fixAlpha;

    public List<GridPolygon> polygonList
    {
        get
        {
            return relaxPolygonList;
        }
    }

    public QuadGrid(int layerNumber, float mergeProb, int learningEpoch, float learningRate, bool fixAlpha)
    {
        this.layerNumber = layerNumber;
        this.mergeProb = mergeProb;
        this.learningEpoch = learningEpoch;
        this.learningRate = learningRate;
        this.fixAlpha = fixAlpha;
    }

    private LineRenderer line;
    List<Color> colorList;

    HexagonGridVertexSampler gridVertexSampler;
    GridGenerator gridGenerator;
    GridModifier gridModifier;
    GridRelaxation gridRelaxation;

    public Material lineMat;

    private void Start()
    {

        random = new System.Random();
        gridVertexSampler = new HexagonGridVertexSampler();
        gridGenerator = new GridGenerator();
        gridModifier = new GridModifier();
        gridRelaxation = new GridRelaxation(learningRate, fixAlpha);

        colorList = new List<Color>{
            Color.blue,
            Color.white,
            Color.red,
            Color.green,
            Color.yellow,
            Color.cyan
        };

        InitQuadGrid();
        DrawGridByRenderer();
    }

    private void DrawGridByRenderer()
    {
        int lineCount = 0;
        foreach (GridPolygon polygon in relaxPolygonList)
        {
            for (int i = 0; i < polygon.gridVertexList.Count; i++)
            {
                int j = (i == polygon.gridVertexList.Count - 1) ? 0 : i + 1;
                line = new GameObject("line" + lineCount).AddComponent<LineRenderer>();
                line.material = new Material(Shader.Find("Sprites/Default"));
                line.startColor = new Color32(65, 105, 225, 200);
                line.endColor = new Color32(65, 105, 225, 200);
                line.positionCount = 2;
                line.startWidth = 0.025f;
                line.endWidth = 0.025f;
                line.useWorldSpace = true;
                line.numCapVertices = 10;
                line.SetPosition(0, polygon.gridVertexList[i].ToVector3());
                line.SetPosition(1, polygon.gridVertexList[j].ToVector3());
                line = null;
                lineCount++;
            }
        }
    }

    void InitQuadGrid()
    {
        gridVertixList = gridVertexSampler.Generate(this.layerNumber);
        gridPolygonList = gridGenerator.GenerateDelaunayGrid(gridVertixList);
        mergedPolygonList = gridModifier.RandomMerge(gridPolygonList, this.mergeProb);
        quadPolygonList = gridModifier.SplitToQuads(mergedPolygonList);
        relaxPolygonList = gridRelaxation.Relaxation(quadPolygonList, this.learningEpoch, this.learningRate);
    }

}
