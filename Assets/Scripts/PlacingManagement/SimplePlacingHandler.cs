using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlacingHandler : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SimplePlaceHouse(Vector3 position)
    {
        Debug.Log("Now Placing house at position: " + position.ToString());
    }
}
