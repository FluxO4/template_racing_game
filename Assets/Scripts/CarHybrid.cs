using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

public class CarHybrid : MonoBehaviour, JoystickControl.ICarControlActions
{
    public float acceleration = 10f;
    Vector3 velocity = Vector3.zero;

    public float forwardsFriction = 1f;
    public float sidewaysFriction = 10f;

    public float driftingForwardsFriction = 0.02f;
    public float driftingSidewaysFriction = 1f;

    public float airResistance = 0.9f;
    public float gravityValue = 9.8f;

    public float angularVelocity = 5f;
    public float correctionFactor = 0.5f;
    public float maxTurningSpeed = 200;

    [Space(10)]
    // nitro stuff
    public int nitroCharges = 3;
    public float nitroCoolDown;
    public float nitroDuration;
    public float nitroFactor;
    public ParticleSystem nitroParticles;
    public ParticleSystem cameraNitroParticles;

    public bool stunting;   // public for debugging

    public float breakForce = 0.05f;

    public bool isOnGround = false; // public for debugging

    public bool IsOnGround
    {
        get { return isOnGround; }

        set { isOnGround = value; }

    }

    [Space(10)]

    [SerializeField]
    float _speed;
    public Text speedText;

    public float speed
    {
        get { return _speed; }
        set
        {
            _speed = value;
            if (speedText != null)
            {
                speedText.text = (value * 3.6f).ToString("F0") + " km/h";
            }
        }
    }


    [SerializeField]
    float xTarget;
    [SerializeField]
    float zTarget;

    Rigidbody rb;

    [Space(10)]




    public GameObject carVisual;

    [SerializeField]
    bool _drifting;



    WheelFrictionCurve _baseForwardFriction;
    WheelFrictionCurve _baseSidewaysFriction;


    bool drifting
    {
        get
        {
            return _drifting;
        }

        set
        {
            if (value)
            {
                foreach (WheelCollider wc in wheelColliderss)
                {
                    WheelFrictionCurve temp = _baseSidewaysFriction;

                    temp.stiffness = 0.5f;


                    wc.sidewaysFriction = temp;
                }


                DriftCloud.Play();
            }
            else
            {
                foreach (WheelCollider wc in wheelColliderss)
                {
                    wc.sidewaysFriction = _baseSidewaysFriction;
                }

                DriftCloud.Stop();
            }



            _drifting = value;


        }
    }

    bool braking = false;

    public float turnBiasBias = 0.2f;
    public float turnBiasFactor = 1f;
    public float driftTurnThreshold = 0.5f;
    public float driftSpeed = 100f;
    float turnBias = 0;

    public ParticleSystem DriftCloud;


    public float wheelRadius = 0.5f;

    float currentSidewaysFriction;
    float currentForwardsFriction;

    public float wheelColliderSteerTime = 1;

    public float steeringA = 2.5f;
    public float steeringB = 0.05f;
    public float steeringC = 0.2f;



    public Vector3 currentNormal = Vector3.up;

    //Pitch and roll correction
    public float maxPitchAngle = 20f; // Maximum pitch angle (in degrees)
    public float maxRollAngle = 20f;  // Maximum roll angle (in degrees)
    public float pitchSpeed = 100f;   // Torque applied for pitch correction
    public float rollSpeed = 100f;    // Torque applied for roll correction
    public float correctionLiftAmount = 10f;

    // Calculate pitch and roll correction based on current angles
    [ReadOnly(true)]
    public float pitchCorrection = 0;
    [ReadOnly(true)]
    public float rollCorrection = 0;

    public WheelCollider[] wheelColliderss = new WheelCollider[4];

    public Transform[] wheelMeshes = new Transform[4];

    float vInput = 0;
    public float hInput = 0;
    public Text vInputText;
    public Text hInputText;

    float _vertical = 0;
    float _horizontal = 0;


    public float vertical
    {
        get { return _vertical; }
        set
        {
            _vertical = value;
            if (vInputText != null)
            {
                vInputText.text = value.ToString("F3");
            }
        }
    }
    public float horizontal
    {
        get { return _horizontal; }
        set
        {
            _horizontal = value;
            if (hInputText != null)
            {
                hInputText.text = value.ToString("F3");
            }
        }
    }

    public float tiltInputMultiplier = 2;


    public bool accelerating = false;
    public bool autoAccelerating = false;
    public bool backing = false;

    public bool nitro = false;

    private JoystickControl controls;

    public bool stuckTriggered;


    private void Awake()
    {
        controls = new JoystickControl();
    }

    private void OnEnable()
    {
        controls.CarControl.SetCallbacks(this);
        controls.CarControl.Enable();
    }

    private void OnDisable()
    {
        controls.CarControl.Disable();
    }
    public void OnSteerTilt(InputAction.CallbackContext context)
    {
        Vector3 vectorValue = context.ReadValue<Vector3>();
        Debug.Log("Detected vector: " + vectorValue);
        vInput = vectorValue.y;
        hInput = vectorValue.x;
    }
    // private void OnSteer(InputValue value)
    //{
    //    Vector2 vectorValue = value.Get<Vector2>();
    //    //Debug.Log("Detected vector: " + vectorValue);
    //    vInput = vectorValue.y;
    //    hInput = vectorValue.x;
    //}

    // private void OnAccelerate(InputValue value)
    //{
    //    if (value.isPressed)
    //    {
    //        accelerating = true;
    //    }
    //    else
    //    {
    //        accelerating = false;
    //    }
    //}

    // private void OnBack(InputValue value)
    //{
    //    if (value.isPressed)
    //    {
    //        backing = true;
    //    }
    //    else
    //    {
    //        backing = false;
    //    }
    //}

    public void OnSteer(InputAction.CallbackContext context)
    {
        Vector2 vectorValue = context.ReadValue<Vector2>();
        vInput = vectorValue.y;
        hInput = vectorValue.x;
        //Debug.Log("ON STEER TRIGGERED: " + vectorValue);
    }

    public void OnAccelerate(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame() && !accelerating)
        {
            accelerating = true;
        }
        else if (context.action.WasReleasedThisFrame() && accelerating)
        {
            accelerating = false;
        }
    }

    public void OnBack(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame() && !backing)
        {
            backing = true;
        }
        else if (context.action.WasReleasedThisFrame() && backing)
        {
            backing = false;
        }
    }

    public void OnNitro(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame())
        {
            ActivateNitro();
        }

    }
    float SteeringValue(float x) => steeringA * (steeringC * x) / (1 + steeringB * (steeringC * x) * (steeringC * x));
    public void AutoAccelerationOn()
    {
        autoAccelerating = true;
    }
    public void AutoAccelerationOff()
    {
        autoAccelerating = false;
    }

    Coroutine nitroTimerHolder;
    public void ActivateNitro()
    {


        if (nitroTimerHolder == null)
        {
            //if (currentNitroCharges > 0) //DEBUG INFINITE NITRO CHARGES
            {
                nitroCharges--;
                nitroTimerHolder = StartCoroutine(nitroTimer());
            }
           /* else
            {
                Debug.Log("Not enough nitro charges");
            }*/
        }
        else
        {
            Debug.Log("Nitro already in progress");
        }

    }

    public IEnumerator nitroTimer()
    {
        nitro = true;
        
        nitroParticles.Play();

        cameraNitroParticles?.Play();
        

        yield return new WaitForSeconds(nitroDuration);


        nitroParticles.Stop();

        cameraNitroParticles?.Stop();
        
        nitro = false;

        yield return new WaitForSeconds(nitroCoolDown);

        //Nitro charging animation
        nitroTimerHolder = null;
    }

    private void Start()
    {
        vInput = 0;
        hInput = 0;

        _baseForwardFriction = wheelColliderss[0].forwardFriction;
        _baseSidewaysFriction = wheelColliderss[0].sidewaysFriction;



        if (!RaceController.i)
        {
            playerCar = true;
        }
        rb = GetComponent<Rigidbody>();
        currentSidewaysFriction = sidewaysFriction;
        currentForwardsFriction = forwardsFriction;

        nitroParticles.Stop();
        nitroParticles.Clear();

//#if UNITY_ANDROID
          autoAccelerating = true;
//#endif
    }

 
    
    private void OnTriggerExit(Collider other)
    {
        if (RaceController.i)
        {
            if (other.tag == "Start Region")
            {
                //RaceController.i.CrossedLine(this);
                //Create an inheritence structure for this
            }
        }
    }


    [HideInInspector]
    public bool playerCar = false;

    float turnBiasSign = -1;
    bool lastBacking = false;

    public void FixedUpdate()
    {
        vertical = 0;
        horizontal = 0;
        bool brake = false;

#if UNITY_ANDROID && !UNITY_EDITOR
          //Accelerometer
          Vector3 vectorValue = Input.acceleration;
          //Debug.Log("Detected vector: " + vectorValue);
          hInput = Mathf.Clamp(vectorValue.x * tiltInputMultiplier, -1, 1);
#endif
        if (playerCar)
        {
            //vertical = Input.GetAxis("Vertical");
            //horizontal = Input.GetAxis("Horizontal");

            horizontal = hInput;

            float biasedHorizontalCubicComplement = 0.2f + 0.8f * (1 - Mathf.Abs(Mathf.Pow(horizontal, 3)));

            //drifting condition
            if(Mathf.Abs(horizontal) >= driftTurnThreshold && speed >= driftSpeed && !drifting)
            {
                if(backing && !lastBacking) //backing was pressed in this frame
                {
                    turnBiasSign = Mathf.Sign(horizontal);
                    drifting = true;
                }
            }





            if (backing) { 
                
                if(drifting)
                {
                    if (accelerating || autoAccelerating)
                    {
                        vertical = 0.8f;
                    }
                    else
                    {
                        vertical = 0.8f * vInput;
                    }
                }
                else
                {
                    vertical = -0.5f;
                }
            }
            else
            if (autoAccelerating)
            {
                vertical = 1 * biasedHorizontalCubicComplement;
            }
            else if (accelerating)
            {
                vertical = 1 * biasedHorizontalCubicComplement;
            }
            else
            {

                vertical = vInput;
            }

            if (nitro)
            {
                vertical = vertical + nitroFactor;
            }


            //Exit drift condition
            if((!backing) && drifting)
            {
                drifting = false;
            }


            //brake = Input.GetKey(KeyCode.Space);
        }
        
 


        //Checking grounding through the wheel colliders rather than a dedicated grounding collider
        for (int i = 0; i < wheelColliderss.Length; i++)
        {
            if (wheelColliderss[i].isGrounded)
            {
                isOnGround = true;
                break;
            }
            else
                isOnGround = false;
        }



        Vector3 forwardsVelocity = Vector3.Project(rb.velocity, transform.forward);
        Vector3 sidewaysVelocity = Vector3.Project(rb.velocity, transform.right);

        Vector3 forces = Vector3.zero;

        float targetForwardsFriction = forwardsFriction;
        float targetSidewaysFriction = sidewaysFriction;
        turnBias = 0;

        float currentAcceleration = acceleration;




        if (backing && isOnGround && drifting)
        {
            turnBias = horizontal * turnBiasFactor + turnBiasSign * turnBiasBias;
            horizontal += turnBias;
            targetSidewaysFriction = 1;
            //targetForwardsFriction = 0;
        }else if (!drifting)
        {

        }

        lastBacking = backing;


        currentForwardsFriction = Mathf.Lerp(currentForwardsFriction, targetForwardsFriction, 0.5f);
        currentSidewaysFriction = Mathf.Lerp(currentSidewaysFriction, targetSidewaysFriction, 0.5f);


        Vector3 frictionVelocity = Vector3.zero;

        if (forwardsVelocity.magnitude > currentForwardsFriction)
        {
            frictionVelocity += -forwardsVelocity.normalized * currentForwardsFriction;
        }
        else
        {
            frictionVelocity = -forwardsVelocity;
        }

        if (sidewaysVelocity.magnitude > currentSidewaysFriction)
        {
            frictionVelocity += -sidewaysVelocity.normalized * currentSidewaysFriction;
        }
        else
        {
            frictionVelocity += -sidewaysVelocity;
        }

        // Modify force application
        if (isOnGround) {
            forces += transform.forward * currentAcceleration * vertical;
            
            if(!stuckTriggered)
            forces += frictionVelocity / Time.fixedDeltaTime;
        }

        // air resistance
        forces += -rb.velocity.sqrMagnitude * (rb.velocity.normalized) * airResistance;

        if (!stuckTriggered)
        {
            rb.AddForce(Vector3.down * gravityValue, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(Vector3.down * gravityValue * 0.2f, ForceMode.Acceleration);
        }
    
        
        rb.AddForce(forces, ForceMode.Acceleration);

        speed = Vector3.ProjectOnPlane(rb.velocity, transform.up).magnitude;
        
        

        // turning

        if (Vector3.Dot(rb.velocity, transform.forward) < 0)
            horizontal *= -1;

        float steeringValue = SteeringValue(speed);

        //Directly changing the angle causes a teleporting effect, thus using a torque addition instead
        //transform.eulerAngles += new Vector3(0, (horizontal + turnBias) * steeringValue * angularVelocity * Time.fixedDeltaTime, 0);
        if (isOnGround)
            rb.AddTorque(-Vector3.up * (horizontal) * steeringValue * angularVelocity * Time.fixedDeltaTime, ForceMode.VelocityChange);

        // Fix steering at low speeds
        float velocityMagnitude = rb.velocity.magnitude;
        bool isEffectivelyMoving = velocityMagnitude > 0.5f;
        if (isEffectivelyMoving && Vector3.Dot(rb.velocity, transform.forward) < 0)
            horizontal *= -1;

// Improve wheel collider steering logic
        float wheelColliderSteerAngle = Mathf.Clamp(1 / (wheelColliderSteerTime * Mathf.Max(velocityMagnitude, 1.0f) + 0.1f), 0, 25);
        //Debug.Log(wheelColliderSteerAngle);
        if (wheelMeshes[0] && wheelMeshes[1])
        {
            //wheelMeshes[0].localEulerAngles = new Vector3(0, horizontal*20, 0);
            //wheelMeshes[1].localEulerAngles = new Vector3(0, horizontal*20, 0);

            wheelColliderss[0].steerAngle = horizontal * wheelColliderSteerAngle * Mathf.Sign(Vector3.Dot(transform.forward, rb.velocity));
            wheelColliderss[1].steerAngle = horizontal * wheelColliderSteerAngle * Mathf.Sign(Vector3.Dot(transform.forward, rb.velocity));
        }





        // Calculate the pitch and roll angles relative to the world up vector
        float pitchAngle = -Vector3.SignedAngle(Vector3.ProjectOnPlane(Vector3.up, transform.right), transform.up, transform.right);
        float rollAngle = -Vector3.SignedAngle(Vector3.ProjectOnPlane(Vector3.up, transform.forward), transform.up, transform.forward);

        // Calculate pitch and roll correction based on current angles
        pitchCorrection = 0;
        rollCorrection = 0;

        if (Mathf.Abs(pitchAngle) > maxPitchAngle)
        {
            pitchCorrection = pitchAngle - maxPitchAngle * Mathf.Sign(pitchAngle);
        }
        if (Mathf.Abs(rollAngle) > maxRollAngle)
        {
            rollCorrection = rollAngle - maxRollAngle * Mathf.Sign(maxRollAngle);
        }


        // Apply corrective torque (invariant to mass)
        rb.AddTorque(transform.right * pitchCorrection * pitchSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        rb.AddTorque(transform.forward * rollCorrection * rollSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

        //transform.eulerAngles += new Vector3(pitchCorrection * pitchSpeed * Time.fixedDeltaTime, 0, rollCorrection * rollSpeed * Time.fixedDeltaTime);

        rb.AddForce(Vector3.up * (Mathf.Abs(pitchCorrection) + Mathf.Abs(rollCorrection)) * Time.fixedDeltaTime * correctionLiftAmount * 0.01f, ForceMode.Acceleration);



        //Gravity

       


        if (IsOnGround)
        {
            carVisual.transform.rotation = transform.rotation;
        }

        if (RaceController.i)
        {
            if (transform.position.y < -10)
            {
                //RaceController.i.Respawn(this);
                //Use an inheritence structure
            }
        }
    }
}
