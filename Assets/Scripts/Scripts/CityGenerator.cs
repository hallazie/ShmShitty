using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    public enum gridType { None, Road, Residential, Commercial };
    public int cityWidth = 50;
    public int cityHeight = 50;
    public int basicBlockSize = 5;

    public List<List<GridItem>> grid = new List<List<GridItem>>();
    public List<BlockItem> blockList = new List<BlockItem>();

    private System.Random rand = new System.Random(System.Guid.NewGuid().GetHashCode());

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
        GenerateRoadGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomMergeBlocks()
    {

    }

    public void GenerateRoadGrid()
    {
        int horizontalStep = (int)Mathf.Ceil(cityWidth / basicBlockSize);
        int verticalStep = (int)Mathf.Ceil(cityHeight / basicBlockSize);
        for(int i = 0; i < horizontalStep; i++)
        {
            for(int j = 0; j < verticalStep; j++)
            {

                gridType currentType = rand.Next(0, 5) > 2 ? gridType.Commercial : gridType.Residential;

                grid[i * basicBlockSize + basicBlockSize - 1][j * basicBlockSize + basicBlockSize - 1].gridType = currentType;
                grid[i * basicBlockSize + basicBlockSize - 1][j * basicBlockSize + basicBlockSize - 1].neighborTypeList[3] = gridType.Road;
              
                for (int p = 0; p < basicBlockSize; p++)
                {
                    grid[i * basicBlockSize + p][j * basicBlockSize].gridType = gridType.Road;
                    if (p != 0 && p != basicBlockSize - 1)
                    {
                        grid[i * basicBlockSize + p][j * basicBlockSize + 1].gridType = currentType;
                        grid[i * basicBlockSize + p][j * basicBlockSize + 1].neighborTypeList[0] = gridType.Road;

                        grid[i * basicBlockSize + p][j * basicBlockSize + (basicBlockSize - 1)].gridType = currentType;
                        grid[i * basicBlockSize + p][j * basicBlockSize + (basicBlockSize - 1)].neighborTypeList[2] = gridType.Road;
                    }
                }
                for (int q = 0; q < basicBlockSize; q++)
                {
                    grid[i * basicBlockSize][j * basicBlockSize + q].gridType = gridType.Road;
                    if (q != 0 && q != basicBlockSize - 1)
                    {
                        grid[i * basicBlockSize + 1][j * basicBlockSize + q].gridType = currentType;
                        grid[i * basicBlockSize + 1][j * basicBlockSize + q].neighborTypeList[1] = gridType.Road;

                        grid[i * basicBlockSize + (basicBlockSize - 1)][j * basicBlockSize + q].gridType = currentType;
                        grid[i * basicBlockSize + (basicBlockSize - 1)][j * basicBlockSize + q].neighborTypeList[3] = gridType.Road;

                    }
                }
            }
        }
    }

}
