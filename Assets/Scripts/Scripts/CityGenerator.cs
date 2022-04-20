using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    public enum gridType { None, Road, Residential, Commercial };
    public int cityWidth = 50;
    public int cityHeight = 50;

    public List<List<GridItem>> grid = new List<List<GridItem>>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < cityWidth; i++)
        {
            List<GridItem> subGrid = new List<GridItem>();
            for (int j = 0; j < cityHeight; j++)
            {
                GridItem gridItem = new GridItem(i, j);
                subGrid.Add(gridItem);
            }
            grid.Add(subGrid);
        }
        GenerateAllSimple();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateRoadGrid()
    {
        
    }

    public void GenerateAllSimple()
    {
        for (int i = 2; i < cityWidth-3; i++)
        {
            grid[i][12].gridType = gridType.Commercial;
            grid[i][13].gridType = gridType.Road;
            grid[i][14].gridType = gridType.Commercial;
        }

        for (int j = 5; j < cityHeight; j++)
        {
            if (grid[28][j].gridType != gridType.Road)
            {
                grid[28][j].gridType = gridType.Residential;
            }
            if (grid[29][j].gridType != gridType.Road)
            {
                grid[29][j].gridType = gridType.Road;
            }
            if (grid[30][j].gridType != gridType.Road)
            {
                grid[30][j].gridType = gridType.Residential;
            }
        }

        for (int j = 13; j < cityHeight - 6; j++)
        {
            if (grid[35][j].gridType != gridType.Road)
            {
                grid[35][j].gridType = gridType.Residential;
            }
            if (grid[36][j].gridType != gridType.Road)
            {
                grid[36][j].gridType = gridType.Road;
            }
            if (grid[37][j].gridType != gridType.Road)
            {
                grid[37][j].gridType = gridType.Residential;
            }
        }
    }

}
