using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public PlacementManager placementManager;

    public List<Vector3Int> temporaryPlacementPositions = new List<Vector3Int>();
    public List<Vector3Int> roadPositionsToRecheck = new List<Vector3Int>();

    public RoadFixer roadFixer;

    private Vector3Int startPosition, endPosition, previousPosition;
    private bool placementMode = false; // if hold and drag, use a*, else use single spot placement

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
        if (placementMode == false)
        {
            // just start dragging

            temporaryPlacementPositions.Clear();
            roadPositionsToRecheck.Clear();

            placementMode = true;
            startPosition = position;
            previousPosition = position;

            temporaryPlacementPositions.Add(position);
            placementManager.PlaceTemporaryStructure(position, roadFixer.roadStraight, CellType.Road);
        }
        else
        {
            // already got a start point, dragging to get more road

            placementManager.RemoveAllTemporaryStructures();
            temporaryPlacementPositions.Clear();

            foreach (var positionToFix in roadPositionsToRecheck)
            {
                roadFixer.FixRoadAtPosition(placementManager, positionToFix);
            }
            roadPositionsToRecheck.Clear();

            temporaryPlacementPositions = placementManager.getPathBetween(startPosition, position);
            Debug.Log("current temporary positions: " + String.Join(", ", temporaryPlacementPositions));

            foreach (var temporaryPosition in temporaryPlacementPositions)
            {
                if (placementManager.CheckIfPositionIsFree(temporaryPosition) == false)
                {
                    roadPositionsToRecheck.Add(temporaryPosition);
                    continue;
                }
                placementManager.PlaceTemporaryStructure(temporaryPosition, roadFixer.roadStraight, CellType.Road);
            }
        }
        FixRoadPrefab();
    }

    private void FixRoadPrefab()
    {
        // roadPositionsToRecheck.Clear();
        foreach(var temporaryPosition in temporaryPlacementPositions)
        {
            roadFixer.FixRoadAtPosition(placementManager, temporaryPosition);
            var neighbours = placementManager.GetNeighboursOfType(temporaryPosition, CellType.Road);
            foreach (var roadPosition in neighbours)
            {
                if (!roadPositionsToRecheck.Contains(roadPosition))
                {
                    roadPositionsToRecheck.Add(roadPosition);
                }
            }
        }
        foreach (var position in roadPositionsToRecheck)
        {
            roadFixer.FixRoadAtPosition(placementManager, position);
        }
    }

    public void FinishPlacingRoad()
    {
        placementMode = false;
        placementManager.AddTemporaryStructureToStructureDictionary();
        if(temporaryPlacementPositions.Count > 0)
        {
            AudioPlayer.instance.PlayPlacementSound();
        }
        temporaryPlacementPositions.Clear();
        startPosition = Vector3Int.zero;
    }
}
