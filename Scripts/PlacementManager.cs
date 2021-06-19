using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{

    public int width, height;
    Grid placementGrid;

    // temporary when drag and placing road
    private Dictionary<Vector3Int, StructureModel> temporaryRoadObjects = new Dictionary<Vector3Int, StructureModel>(); 

    // permanent 
    private Dictionary<Vector3Int, StructureModel> structureDictionary = new Dictionary<Vector3Int, StructureModel>();

    private void Start()
    {
        placementGrid = new Grid(width, height);
    }

    internal bool CheckIfPositionInBound(Vector3Int position)
    {
        return (position.x >= 0 && position.x < width && position.z >= 0 && position.z < height) ;
    }

    internal CellType[] GetNeighbourTypeFor(Vector3Int position)
    {
        return placementGrid.GetAllAdjacentCellTypes(position.x, position.z);
    }

    internal bool CheckIfPositionIsFree(Vector3Int position)
    {
        return CheckPositionIsOfType(position, CellType.Empty) || !structureDictionary.ContainsKey(position); 
    }

    private bool CheckPositionIsOfType(Vector3Int position, CellType type)
    {
        return placementGrid[position.x, position.z] == type;
    }

    internal void PlaceTemporaryStructure(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        placementGrid[position.x, position.z] = type;
        // GameObject newStructure = Instantiate(roadStraight, position, Quaternion.identity); 
        StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
        temporaryRoadObjects.Add(position, structure);
    }

    internal List<Vector3Int> GetNeighboursOfType(Vector3Int temporaryPosition, CellType type)
    {
        var neighbourVertices = placementGrid.GetAdjacentCellsOfType(temporaryPosition.x, temporaryPosition.z, type);
        List<Vector3Int> neighbours = new List<Vector3Int>();
        foreach (var point in neighbourVertices)
        {
            neighbours.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return neighbours;
    }

    private StructureModel CreateANewStructureModel(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        GameObject structure = new GameObject(type.ToString());
        structure.transform.SetParent(transform);
        structure.transform.localPosition = position;
        var structureModel = structure.AddComponent<StructureModel>();
        structureModel.CreateModel(structurePrefab);
        return structureModel;
    }

   public void ModifyStructureModel(Vector3Int position, GameObject newModel, Quaternion rotation)
    {
        if (temporaryRoadObjects.ContainsKey(position))
        {
            // Debug.Log("modifying " + position.x.ToString() + ", " + position.z.ToString() + " by swapping...");
            temporaryRoadObjects[position].SwapModel(newModel, rotation);
        }
        else if (structureDictionary.ContainsKey(position))
        {
            structureDictionary[position].SwapModel(newModel, rotation);
        }
    }

    internal void AddTemporaryStructureToStructureDictionary()
    {
        /*
        * finish building temporary roads, and save to permanent roads
        */
        foreach (var structure in temporaryRoadObjects)
        {
            structureDictionary.Add(structure.Key, structure.Value);
        }
        temporaryRoadObjects.Clear();
    }

    internal void RemoveAllTemporaryStructures()
    {
        foreach (var structure in temporaryRoadObjects.Values)
        {
            var position = Vector3Int.RoundToInt(structure.transform.position);
            placementGrid[position.x, position.z] = CellType.Empty;
            // Debug.Log("destroying road object at position: " + position.x.ToString() + ", " + position.z.ToString());
            Destroy(structure.gameObject);
        }
        temporaryRoadObjects.Clear();
    }

    internal List<Vector3Int> getPathBetween(Vector3Int startPosition, Vector3Int endPosition)
    {
        var resultPath = GridSearch.AStarSearch(placementGrid, new Point(startPosition.x, startPosition.z), new Point(endPosition.x, endPosition.z), false);
        List<Vector3Int> path = new List<Vector3Int>();
        foreach (Point point in resultPath)
        {
            path.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return path;
    }

}
