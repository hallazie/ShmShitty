using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VehicleItem : MonoBehaviour
{

    private System.Random rand = new System.Random(System.Guid.NewGuid().GetHashCode());

    public CityGenerator cityGenerator;

    public float speed;
    public Vector3 direction;
    public Vector3 destination;
    public Vector3 startPoint;

    private int prevIndex = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void ResetRandomSpeed()
    {
        speed = (float)rand.Next(2, 5) / 10f;
    }

    void FindNewDestination()
    {
        startPoint = destination;
        int randIndex = rand.Next(1, 5);
        switch (randIndex)
        {
            case 1:
                if(startPoint.x + cityGenerator.basicBlockSize < cityGenerator.cityWidth && prevIndex != 2)
                {
                    destination = new Vector3(startPoint.x + cityGenerator.basicBlockSize, 0, startPoint.z);
                    this.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                break;
            case 2:
                if (startPoint.x - cityGenerator.basicBlockSize >= 0.1f && prevIndex != 1)
                {
                    destination = new Vector3(startPoint.x - cityGenerator.basicBlockSize, 0, startPoint.z);
                    this.transform.rotation = Quaternion.Euler(0, 270, 0);
                }
                break;
            case 3:
                if (startPoint.z + cityGenerator.basicBlockSize < cityGenerator.cityHeight && prevIndex != 4)
                {
                    destination = new Vector3(startPoint.x, 0, startPoint.z + cityGenerator.basicBlockSize);
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                break;
            case 4:
                if (startPoint.z - cityGenerator.basicBlockSize >= 0.1f && prevIndex != 3)
                {
                    destination = new Vector3(startPoint.x, 0, startPoint.z - cityGenerator.basicBlockSize);
                    this.transform.rotation = Quaternion.Euler(0, 180, 0);
                }

                break;
        }
        direction = destination - startPoint;
        prevIndex = randIndex;
        ResetRandomSpeed();
        //Debug.Log("find new dest " + destination + " for start " + startPoint + " with direct " + direction);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, destination) < 0.05f)
        {
            this.transform.position = destination;
            FindNewDestination();
        }
        float delta = Time.deltaTime;
        this.transform.position += direction * delta * speed;
        //Debug.Log("current: " + transform.position + "new dest: " + destination);
    }

}
