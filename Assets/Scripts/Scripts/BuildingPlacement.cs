using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    public CityGenerator cityGenerator;
    public RoadPlacement roadGenerator;

    public static int commercialPrefabNum = 10;
    public GameObject[] commercialPrefabList = new GameObject[commercialPrefabNum];
    public static int residentialPrefabNum = 10;
    public GameObject[] residentialPrefabList = new GameObject[residentialPrefabNum];

    private List<GameObject> commercialBuildingList = new List<GameObject>();
    private List<GameObject> residentialBuildingList = new List<GameObject>();
    private List<Vector3> commercialBoundSizeList = new List<Vector3>();
    private List<Vector3> residentialBoundSizeList = new List<Vector3>();

    private System.Random rand = new System.Random(System.Guid.NewGuid().GetHashCode());

    // Start is called before the first frame update
    void Start()
    {
        InitiateBuildingList();
        PlaceBuildingByGenerator();
    }

    private void PlaceBuildingByGenerator()
    {
        for (int i = 0; i < cityGenerator.cityWidth; i++)
        {
            for (int j = 0; j < cityGenerator.cityHeight; j++)
            {
                GridItem gridItem = cityGenerator.grid[i][j];
                if (gridItem.gridType == CityGenerator.gridType.Commercial || gridItem.gridType == CityGenerator.gridType.Residential)
                {
                    int direct = 0;
                    for (int k = 0; k < 4; k++)
                    {
                        if (gridItem.neighborTypeList[k] == CityGenerator.gridType.Road)
                        {
                            direct = k;
                            break;
                        }
                    }
                    PlaceBuildingAdaptSize(i, j, direct, gridItem.gridType);
                }
            }
        }
    }

    private void PlaceBuildingAdaptSize(int xPos, int yPos, int direction, CityGenerator.gridType type)
    {
        
        GameObject building;
        Vector3 size;
        if (type == CityGenerator.gridType.Commercial)
        {
            int idx = rand.Next(0, commercialBuildingList.Count);
            building = commercialBuildingList[idx];
            size = commercialBoundSizeList[idx];
        }
        else if (type == CityGenerator.gridType.Residential)
        {
            int idx = rand.Next(0, residentialBuildingList.Count);
            building = residentialBuildingList[idx];
            size = residentialBoundSizeList[idx];
        }
        else
        {
            return;
        }
        // float scaleX = size.x > 0 ? Mathf.Ceil(size.x - 0.5f) / size.x : 0;
        // float scaleZ = size.z > 0 ? Mathf.Ceil(size.z - 0.5f) / size.z : 0;
        float scaleX = size.x > 0 ? 1f / size.x : 0;
        float scaleZ = size.z > 0 ? 1f / size.z : 0;
        GameObject clone = Instantiate(building.gameObject, new Vector3(xPos, 0, yPos), Quaternion.Euler(0, 90 * direction, 0));
        clone.transform.localScale = new Vector3(scaleX, 1, scaleZ);
        clone.transform.parent = this.transform;

    }

    private void InitiateBuildingList()
    {
        for(int i = 0; i < commercialPrefabNum; i++)
        {
            GameObject prefab = commercialPrefabList[i];
            if (prefab != null)
            {
                Vector3 size = prefab.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size;
                commercialBuildingList.Add(prefab);
                commercialBoundSizeList.Add(size);
            }
        }
        for (int i = 0; i < residentialPrefabNum; i++)
        {
            GameObject prefab = residentialPrefabList[i];
            if (prefab != null)
            {
                Vector3 size = prefab.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size;
                residentialBuildingList.Add(prefab);
                residentialBoundSizeList.Add(size);
            }
        }
    }
}
