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
        //MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        //meshFilter.transform.parent = gameObject.transform;
        //meshFilter.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //GameObject sphereMesh = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //meshFilter.mesh = sphereMesh.GetComponent<MeshFilter>().mesh;
        //meshFilter.gameObject.AddComponent<MeshRenderer>();
        //Destroy(sphereMesh);
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
