using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem
{

    public CityGenerator.gridType gridType = CityGenerator.gridType.None;

    // 0: left, 1: up, 2: right, 3: down
    public CityGenerator.gridType[] neighborTypeList = new CityGenerator.gridType[4] { CityGenerator.gridType.None, CityGenerator.gridType.None, CityGenerator.gridType.None, CityGenerator.gridType.None };
    public int gridX;
    public int gridY;

    public GridItem(int gridX, int gridY)
    {
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public GridItem()
    {

    }

}
