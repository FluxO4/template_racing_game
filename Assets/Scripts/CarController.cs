using System;
using UnityEngine;

public class CarController : MonoBehaviour
{//// old nitro stuff
    //public int nitroCharges = 3;
    //public int currentNitroCharges;     // public for debugging
    //public float nitroCoolDown;
    //public float currentNitroCoolDown;  // public for debugging
    //public float nitroDuration;
    //public float currentNitroDuration;  // public for debugging
    //public float nitroFactor;

    public float maxSpeed = 100;
    public float motorTorque = 2000f;
    public float brakeForce = 2000f;
    public float steeringRange = 35;
    public float steeringRangeAtMaxSpeed = 10;

    [Space(5)]
    public float centreOfGravityOffset = -1f;
    public bool isOnGround = false; // public for debugging
    public float airResistance = 0.05f;


    [Space(5)]
    public float nitroCooldown = 10f;
    public float nitroFactor = 10f;
    public float downForce = 100f;

    [Space(5)]
    [Header("Speed Display")]
    public int SpeedInKPH; // integer vals

    private float timeTillNitroCanBeUsed;

    [HideInInspector] public float currentSpeed;

    WheelFrictionCurve forwardFriction, sidewaysFriction;

    public AnimationCurve steeringCurve;

    public bool IsOnGround
    {
        get { return isOnGround; }

        set { isOnGround = value; }
    }

    WheelControl[] wheels;
    Rigidbody rb;

    [HideInInspector]
    public bool playerCar = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Adjust center of mass vertically, to help prevent the car from rolling
        rb.centerOfMass += Vector3.up * centreOfGravityOffset;

        // Find all child GameObjects that have the WheelControl script attached
        wheels = GetComponentsInChildren<WheelControl>();

        forwardFriction = wheels[0].WheelCollider.forwardFriction;
        sidewaysFriction = wheels[0].WheelCollider.sidewaysFriction;

    }

    public void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (!playerCar)
        {
            verticalInput = 0;
            horizontalInput = 0;
           // brake = Input.GetKey(KeyCode.Space);
        }


        //Applying Maximum Speed
        if (SpeedInKPH < maxSpeed)
        {
            foreach (WheelControl wheel in wheels)
            {
                if (wheel.motorized)
                    wheel.WheelCollider.motorTorque = verticalInput * motorTorque;
                wheel.WheelCollider.brakeTorque = 0;
            }
        }

        //Debug.Log(wheels[0].WheelCollider.rpm);

        if (SpeedInKPH > maxSpeed)
        {
            foreach (WheelControl wheel in wheels)
            {
                if (wheel.motorized)
                    wheel.WheelCollider.motorTorque = 0;
            }
        }

        currentSpeed = rb.velocity.magnitude;
        SpeedInKPH = (int)(currentSpeed * 3.6f);

        float currentSteerRange = horizontalInput * steeringCurve.Evaluate(currentSpeed);

        foreach (WheelControl wheel in wheels)
        {
            if (wheel.steerable)
                wheel.WheelCollider.steerAngle = currentSteerRange;
        }

        if (playerCar)
        {

            if (Input.GetKeyDown(KeyCode.C) && timeTillNitroCanBeUsed < Time.time)
            {
                rb.AddForce(nitroFactor * transform.forward, ForceMode.VelocityChange);
                timeTillNitroCanBeUsed = Time.time + nitroCooldown;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                foreach (WheelControl wheel in wheels)
                {
                    wheel.WheelCollider.brakeTorque = brakeForce;
                    // wheel.WheelCollider.motorTorque = 0;
                }
            }
        }


        Vector3 forces = -rb.velocity.sqrMagnitude * (rb.velocity.normalized) * airResistance;
        // Vector3 forces = -forwardSpeed * forwardSpeed * (rb.velocity.normalized) * airResistance;

        if (playerCar && !IsOnGround) // ideally this will always be false when in here
        {
            if (Input.GetKey(KeyCode.X)) // rotate about X axis (Backflip)
            {
                rb.AddTorque(- transform.right * 25, ForceMode.Acceleration);
            }
            else if (Input.GetKey(KeyCode.Z)) // rotate about Z axis (Barrel Roll)
            {
                rb.AddTorque(transform.forward * 25, ForceMode.Acceleration);
            }
        }

        rb.AddForce(forces, ForceMode.Acceleration);

        if (downForce > 0)
            rb.AddForce(downForce * rb.velocity.magnitude * -transform.up);
    }


}
