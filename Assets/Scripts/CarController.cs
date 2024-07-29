using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public float brakeTorque = 2000f;
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

    float vInput = 0;
    float hInput = 0;

    private void OnSteer(InputValue value)
    {
        Vector2 vectorValue = value.Get<Vector2>();
        Debug.Log("Detected vector: " + vectorValue);
        vInput = vectorValue.y;
        hInput = vectorValue.x;
    }
     private void OnSteerTilt(InputValue value)
     {
          Vector3 vectorValue = value.Get<Vector3>();
          Debug.Log("Detected vector: " + vectorValue);
          vInput = vectorValue.y;
          hInput = vectorValue.x;
     }

     public void Update()
    {
        //Vector2 joystickInput = Gamepad.current.leftStick.ReadValue();

        /*float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");*/

        //Debug.Log("Current vertical and horizontal are v="+vInput +", h="+hInput);

        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        float forwardSpeed = Vector3.Dot(transform.forward, rb.velocity);


        // Calculate how close the car is to top speed
        // as a number from zero to one
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        // Use that to calculate how much torque is available 
        // (zero torque at top speed)
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        currentSpeed = rb.velocity.magnitude;

        // …and to calculate how much to steer 
        // (the car steers more gently at top speed)
        //float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);
        float currentSteerRange = Mathf.Abs(steeringCurve.Evaluate(currentSpeed));

        // Check whether the user input is in the same direction 
        // as the car's velocity
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in wheels)
        {
            // Apply steering to Wheel colliders that have "Steerable" enabled
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }

            if (isAccelerating)
            {
                // Apply torque to Wheel colliders that have "Motorized" enabled
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = vInput * motorTorque;
                }
                wheel.WheelCollider.brakeTorque = 0;
            }
            else
            {
                // If the user is trying to go in the opposite direction
                // apply brakes to all wheels
                wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }

        //Also accelerate the body of the car
        /*if (isAccelerating)
        {
            rb.AddForce(transform.forward * vInput * maxSpeed * Time.deltaTime + transform.right * hInput * currentSteerRange * 0.05f * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
        
        }*/

            
        SpeedInKPH = (int)(currentSpeed * 3.6f);

        /*Vector3 forces = -rb.velocity.sqrMagnitude * (rb.velocity.normalized) * airResistance;
        // Vector3 forces = -forwardSpeed * forwardSpeed * (rb.velocity.normalized) * airResistance;

        rb.AddForce(forces, ForceMode.Acceleration);*/

        if (downForce > 0)
            rb.AddForce(downForce * rb.velocity.magnitude * -transform.up); 



        /*
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

        //float currentSteerRange = horizontalInput * steeringCurve.Evaluate(currentSpeed);

        foreach (WheelControl wheel in wheels)
        {
            if (wheel.steerable)
                wheel.WheelCollider.steerAngle = steeringRange;


        }


        /*if (playerCar)
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
            rb.AddForce(downForce * rb.velocity.magnitude * -transform.up);*/
    }


}
