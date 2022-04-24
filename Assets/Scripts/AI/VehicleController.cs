using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class VehicleController : MonoBehaviour
{

    Rigidbody rigidbody;

    public float power = 10;
    public float torque = 0.1f;
    public float maxSpeed = 3;

    public Vector2 direction;       // y forward, x turn

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void MoveToDirection(Vector2 direction)
    {
        this.direction = direction; 
    }

    private void FixedUpdate()
    {
        if(rigidbody.velocity.magnitude < maxSpeed)
        {
            rigidbody.AddForce(direction.y * transform.forward * power);
        }
        rigidbody.AddTorque(direction.x * Vector3.up * torque * direction.y);
    }

}
