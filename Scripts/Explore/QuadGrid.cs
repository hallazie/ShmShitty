using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadGrid : MonoBehaviour
{

    private List<GridPolygon> gridPolygonList = new List<GridPolygon>();
    private List<GridVertex> gridVertixList = new List<GridVertex>();
    private List<GridPolygon> quadPolygonList = new List<GridPolygon>();
    private List<GridPolygon> mergedPolygonList = new List<GridPolygon>();
    private List<GridPolygon> relaxPolygonList = new List<GridPolygon>();
    List<Color> colorList;

    HexagonGridVertexSampler gridVertexSampler;
    GridGenerator gridGenerator;
    GridModifier gridModifier;
    GridRelaxation gridRelaxation;

    public LineRenderer lineRenderer;

    private void Start()
    {

        lineRenderer = gameObject.AddComponent<LineRenderer>();

        gridVertexSampler = new HexagonGridVertexSampler();
        gridGenerator = new GridGenerator();
        gridModifier = new GridModifier(0.66f);
        gridRelaxation = new GridRelaxation(0.1f, false);

        colorList = new List<Color>{
            Color.blue,
            Color.white,
            Color.red,
            Color.green,
            Color.yellow,
            Color.cyan
        };

        lineRenderer.startColor = Color.gray;
        lineRenderer.endColor = Color.gray;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;

        InitQuadGrid();

        DrawGrid();

    }

    private void DrawGrid()
    {
        foreach (GridPolygon polygon in relaxPolygonList)
        {
            for (int i = 0; i < polygon.gridVertexList.Count; i++)
            {
                lineRenderer = GetComponent<LineRenderer>();
                int j = (i != polygon.gridVertexList.Count - 1) ? i + 1 : 0;
                GridVertex v1 = polygon.gridVertexList[i];
                GridVertex v2 = polygon.gridVertexList[j];
                lineRenderer.SetPosition(0, new Vector3(v1.x, 0, v1.y));
                lineRenderer.SetPosition(1, new Vector3(v2.x, 0, v2.y));
            }
        }
    }

    void InitQuadGrid()
    {
        gridVertixList = gridVertexSampler.Generate(10);
        gridPolygonList = gridGenerator.GenerateDelaunayGrid(gridVertixList);
        mergedPolygonList = gridModifier.RandomMerge(gridPolygonList, 0.75f);
        quadPolygonList = gridModifier.SplitToQuads(mergedPolygonList);
        relaxPolygonList = gridRelaxation.Relaxation(quadPolygonList, 100, 0.25f);
    }

    private void OnDrawGizmos()
    {
        int colorIndex = 0;
        foreach (GridPolygon polygon in relaxPolygonList)
        {
            polygon.SortGridVertexClockwise();
            for(int i = 0; i < polygon.gridVertexList.Count; i++)
            {
                int index1 = i;
                int index2 = i == polygon.gridVertexList.Count - 1 ? 0 : i + 1;
                GridVertex v1 = polygon.gridVertexList[index1];
                GridVertex v2 = polygon.gridVertexList[index2];
                // Gizmos.color = colorList[colorIndex % (colorList.Count - 1)];
                Gizmos.DrawLine(v1.ToVector3(), v2.ToVector3());
                colorIndex += 1;
            }
        }
    }

}
