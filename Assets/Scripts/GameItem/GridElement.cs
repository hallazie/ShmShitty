using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour
{

    private List<GridElement> adjecencyGridList;
    private List<CornerElement> cornerList;
    private int floor;
    private Vector3 coord;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GridElement> getAdjecencyGridList()
    {
        return adjecencyGridList;
    }

    public bool addAdjecencyElement(GridElement adjecentElement)
    {
        // if success, return true
        return true;
    }

    public bool removeAdjecencyElement(GridElement adjecentElement)
    {
        // if success, return true
        return true;
    }

}
