using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public PlacementManager placementManager;

    public List<Vector3Int> temporaryPlacementPositions = new List<Vector3Int>();

    public GameObject roadStraight;

    public RoadFixer roadFixer;

    public void Start()
    {
        roadFixer = GetComponent<RoadFixer>();
    }

    public void PlaceRoad(Vector3Int position)
    {
        if (placementManager.CheckIfPositionInBound(position) == false)
            return;
        if (placementManager.CheckIfPositionIsFree(position) == false)
            return;
        temporaryPlacementPositions.Clear();
        temporaryPlacementPositions.Add(position);
        placementManager.PlaceTemporaryStructure(position, roadStraight, CellType.Road);
        FixRoadPrefab();
    }

    private void FixRoadPrefab()
    {
        foreach(var temporaryPosition in temporaryPlacementPositions)
        {
            roadFixer.FixRoadAtPosition(placementManager, temporaryPosition); 
        }
    }
}
