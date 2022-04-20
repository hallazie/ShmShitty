using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem
{

    public CityGenerator.gridType gridType = CityGenerator.gridType.None;
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
