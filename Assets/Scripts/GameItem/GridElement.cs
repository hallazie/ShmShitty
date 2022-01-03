using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour
{

    public List<GridElement> adjecencyGridList;
    public List<CornerElement> cornerList;
    public int floor;
    public Vector3 coord;
    public bool enabled;


    public GridElement(int floor, Vector3 coord)
    {
        this.floor = floor;
        this.coord = coord;
        this.enabled = false;
    }


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
