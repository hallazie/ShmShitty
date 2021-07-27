using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonUtils : MonoBehaviour
{

    public static float GridVertexEuclideanDistance(GridVertex vertex1, GridVertex vertex2)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(vertex1.x - vertex2.x, 2) + Mathf.Pow(vertex1.y - vertex2.y, 2));
        return distance;
    }

}
