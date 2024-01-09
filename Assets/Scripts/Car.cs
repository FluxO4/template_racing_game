using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public WheelCollider[] backWheels = new WheelCollider[2];
    public WheelCollider[] frontWheels = new WheelCollider[2];

    public float maxAcceleration = 500f;
    public float breakingForce = 300f;
    public float maxTurnAngle = 15f;

    float currentAcceleration = 0f;
    float currentBreakForce = 0f;
    float currentTurnAngle = 0f;

    void Start()
    {
    }

    private void FixedUpdate()
    {

        currentAcceleration = Input.GetAxis("Vertical") * maxAcceleration;

        currentTurnAngle = Input.GetAxis("Horizontal") * maxTurnAngle;

        if (Input.GetKey(KeyCode.Space))
            currentBreakForce = breakingForce;
        else
            currentBreakForce = 0;

        foreach (var wheel in backWheels)
        {
            wheel.motorTorque = currentAcceleration;
        }

        foreach (var wheel in frontWheels)
        {
            // wheel.motorTorque = currentAcceleration;
            wheel.steerAngle = currentTurnAngle;
        }

    }


}
