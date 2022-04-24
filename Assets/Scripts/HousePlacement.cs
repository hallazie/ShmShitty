using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousePlacement : MonoBehaviour
{
    public CityGenerator cityGenerator;

    public GameObject urbanHouse1;
    public GameObject urbanHouse2;
    public GameObject urbanHouse3;
    public GameObject urbanShop1;
    public GameObject urbanShop2;

    public List<GameObject> houseList = new List<GameObject>();
    public List<Vector3> boundSizeList = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        InitiateHouseList();
        PlaceHouseAdaptSizeTest(20, 0, 14);
        PlaceHouseAdaptSizeTest(20, 2, 12);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlaceHouseAdaptSizeTest(int houseNumber, int direction, int yPos)
    {
        System.Random rand = new System.Random();
        float offset = 0;
        float prev = 0;
        for (int i = 0; i < houseNumber; i++)
        {
            int idx = rand.Next(0, houseList.Count);
            var house = houseList[idx];
            Vector3 size = boundSizeList[idx];
            float scaleX = size.x > 0 ? Mathf.Ceil(size.x-0.5f) / size.x : 0;
            float scaleZ = size.z > 0 ? Mathf.Ceil(size.z-0.5f) / size.z : 0;
            offset += Mathf.Ceil(size.x - 0.5f) / 2 + prev;
            prev = Mathf.Ceil(size.x - 0.5f) / 2;
            GameObject clone = Instantiate(house.gameObject, new Vector3(offset, 0, yPos), Quaternion.Euler(0, 90 * direction, 0));
            clone.transform.localScale = new Vector3(scaleX, 1, scaleZ);

        }
    }

    private void InitiateHouseList()
    {
        Vector3 size1 = urbanHouse1.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size;
        houseList.Add(urbanHouse1);
        boundSizeList.Add(size1);

        Vector3 size2 = urbanHouse2.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size;
        houseList.Add(urbanHouse2);
        boundSizeList.Add(size2);

        Vector3 size3 = urbanHouse3.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size;
        houseList.Add(urbanHouse3);
        boundSizeList.Add(size3);

        Vector3 size4 = urbanShop1.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size;
        houseList.Add(urbanShop1);
        boundSizeList.Add(size4);

        Vector3 size5 = urbanShop2.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size;
        houseList.Add(urbanShop2);
        boundSizeList.Add(size5);
    }
}
