using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCorner
{
    private GridVertex _bottomCenter;
    private GridVertex _bottomLeft;
    private GridVertex _bottomRight;
    private GridVertex _topCenter;
    private GridVertex _topLeft;
    private GridVertex _topRight;

    public GridCorner(GridVertex center, GridVertex left, GridVertex right)
    {
        _bottomCenter = center;
        _bottomLeft = left;
        _bottomRight = right;
        _topCenter = new GridVertex(center.x, center.y, center.z + GlobalConfig.LAYER_HEIGHT);
        _topLeft = new GridVertex(left.x, left.y, left.z + GlobalConfig.LAYER_HEIGHT);
        _topRight = new GridVertex(right.x, right.y, right.z + GlobalConfig.LAYER_HEIGHT);
    }
}
