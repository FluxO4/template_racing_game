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

    [SerializeField]
    float speed;

    [SerializeField]
    float xTarget;
    [SerializeField]
    float zTarget;

    Rigidbody rb;

    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        speed = rb.velocity.magnitude;

        float scaleFactorForward = Mathf.Max(0, (speed - forwardsFriction) / speed);
        float scaleFactorSideways = Mathf.Max(0, (speed - sidewaysFriction) / speed);

        Vector3 forwardsVelocity = Vector3.Project(velocity, transform.forward);
        Vector3 sidewaysVelocity = velocity - forwardsVelocity;

        forwardsVelocity *= scaleFactorForward;
        sidewaysVelocity *= scaleFactorSideways;

        //velocity = forwardsVelocity + sidewaysVelocity;

        // friction code ends

        // air resistance

        // velocity *= 1f / (1 + speed * airResistance / 80000);

        Vector3 forces = Vector3.zero; ;

        if (brake)
        {
            vertical = 0;
            forces += (-rb.velocity / Time.fixedDeltaTime) * breakForce;
        }

        forces += transform.forward * acceleration * vertical;

        // friction
        forces += -(forwardsVelocity + sidewaysVelocity) / Time.fixedDeltaTime;

        // air resistance
        forces += - rb.velocity.sqrMagnitude * (rb.velocity.normalized / Time.fixedDeltaTime) * airResistance;

        

        rb.AddForce(forces);
        
        speed = Vector3.ProjectOnPlane(rb.velocity, transform.up).magnitude;

        // Vector3 newPosition = rb.position + velocity * Time.fixedDeltaTime;

        // transform.eulerAngles += new Vector3(Vector3.SignedAngle(transform.up, Vector3.up, transform.forward) * correctionFactor, 0, 0);
        transform.eulerAngles += new Vector3(0, horizontal * (speed < maxTurningSpeed ? speed : 0) * angularVelocity * Time.fixedDeltaTime, 0);

        // velocity += -transform.up * 9.8f;

        // rb.AddForce((velocity - rb.velocity) * Time.fixedDeltaTime);
        //rb.MovePosition(newPosition);

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, -transform.up, out hit, 2, LayerMask.GetMask("Track")))
        //{
        //    Quaternion quat;
        //    Vector3 normal = hit.normal;
        //    quat = Quaternion.FromToRotation(transform.up, normal);
        //    xTarget = quat.eulerAngles.x;
        //    zTarget = quat.eulerAngles.z;
        //} 
        //else
        {
            xTarget = 0;
            zTarget = 0;
        }

        transform.eulerAngles = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, xTarget, 1f), transform.eulerAngles.y, Mathf.LerpAngle(transform.eulerAngles.z, zTarget, 1f));


        // rb.AddTorque(transform.up * angularVelocity * horizontal);
        // transform.rotation *= Quaternion.FromToRotation(transform.forward, rb.velocity.normalized);
        // transform.rotation *= Quaternion.FromToRotation(transform.up, Vector3.up);
    }
}
