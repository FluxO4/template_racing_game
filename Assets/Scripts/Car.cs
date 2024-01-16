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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        
        for(int i = 0; i < wheelBuffer.Length; ++i)
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

        Vector3 frictionVelocity = Vector3.zero;

        if (forwardsVelocity.magnitude > forwardsFriction)
        {
            frictionVelocity += -forwardsVelocity.normalized * forwardsFriction;
        }
        else
        {
            frictionVelocity = -forwardsVelocity;
        }

        if (sidewaysVelocity.magnitude > sidewaysFriction)
        {
            frictionVelocity += -sidewaysVelocity.normalized * sidewaysFriction;
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

        Vector3 forces = Vector3.zero;

        if (brake && isOnGround)
        {
            vertical = 0;
            forces *= 0.9f;
        }

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

        transform.eulerAngles += new Vector3(0, horizontal * (speed < maxTurningSpeed ? speed : 0) * angularVelocity * Time.fixedDeltaTime, 0);

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


        Debug.DrawLine(transform.position, transform.position + normal * 10);

        if (!raycastCollided)
        {
            // stunts
            angleStep = Time.fixedDeltaTime * 20;
        }


        Quaternion quat = Quaternion.FromToRotation(transform.up, normal);



        xTarget = quat.eulerAngles.x;
        zTarget = quat.eulerAngles.z;



        transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * quat, angleStep);
        // transform.eulerAngles = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, xTarget, 1f), transform.eulerAngles.y, Mathf.LerpAngle(transform.eulerAngles.z, zTarget, 1f));


        // rb.AddTorque(transform.up * angularVelocity * horizontal);
        // transform.rotation *= Quaternion.FromToRotation(transform.forward, rb.velocity.normalized);
        // transform.rotation *= Quaternion.FromToRotation(transform.up, Vector3.up);
    }
}
