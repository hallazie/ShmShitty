using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerElement : MonoBehaviour
{

    public List<GridElement> ownerGridList;
    public Vector3 coord;
    public int floor;

    public Vector3 prevCoord = Vector3.zero;
    public Vector3 nextCoord = Vector3.zero;

    public void Instantiate(int floor, Vector3 coord)
    {
        this.floor = floor;
        this.coord = coord;
        this.enabled = false;

        this.name = "GridElem_" + coord.x + "_" + coord.y + "_" + coord.z;
        this.transform.position = coord;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
