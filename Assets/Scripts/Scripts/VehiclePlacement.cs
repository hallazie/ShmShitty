using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePlacement : MonoBehaviour
{

    public static int prefabNum = 2;
    public GameObject[] prefabList = new GameObject[prefabNum];

    public CityGenerator cityGenerator;

    private GameObject car;
    Vector3 start = new Vector3(0, 0, 0);
    Vector3 end = new Vector3(0, 0, 5);
    bool direct = true;

    // Start is called before the first frame update
    void Start()
    {
        GenerateVehicleSimple();
    }

    // Update is called once per frame
    void Update()
    {
        if (direct)
        {
            car.transform.position += new Vector3(0, 0, Time.deltaTime);
        }
        else
        {
            car.transform.position -= new Vector3(0, 0, Time.deltaTime);
        }
        if (Vector3.Distance(car.transform.position, end) < 0.3)
        {
            direct = false;
            car.transform.rotation = Quaternion.Euler(0, 180, 0);
            car.transform.localPosition += new Vector3(-0.08f, 0, 0);
        }
        if (Vector3.Distance(car.transform.position, start) < 0.3)
        {
            direct = true;
            car.transform.rotation = Quaternion.Euler(0, 0, 0);
            car.transform.localPosition += new Vector3(0.08f, 0, 0);
        }
    }

    void GenerateVehicleSimple()
    {
        // VehicleItem vehicle = new VehicleItem();
        // vehicle.mesh = prefabList[0].GetComponent<MeshFilter>().mesh;
        // vehicle.transform.position = new Vector3(0, 0, 0);
        car = Instantiate(prefabList[0].gameObject, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        car.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        car.transform.parent = this.transform;
    }
}
