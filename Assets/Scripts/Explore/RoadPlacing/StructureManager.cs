using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    // Start is called before the first frame update

    public StructurePrefabWeighted[] housePrefabs, specialPrefabs, bigPrefabs;
    public PlacementManager placementManager;

    private float[] houseWeights, specialWeights, bigWeighths;

    private void Start()
    {
        houseWeights = housePrefabs.Select(prefabStats => prefabStats.weight).ToArray();
        specialWeights = specialPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
        bigWeighths = bigPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
    }

    public void PlaceHouse(Vector3Int position)
    {
        if (CheckPositionBeforePlacement(position))
        {
            int randomIndex = GetRandomWeightedIndex(houseWeights);
            placementManager.PlaceObjectOnTheMap(position, housePrefabs[randomIndex].prefab, CellType.Structure);
            AudioPlayer.instance.PlayPlacementSound();  
        }
    }

    public void PlaceSpecial(Vector3Int position)
    {
        if (CheckPositionBeforePlacement(position))
        {
            int randomIndex = GetRandomWeightedIndex(specialWeights);
            placementManager.PlaceObjectOnTheMap(position, specialPrefabs[randomIndex].prefab, CellType.Structure);
            AudioPlayer.instance.PlayPlacementSound();
        }
    }

    internal void placeBig(Vector3Int position)
    {
        int width = 2;
        int height = 2;
        if (CheckBigStructure(position, width, height))
        {
            int randomIndex = GetRandomWeightedIndex(bigWeighths);
            placementManager.PlaceObjectOnTheMap(position, bigPrefabs[randomIndex].prefab, CellType.Structure, width, height);
            AudioPlayer.instance.PlayPlacementSound();
        }
    }

    private bool CheckBigStructure(Vector3Int position, int width, int height)
    {
        bool nearRoad = false;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var newPosition = position + new Vector3Int(i, 0, j);
                if (nearRoad == false)
                {
                    nearRoad = RoadCheck(newPosition);
                }
                if (DefaultCheck(newPosition))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
        }
        return nearRoad;
    }

    private int GetRandomWeightedIndex(float[] weights)
    {
        float sum = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            sum += weights[i];
        }
        float randomValue = UnityEngine.Random.Range(0, sum);
        float tempSum = 0;
        for(int i = 0; i < weights.Length; i++)
        {
            if(randomValue >= tempSum && randomValue < tempSum + weights[i])
            {
                return i;   
            }
            tempSum += weights[i];
        }
        return 0;
    }

    private bool CheckPositionBeforePlacement(Vector3Int position)
    {

        if (!DefaultCheck(position))
            return false;
        if (!RoadCheck(position))
            return false;
        return true;
    }

    private bool DefaultCheck(Vector3Int position)
    {
        if (!placementManager.CheckIfPositionInBound(position))
            return false;
        if (!placementManager.CheckIfPositionIsFree(position))
            return false;
        return true;
    }

    private bool RoadCheck(Vector3Int position)
    {
        return (placementManager.GetNeighboursOfType(position, CellType.Road).Count > 0);
    }

    void RotateStructurePrefabToFaceRoad(GameObject structure, Vector3Int position) 
    {
        List<Vector3Int> roadNeighbour = placementManager.GetNeighboursOfType(position, CellType.Road);
        if (roadNeighbour.Count == 0)
            return;
    }
}


[Serializable]
public struct StructurePrefabWeighted
{
    public GameObject prefab;
    [Range(0, 1)]
    public float weight;
}
