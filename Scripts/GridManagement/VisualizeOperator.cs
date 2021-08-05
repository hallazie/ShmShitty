using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeOperator : MonoBehaviour
{

    public QuadGrid quadGrid;

    private GridPolygon currentHoverOverPolygon = null;

    public void Start()
    {
        // quadGrid = GetComponent<QuadGrid>();
    }

    public void HighlightHoverQuad(Vector3 position)
    {
        GridVertex positionVertex = new GridVertex(position.x, position.z);
        float closestDistance = 99999;
        GridPolygon closestPolygon = null;
        foreach (GridPolygon polygon in quadGrid.polygonList)
        {
            float currentDistance = CommonUtils.GridVertexEuclideanDistance(polygon.center, positionVertex);
            if (currentDistance <= closestDistance)
            {
                closestDistance = currentDistance;
                closestPolygon = polygon;
            }
        }
        if (closestPolygon == null)
            return;
        if (currentHoverOverPolygon != closestPolygon)
        {
            Debug.Log("current hover over polygon change from " + currentHoverOverPolygon.ToString() + " ---> " + closestPolygon.ToString());
            currentHoverOverPolygon = closestPolygon;
        }
        
    }

}
