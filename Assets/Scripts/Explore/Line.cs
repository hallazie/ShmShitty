using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{

    public LineRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = new GameObject("").AddComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
