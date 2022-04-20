using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPlacement : MonoBehaviour
{

    public CityGenerator cityGenerator;

    public GameObject roadBasic;
    public GameObject roadCross;
    public GameObject roadTurn;
    public GameObject roadIntersec;
    public GameObject roadEnd;

    public List<RoadItem> roadList = new List<RoadItem>();

    // Start is called before the first frame update
    void Start()
    {
        // RandomBuild();
        GenerateRoadList();
        Debug.Log("road size: " + roadList.Count);
        PlaceRoadByGenerator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateRoadList()
    {
        /*
         初始化邻接列表
         */
        for (int i = 0; i < cityGenerator.cityWidth; i++)
        {
            for(int j = 0; j < cityGenerator.cityHeight; j++)
            {
                GridItem gridItem = cityGenerator.grid[i][j];
                if (gridItem.gridType == CityGenerator.gridType.Road)
                {
                    RoadItem roadItem = new RoadItem(gridItem.gridX, gridItem.gridY);
                    if (i == 0)
                    {
                        roadItem.neighbors[0] = new GridItem();
                    }
                    else
                    {
                        roadItem.neighbors[0] = cityGenerator.grid[i - 1][j];
                    }
                    if (j == 0)
                    {
                        roadItem.neighbors[1] = new GridItem();
                    }
                    else
                    {
                        roadItem.neighbors[1] = cityGenerator.grid[i][j - 1];
                    }
                    if (i == cityGenerator.cityWidth - 1)
                    {
                        roadItem.neighbors[2] = new GridItem();
                    }
                    else
                    {
                        roadItem.neighbors[2] = cityGenerator.grid[i + 1][j];
                    }
                    if (j == cityGenerator.cityHeight - 1)
                    {
                        roadItem.neighbors[3] = new GridItem();
                    }
                    else
                    {
                        roadItem.neighbors[3] = cityGenerator.grid[i][j + 1];
                    }
                    roadList.Add(roadItem);
                }
            }
        }
    }

    void PlaceRoadByGenerator()
    {
        GameObject tempObject = roadEnd.gameObject;
        foreach (RoadItem roadItem in roadList)
        {
            roadItem.GetRoadTypeAndDirection();
            if (roadItem.roadType == 1)
            {
                tempObject = roadEnd.gameObject;
            }
            else if (roadItem.roadType == 2)
            {
                tempObject = roadTurn.gameObject;
            }
            else if (roadItem.roadType == 3)
            {
                tempObject = roadBasic.gameObject;
            }
            else if (roadItem.roadType == 4)
            {
                tempObject = roadIntersec.gameObject;
            }
            else if (roadItem.roadType == 5)
            {
                tempObject = roadCross.gameObject;
            }
            GameObject clone = Instantiate(tempObject, new Vector3(roadItem.gridX, 0, roadItem.gridY), Quaternion.Euler(0, 90 * roadItem.roadDirection, 0));
            clone.transform.parent = this.transform;
            Debug.Log("adding road at " + roadItem.gridX + ", " + roadItem.gridY);
        }
    }

    void RandomBuild()
    {
        for (int i = 0; i < 10; i++)
        {
            Transform transform = roadBasic.transform;
            GameObject clone = Instantiate(transform.gameObject, new Vector3(i, 0, 0), Quaternion.identity);
        }
    }



}
