using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePlacement : MonoBehaviour
{
    private System.Random rand = new System.Random(System.Guid.NewGuid().GetHashCode());

    public static int prefabNum = 17;
    public int vehicleNum = 20;
    public GameObject[] prefabList = new GameObject[prefabNum];

    public CityGenerator cityGenerator;
    public VehicleItem vehiclePrefab;

    private List<VehicleItem> generatedCache = new List<VehicleItem>();

    // Start is called before the first frame update
    void Start()
    {
        //GenerateVehicleTest();
        GenerateVehicleSimple();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateVehicleSimple()
    {
        int stepX = cityGenerator.cityWidth / cityGenerator.basicBlockSize;
        int stepY = cityGenerator.cityHeight / cityGenerator.basicBlockSize;

        for (int i = 0; i < vehicleNum; i++)
        {

            int indexX = rand.Next(0, stepX);
            int indexY = rand.Next(0, stepY);
            Vector3 startPoint = new Vector3(indexX * cityGenerator.basicBlockSize, 0, indexY * cityGenerator.basicBlockSize);
            VehicleItem clone = Instantiate(vehiclePrefab, startPoint, Quaternion.identity);
            int randomIndex = rand.Next(0, prefabList.Length);
            GameObject meshModel = Instantiate(prefabList[randomIndex].gameObject, startPoint, Quaternion.identity);
            meshModel.transform.parent = clone.transform;
            clone.GetComponent<VehicleItem>().cityGenerator = cityGenerator;
            clone.transform.parent = this.transform;
            clone.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            clone.startPoint = startPoint;
            clone.destination = startPoint;
            generatedCache.Add(clone);
        }
    }

}
