using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadItem
{

    public GridItem[] neighbors = new GridItem[4];  // 0: left, 1: up, 2: right, 3: down
    public int gridX;
    public int gridY;
    public int roadType = 0;         // 0: none, 1: end, 2: corner, 3: straight, 4: intersect, 5: cross
    public int roadDirection;        // 0: left, 1: up, 2: right, 3: down
    // public Mesh mesh;
    // public Bounds bounds;

    private CityGenerator.gridType roadEnum = CityGenerator.gridType.Road;

    public RoadItem(int gridX, int gridY)
    {
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public void GetRoadTypeAndDirection()
    {
        if (neighbors[0].gridType == roadEnum && neighbors[1].gridType != roadEnum && neighbors[2].gridType != roadEnum && neighbors[3].gridType != roadEnum)
        {
            // 0, end, left
            this.roadType = 1;
            this.roadDirection = 0;
        }
        else if (neighbors[0].gridType != roadEnum && neighbors[1].gridType == roadEnum && neighbors[2].gridType != roadEnum && neighbors[3].gridType != roadEnum)
        {
            // 1, end, up
            this.roadType = 1;
            this.roadDirection = 3;
        }
        else if (neighbors[0].gridType != roadEnum && neighbors[1].gridType != roadEnum && neighbors[2].gridType == roadEnum && neighbors[3].gridType != roadEnum)
        {
            // 2, end, right
            this.roadType = 1;
            this.roadDirection = 2;
        }
        else if (neighbors[0].gridType != roadEnum && neighbors[1].gridType != roadEnum && neighbors[2].gridType != roadEnum && neighbors[3].gridType == roadEnum)
        {
            // 3, end, down
            this.roadType = 1;
            this.roadDirection = 1;
        }
        else if (neighbors[0].gridType == roadEnum && neighbors[1].gridType == roadEnum && neighbors[2].gridType != roadEnum && neighbors[3].gridType != roadEnum)
        {
            // 4, corner, left
            this.roadType = 2;
            this.roadDirection = 0;
        }
        else if (neighbors[0].gridType != roadEnum && neighbors[1].gridType == roadEnum && neighbors[2].gridType == roadEnum && neighbors[3].gridType != roadEnum)
        {
            // 5, corner, up
            this.roadType = 2;
            this.roadDirection = 1;
        }
        else if (neighbors[0].gridType != roadEnum && neighbors[1].gridType != roadEnum && neighbors[2].gridType == roadEnum && neighbors[3].gridType == roadEnum)
        {
            // 6, corner, right
            this.roadType = 2;
            this.roadDirection = 2;
        }
        else if (neighbors[0].gridType == roadEnum && neighbors[1].gridType != roadEnum && neighbors[2].gridType != roadEnum && neighbors[3].gridType == roadEnum)
        {
            // 7, corner, down
            this.roadType = 2;
            this.roadDirection = 3;
        }
        else if (neighbors[0].gridType == roadEnum && neighbors[1].gridType != roadEnum && neighbors[2].gridType == roadEnum && neighbors[3].gridType != roadEnum)
        {
            // 8, straight, left
            this.roadType = 3;
            this.roadDirection = 0;
        }
        else if (neighbors[0].gridType != roadEnum && neighbors[1].gridType == roadEnum && neighbors[2].gridType != roadEnum && neighbors[3].gridType == roadEnum)
        {
            // 9, straight, up
            this.roadType = 3;
            this.roadDirection = 1;
        }
        else if (neighbors[0].gridType == roadEnum && neighbors[1].gridType == roadEnum && neighbors[2].gridType == roadEnum && neighbors[3].gridType != roadEnum)
        {
            // 10, intersect, left
            this.roadType = 4;
            this.roadDirection = 2;
        }
        else if (neighbors[0].gridType == roadEnum && neighbors[1].gridType == roadEnum && neighbors[2].gridType != roadEnum && neighbors[3].gridType == roadEnum)
        {
            // 11, intersect, up
            this.roadType = 4;
            this.roadDirection = 3;
        }
        else if (neighbors[0].gridType == roadEnum && neighbors[1].gridType != roadEnum && neighbors[2].gridType == roadEnum && neighbors[3].gridType == roadEnum)
        {
            // 12, intersect, right
            this.roadType = 4;
            this.roadDirection = 0;
        }
        else if (neighbors[0].gridType != roadEnum && neighbors[1].gridType == roadEnum && neighbors[2].gridType == roadEnum && neighbors[3].gridType == roadEnum)
        {
            // 13, intersect, down
            this.roadType = 4;
            this.roadDirection = 1;
        }
        else if (neighbors[0].gridType == roadEnum && neighbors[1].gridType == roadEnum && neighbors[2].gridType == roadEnum && neighbors[3].gridType == roadEnum)
        {
            // 14, intersect, down
            this.roadType = 5;
            this.roadDirection = 0;
        }
    }


}
