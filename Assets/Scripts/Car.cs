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

    [SerializeField]
    float speed;

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

        velocity += vertical * transform.forward * acceleration * Time.fixedDeltaTime;

        // friction code starts
        speed = velocity.magnitude;

        float scaleFactorForward = Mathf.Max(0, (speed - forwardsFriction) / speed);
        float scaleFactorSideways = Mathf.Max(0, (speed - sidewaysFriction) / speed);

        Vector3 forwardsVelocity = Vector3.Project(velocity, transform.forward);
        Vector3 sidewaysVelocity = velocity - forwardsVelocity;

        forwardsVelocity *= scaleFactorForward;
        sidewaysVelocity *= scaleFactorSideways;

        velocity = forwardsVelocity + sidewaysVelocity;

        // friction code ends

        // air resistance

        velocity *= 1f / (1 + speed * airResistance / 80000);


        if (brake)
        {
            velocity *= 0.95f;
        }

        speed = velocity.magnitude;

        Vector3 newPosition = rb.position + velocity * Time.fixedDeltaTime;

        // transform.eulerAngles += new Vector3(Vector3.SignedAngle(transform.up, Vector3.up, transform.forward) * correctionFactor, 0, 0);
        transform.eulerAngles += new Vector3(0, horizontal * (speed < maxTurningSpeed ? speed : 0) * angularVelocity * Time.fixedDeltaTime, 0);
        transform.eulerAngles = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, 0, 0.1f), transform.eulerAngles.y, Mathf.LerpAngle(transform.eulerAngles.z, 0, 0.5f));


        rb.MovePosition(newPosition);

        // rb.AddTorque(transform.up * angularVelocity * horizontal);
        // transform.rotation *= Quaternion.FromToRotation(transform.forward, rb.velocity.normalized);
        // transform.rotation *= Quaternion.FromToRotation(transform.up, Vector3.up);
    }
}
