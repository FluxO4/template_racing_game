using System;
using System.Linq;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float acceleration = 10f;
    Vector3 velocity = Vector3.zero;

    public float forwardsFriction = 1f;
    public float sidewaysFriction = 10f;
    public float airResistance = 0.9f;

    public float angularVelocity = 5f;
    public float correctionFactor = 0.5f;
    public float maxTurningSpeed = 200;

    public float breakForce = 0.05f;

    public bool isOnGround = false;

    public bool IsOnGround
    {
        get { return isOnGround; }

        set { isOnGround = value; }

    }


    [SerializeField]
    float speed;

    [SerializeField]
    float xTarget;
    [SerializeField]
    float zTarget;

    Rigidbody rb;

    public Transform[] wheels = new Transform[4];
    Vector3[] wheelBuffer = new Vector3[4];

    public enum StuntState
    {
        None,
        Wheelie,
        Backflip,
        // front flip?
        BarrelRoll,
    }

    public StuntState stuntState = StuntState.None;
    public StuntState prevState = StuntState.None;

    [SerializeField]
    bool drifting;

    bool braking = false;

    Vector3 stuntTarget;

    public float turnBiasBias = 0.2f;
    public float turnBiasFactor = 1f;
    float turnBias = 0;



    float currentSidewaysFriction;
    float currentForwardsFriction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSidewaysFriction = sidewaysFriction;
        currentForwardsFriction = forwardsFriction;

        for (int i = 0; i < wheelBuffer.Length; ++i)
        {
            wheelBuffer[i] = wheels[i].position;
        }
    }


    public void FixedUpdate()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        bool brake = Input.GetKey(KeyCode.Space);

        // rb.velocity += transform.forward * velocity * vertical * Time.fixedDeltaTime;
        // rb.velocity = Vector3.RotateTowards(rb.velocity, transform.forward, Vector3.SignedAngle(rb.velocity, transform.forward, transform.up), Time.fixedDeltaTime);
        // transform.rotation *= Quaternion.FromToRotation(transform.forward, (transform.forward + (transform.right * horizontal) * Time.fixedDeltaTime).normalized);

        //velocity += vertical * transform.forward * acceleration * Time.fixedDeltaTime;


        //// friction code starts
        // speed = rb.velocity.magnitude;

        //float scaleFactorForward = Mathf.Max(0, (speed - forwardsFriction) / speed);
        //float scaleFactorSideways = Mathf.Max(0, (speed - sidewaysFriction) / speed);

        Vector3 forwardsVelocity = Vector3.Project(rb.velocity, transform.forward);
        Vector3 sidewaysVelocity = Vector3.Project(rb.velocity, transform.right);

        Vector3 forces = Vector3.zero;

        float targetForwardsFriction = forwardsFriction;
        float targetSidewaysFriction = sidewaysFriction;


        if (brake && isOnGround)
        {
            if (!braking && (Mathf.Abs(horizontal) > 0.5f || drifting))
            {
                if (drifting == false)
                    turnBias = horizontal * turnBiasFactor + Mathf.Sign(horizontal) * turnBiasBias;
                drifting = true;
                targetSidewaysFriction = 0;
                
            }
            else
            {
                targetForwardsFriction = 3;
                braking = true;
            }
        } else
        {
            drifting = false;
            braking = false;
            turnBias = 0;
        }


        currentForwardsFriction = Mathf.Lerp(currentForwardsFriction, targetForwardsFriction, 0.07f);
        currentSidewaysFriction = Mathf.Lerp(currentSidewaysFriction, targetSidewaysFriction, 0.07f);


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

        //forwardsVelocity *= scaleFactorForward;
        //sidewaysVelocity *= scaleFactorSideways;

        //velocity = forwardsVelocity + sidewaysVelocity;

        // friction code ends

        // air resistance

        // velocity *= 1f / (1 + speed * airResistance / 80000);

       
        if (isOnGround)
        {
            // accelerator
            forces += transform.forward * acceleration * vertical;
            // friction
            forces += frictionVelocity / Time.fixedDeltaTime;
        }

        // air resistance
        forces += -rb.velocity.sqrMagnitude * (rb.velocity.normalized) * airResistance;

        rb.AddForce(forces, ForceMode.Acceleration);

        speed = Vector3.ProjectOnPlane(rb.velocity, transform.up).magnitude;

        // Vector3 newPosition = rb.position + velocity * Time.fixedDeltaTime;

        // transform.eulerAngles += new Vector3(Vector3.SignedAngle(transform.up, Vector3.up, transform.forward) * correctionFactor, 0, 0);

        // turning

        if (Vector3.Dot(rb.velocity, transform.forward) < 0)
            horizontal *= -1;

        transform.eulerAngles += new Vector3(0, (horizontal + turnBias) * (speed < maxTurningSpeed ? speed : 0) * angularVelocity * Time.fixedDeltaTime, 0);

        // velocity += -transform.up * 9.8f;

        // rb.AddForce((velocity - rb.velocity) * Time.fixedDeltaTime);
        //rb.MovePosition(newPosition);

        // TODO: account for speed
        float angleStep = Time.fixedDeltaTime * 100;




        RaycastHit hit;

        bool raycastCollided = false;



        const float maxHitDistance = 2;
        for (int i = 0; i < wheels.Length; ++i)
        {
            Transform wheel = wheels[i];
            if (Physics.Raycast(wheel.position, -transform.up, out hit, maxHitDistance, LayerMask.GetMask("Track")))
            {
                //normal += hit.normal;

                //averagePosition += (wheel.position - transform.position) * hit.distance;
                //divisor += hit.distance;

                wheelBuffer[i] = hit.point - wheel.position;

                raycastCollided = true;
            }
            else
            {
                // wheelBuffer[i] = (wheel.position - transform.up * maxHitDistance);
                // averagePosition += (wheel.position - transform.position) * 2;
                // divisor += 2;
                // normal += Vector3.up * 2;
            }
        }

        // normal = (normal + averagePosition).normalized;

        Vector3 normal = Vector3.zero;

        for (int i = 0; i < wheelBuffer.Length; ++i)
        {
            int next = (i + 1) % wheelBuffer.Length;
            int prev = (wheelBuffer.Length + (i - 1)) % wheelBuffer.Length;

            Vector3 a = wheelBuffer[i] + wheels[i].position;
            Vector3 b = wheelBuffer[next] + wheels[next].position;
            Vector3 c = wheelBuffer[prev] + wheels[prev].position;
            Vector3 ab = b - a;
            Vector3 ac = c - a;

            Debug.DrawLine(a, b);
            Debug.DrawLine(a, c);

            normal += Vector3.Cross(ab, ac);
        }

        normal = normal.normalized;


        Quaternion stuntQuat = Quaternion.identity;

        if (!isOnGround)
        {
            normal = Vector3.Cross(rb.velocity, transform.right).normalized;
            angleStep = Time.fixedDeltaTime * 20;

        }

        stuntState = StuntState.None;

        if (!raycastCollided)
        {
            // stunts

            if (!IsOnGround) // ideally this will always be false when in here
            {
                
                Debug.DrawLine(transform.position, transform.position + transform.up * 5, Color.blue);
                Debug.DrawLine(transform.position, transform.position + stuntTarget * 5, Color.red);

                switch (stuntState)
                {
                    case StuntState.None:
                        if (Input.GetKey(KeyCode.C))
                        {
                            //stuntQuat = Quaternion.FromToRotation(transform.up, transform.up - transform.forward);
                            stuntTarget = transform.up - transform.forward * 15;
                            stuntState = StuntState.Wheelie;
                            angleStep = Time.fixedDeltaTime * 200;
                        }
                        else if (Input.GetKey(KeyCode.X))
                        {
                            //stuntQuat = Quaternion.FromToRotation(transform.up, transform.up - transform.forward);
                            stuntState = StuntState.Backflip;
                            stuntTarget = Vector3.up;
                            angleStep = Time.fixedDeltaTime * 150;
                        }
                        else if (Input.GetKey(KeyCode.Z))
                        {
                            angleStep = Time.fixedDeltaTime * 200;
                            stuntState = StuntState.BarrelRoll;
                            stuntTarget = Vector3.up;
                        }
                        else
                        {
                            angleStep = Time.fixedDeltaTime * 20;
                        }
                        break;

                    case StuntState.Wheelie:
                        //stuntQuat = Quaternion.FromToRotation(transform.up, transform.up - transform.forward);
                        angleStep = Time.fixedDeltaTime * 300;
                        break;

                    case StuntState.Backflip:
                        transform.eulerAngles -= new Vector3(5f, 0, 0);
                        break;

                    case StuntState.BarrelRoll:
                        float rollAngle = 180; // or any other value you prefer
                        normal = Quaternion.AngleAxis(rollAngle, transform.forward) * normal;
                        angleStep = Time.fixedDeltaTime * 300;
                        break;
                }


            }
        }

        Debug.DrawLine(transform.position, transform.position + transform.up - transform.forward, Color.red);
        Debug.DrawLine(transform.position, transform.position + transform.up - transform.right, Color.yellow);


        Quaternion quat = Quaternion.identity;
        if (stuntState == StuntState.None)
            quat = Quaternion.FromToRotation(transform.up, normal);


        xTarget = quat.eulerAngles.x;
        zTarget = quat.eulerAngles.z;


        transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * quat, angleStep);
        // transform.eulerAngles = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, xTarget, 1f), transform.eulerAngles.y, Mathf.LerpAngle(transform.eulerAngles.z, zTarget, 1f));


        // rb.AddTorque(transform.up * angularVelocity * horizontal);
        // transform.rotation *= Quaternion.FromToRotation(transform.forward, rb.velocity.normalized);
        // transform.rotation *= Quaternion.FromToRotation(transform.up, Vector3.up);
    }
}
