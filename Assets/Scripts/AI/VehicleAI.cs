using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VehicleAI : MonoBehaviour
{
    public List<Vector3> pathList;
    public float arriveDistanceTurn = 0.3f;
    public float arriveDistanceForward = 0.1f;
    public float rotationOffset = 5;

    private Vector3 currentDestination;
    private int currentIndex;

    public bool stop;

    [field: SerializeField]
    public UnityEvent<Vector2> OnDrive
    {
        get;
        set;
    }


}
