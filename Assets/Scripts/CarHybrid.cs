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
     public float airResistance = 0.9f;
     public float gravityValue = 9.8f;

     public float angularVelocity = 5f;
     public float correctionFactor = 0.5f;
     public float maxTurningSpeed = 200;

     [Space(10)]
     // nitro stuff
     public int nitroCharges = 3;
     public int currentNitroCharges;     // public for debugging
     public float nitroCoolDown;
     public float currentNitroCoolDown;  // public for debugging
     public float nitroDuration;
     public float currentNitroDuration;  // public for debugging
     public float nitroFactor;
     public ParticleSystem nitroParticles;

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
                    speedText.text = (value*3.6f).ToString("F2")+" km/h";
               }
          }
     }


     [SerializeField]
     float xTarget;
     [SerializeField]
     float zTarget;

     Rigidbody rb;

     [Space(10)]

     public WheelCollider[] wheelColliderss = new WheelCollider[4];

     public Transform[] wheelMeshes = new Transform[4];


     public GameObject carVisual;

     [SerializeField]
     bool drifting;

     bool braking = false;

     public float turnBiasBias = 0.2f;
     public float turnBiasFactor = 1f;
     float turnBias = 0;



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

     float _vInput = 0;
     float _hInput = 0;
     public Text vInputText;
     public Text hInputText;

     public float vInput
     {
          get { return _vInput; }
          set
          {
               _vInput = value;
               if (vInputText != null)
               {
                    vInputText.text = value.ToString("F2");
               }
          }
     }
     public float hInput
     {
          get { return _hInput; }
          set
          {
               _hInput = value;
               if (hInputText != null)
               {
                    hInputText.text = value.ToString("F2");
               }
          }
     }

     public bool accelerating = false;
     public bool backing = false;
     public bool nitro = false;

     private JoystickControl controls;

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
     }

     public void OnAccelerate(InputAction.CallbackContext context)
     {
          if (context.performed)
          {
               accelerating = true;
          }
          else if (context.canceled)
          {
               accelerating = false;
          }
     }

     public void OnBack(InputAction.CallbackContext context)
     {
          if (context.performed)
          {
               backing = true;
          }
          else if (context.canceled)
          {
               backing = false;
          }
     }

     public void OnDrift(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            drifting = true;
        }else if (context.canceled)
        {
            drifting= false;
        }
    }
     float SteeringValue(float x) => steeringA * (steeringC * x) / (1 + steeringB * (steeringC * x) * (steeringC * x));
     public void AccelerationOn()
     {
          accelerating = true;
     }
     public void AccelerationOff()
     {
          accelerating = false;
     }

     private void Start()
     {
          vInput = 0;
          hInput = 0;

          if (!RaceController.i)
          {
               playerCar = true;
          }
          rb = GetComponent<Rigidbody>();
          currentSidewaysFriction = sidewaysFriction;
          currentForwardsFriction = forwardsFriction;

          nitroParticles.Stop();
          nitroParticles.Clear();
          currentNitroCharges = nitroCharges;

#if UNITY_ANDROID
          accelerating = true;
#endif

     }

     private void Update()
     {

          if (Input.GetKeyDown(KeyCode.N)) // debug nitro gaining
          {
               currentNitroCharges++;
          }

          if (Input.GetKeyDown(KeyCode.C) || nitro)
          {
               Debug.Log("Nitro!");
               if (currentNitroCoolDown == 0)
               {
                    if (currentNitroCharges > 0)
                    {
                         currentNitroDuration = nitroDuration;
                         if (nitroParticles != null)
                         {
                              var mainModule = nitroParticles.main;
                              mainModule.duration = nitroDuration;
                              nitroParticles.Play();
                         }
                         currentNitroCoolDown = nitroCoolDown;
                         currentNitroCharges--;
                    }
               }
               nitro = false;
          }

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

     public void FixedUpdate()
     {
          float vertical = 0;
          float horizontal = 0;
          bool brake = false;

#if UNITY_ANDROID
          //Accelerometer
          Vector3 vectorValue = Input.acceleration;
          //Debug.Log("Detected vector: " + vectorValue);
          hInput = vectorValue.x;
#endif
          if (playerCar)
          {
               //vertical = Input.GetAxis("Vertical");
               //horizontal = Input.GetAxis("Horizontal");

               horizontal = hInput;

               float biasedHorizontalParabolicComplement = 0.1f + 0.9f * (1 - horizontal * horizontal);

               if (accelerating)
               {
                    vertical = 1 * biasedHorizontalParabolicComplement;
               }
               else if (backing)
               {
                    vertical = -1 * biasedHorizontalParabolicComplement;
               }
               else
               {

                    vertical = vInput;
               }


               brake = Input.GetKey(KeyCode.Space);
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

          float currentAcceleration = acceleration;




          if (currentNitroDuration > 0)
          {
               currentNitroDuration = Mathf.Max(0, currentNitroDuration - Time.fixedDeltaTime);
               currentAcceleration *= nitroFactor;

          }

          if (currentNitroCoolDown > 0)
          {
               currentNitroCoolDown = Mathf.Max(0, currentNitroCoolDown - Time.fixedDeltaTime);
          }


          if (brake && isOnGround)
          {
               if (!braking && (Mathf.Abs(horizontal) > 0.5f || drifting))
               {
                    if (drifting == false)
                         turnBias = horizontal * turnBiasFactor + Mathf.Sign(horizontal) * turnBiasBias;
                    drifting = true;
                    targetSidewaysFriction = 0;
                    targetForwardsFriction = 0;

               }
               else
               {
                    targetForwardsFriction = 3;
                    braking = true;
               }
          }
          else
          {
               drifting = false;
               braking = false;
               turnBias = 0;
          }


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

          if (isOnGround)
          {
               // accelerator
               forces += transform.forward * currentAcceleration * vertical;
               // friction
               forces += frictionVelocity / Time.fixedDeltaTime;
          }

          // air resistance
          forces += -rb.velocity.sqrMagnitude * (rb.velocity.normalized) * airResistance;

          rb.AddForce(forces, ForceMode.Acceleration);

          speed = Vector3.ProjectOnPlane(rb.velocity, transform.up).magnitude;

          // turning

          if (Vector3.Dot(rb.velocity, transform.forward) < 0)
               horizontal *= -1;

          float steeringValue = SteeringValue(speed);

          //Directly changing the angle causes a teleporting effect, thus using a torque addition instead
          //transform.eulerAngles += new Vector3(0, (horizontal + turnBias) * steeringValue * angularVelocity * Time.fixedDeltaTime, 0);

          rb.AddTorque(-Vector3.up * (horizontal + turnBias) * steeringValue * angularVelocity * Time.fixedDeltaTime, ForceMode.VelocityChange);


          float wheelColliderSteerAngle = Mathf.Clamp(1 / (wheelColliderSteerTime * rb.velocity.magnitude + 0.1f), 0, 25);
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

          rb.AddForce(Vector3.down * gravityValue, ForceMode.Acceleration);


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
