using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadFixer : MonoBehaviour
{

    public GameObject deadEnd, roadStraight, corner, threeWay, fourWay;

    public void FixRoadAtPosition(PlacementManager placementManager, Vector3Int temporaryPosition)
    {
        /* fix road according to its neighbors */

        // right, up, left, down
        var result = placementManager.GetNeighbourTypeFor(temporaryPosition);
        int roadCount = 0;
        roadCount = result.Where(x => x == CellType.Road).Count();
        if (roadCount == 1)
        {
            // Debug.Log("triggered create: dead end");
            CreateDeadEnd(placementManager, result, temporaryPosition);
        }
        else if (roadCount == 2)
        {
            if (CreateStraightRoad(placementManager, result, temporaryPosition))
            {
                // Debug.Log("triggered create: straight road");
                return;
            }
            // Debug.Log("triggered create: corner");
            CreateCorner(placementManager, result, temporaryPosition);
        }
        else if (roadCount == 3)
        {
            // Debug.Log("triggered create: three way");
            CreateThreeWay(placementManager, result, temporaryPosition);
        }
        else
        {
            // Debug.Log("triggered create: four way");
            CreateFourWay(placementManager, result, temporaryPosition);
        }
    }

    private void CreateFourWay(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        placementManager.ModifyStructureModel(temporaryPosition, fourWay, Quaternion.identity);
    }

    private void CreateThreeWay(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        /*
         left, up, right, down
         */
        if(result[1] == CellType.Road && result[2] == CellType.Road && result[3] == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, threeWay, Quaternion.identity);
        }
        else if(result[2] == CellType.Road && result[3] == CellType.Road && result[0] == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, threeWay, Quaternion.Euler(0, 90, 0));
        }
        else if (result[3] == CellType.Road && result[0] == CellType.Road && result[1] == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, threeWay, Quaternion.Euler(0, 180, 0));
        }
        else
        {
            placementManager.ModifyStructureModel(temporaryPosition, threeWay, Quaternion.Euler(0, 270, 0));
        }
    }

    private void CreateCorner(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        /*
         left, up, right, down
         */
        if (result[1] == CellType.Road && result[2] == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, corner, Quaternion.Euler(0, 90, 0));
        }
        else if (result[2] == CellType.Road && result[3] == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, corner, Quaternion.Euler(0, 180, 0));
        }
        else if (result[3] == CellType.Road && result[0] == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, corner, Quaternion.Euler(0, 270, 0));
        }
        else
        {
            placementManager.ModifyStructureModel(temporaryPosition, corner, Quaternion.identity);
        }
    }

    private bool CreateStraightRoad(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        if (result[0] == CellType.Road && result[2] == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, roadStraight, Quaternion.identity);
            return true;
        }
        else if(result[1] == CellType.Road && result[3] == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, roadStraight, Quaternion.Euler(0, 90, 0));
            return true;
        }
        return false;
    }

    private void CreateDeadEnd(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        /*
         left, up, right, down
         */
        if (result[1] == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 270, 0));
        }
        else if (result[2] == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.identity);
        }
        else if (result[3] == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 90, 0));
        }
        else
        {
            placementManager.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 180, 0));
        }
    }
}
