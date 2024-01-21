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

    [Space(10)]
    // nitro stuff
    public int nitroCharges = 3;
    public int currentNitroCharges;     // public for debugging
    public float nitroCoolDown;
    public float currentNitroCoolDown;  // public for debugging
    public float nitroDuration;
    public float currentNitroDuration;  // public for debugging
    public float nitroFactor;

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
    float speed;

    [SerializeField]
    float xTarget;
    [SerializeField]
    float zTarget;

    Rigidbody rb;

    [Space(10)]

    public Transform[] wheelRaycasts = new Transform[4];
    Vector3[] wheelBuffer = new Vector3[4];

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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSidewaysFriction = sidewaysFriction;
        currentForwardsFriction = forwardsFriction;

        for (int i = 0; i < wheelBuffer.Length; ++i)
        {
            wheelBuffer[i] = wheelRaycasts[i].position;
        }

        currentNitroCharges = nitroCharges;

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.N)) // debug nitro gaining
        {
            currentNitroCharges++;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (currentNitroCoolDown == 0)
            {
                if (currentNitroCharges > 0)
                {
                    currentNitroDuration = nitroDuration;
                    currentNitroCoolDown = nitroCoolDown;
                    currentNitroCharges--;
                }
            }
        }

    }

    public void FixedUpdate()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        bool brake = Input.GetKey(KeyCode.Space);

        // the car visual is not exactly upright, so possible misalignment during stunts.
        // TODO: decide the threshold.
        if (Vector3.Dot(transform.up, carVisual.transform.up) < 0.99f)
        {
            // so fix the rotations and we disable all controls for a few seconds.
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

        transform.eulerAngles += new Vector3(0, (horizontal + turnBias) * (speed < maxTurningSpeed ? speed : 0) * angularVelocity * Time.fixedDeltaTime, 0);

        // TODO: account for speed
        float angleStep = Time.fixedDeltaTime * 100;

        bool raycastCollided = false;

        const float maxHitDistance = 1.5f;
        for (int i = 0; i < wheelRaycasts.Length; ++i)
        {
            Transform wheel = wheelRaycasts[i];
            if (Physics.Raycast(wheel.position, -transform.up, out RaycastHit hit, maxHitDistance, LayerMask.GetMask("Track")))
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
            }
        }

        Vector3 normal = Vector3.zero;

        for (int i = 0; i < wheelBuffer.Length; ++i)
        {
            int next = (i + 1) % wheelBuffer.Length;
            int prev = (wheelBuffer.Length + (i - 1)) % wheelBuffer.Length;

            Vector3 a = wheelBuffer[i] + wheelRaycasts[i].position;
            Vector3 b = wheelBuffer[next] + wheelRaycasts[next].position;
            Vector3 c = wheelBuffer[prev] + wheelRaycasts[prev].position;
            Vector3 ab = b - a;
            Vector3 ac = c - a;

            Debug.DrawLine(a, b);
            Debug.DrawLine(a, c);

            normal += Vector3.Cross(ab, ac);

            // NOTE: This is supposed to be the vertical motion of the wheels

            //wheelMeshes[i].transform.position = new Vector3(wheelRaycasts[i].transform.position.x, 
            //                                                wheelRaycasts[i].transform.position.y + wheelBuffer[i].y + wheelRadius,
            //                                                wheelRaycasts[i].transform.position.z);
        }

        normal = normal.normalized;



        if (!isOnGround)
        {
            normal = Vector3.Cross(rb.velocity, transform.right).normalized;
            angleStep = Time.fixedDeltaTime * 20;

        }

        stunting = false;

        if (!raycastCollided)
        {
            // stunts
            if (!IsOnGround) // ideally this will always be false when in here
            {
                if (Input.GetKey(KeyCode.X)) // rotate about X axis (Backflip)
                {
                    // Quaternion stuntQuat = Quaternion.FromToRotation(carVisual.transform.up, carVisual.transform.up - carVisual.transform.forward);
                    // carVisual.transform.rotation = Quaternion.RotateTowards(carVisual.transform.rotation, transform.rotation * stuntQuat, 5);
                    //Vector3 eulerRot = carVisual.transform.localEulerAngles;
                    // eulerRot.x -= 3;
                    // carVisual.transform.localEulerAngles = eulerRot;

                    carVisual.transform.localRotation = carVisual.transform.localRotation * Quaternion.AngleAxis(-3, Vector3.right);

                    angleStep = Time.fixedDeltaTime * 150;
                    stunting = true;
                }
                else if (Input.GetKey(KeyCode.Z)) // rotate about Z axis (Barrel Roll)
                {
                    carVisual.transform.localRotation = carVisual.transform.localRotation * Quaternion.AngleAxis(3, Vector3.forward);
                    angleStep = Time.fixedDeltaTime * 200;
                    stunting = true;
                }
                else
                {
                    angleStep = Time.fixedDeltaTime * 20;
                    stunting = false;
                }

            }
        }

        Debug.DrawLine(carVisual.transform.position, carVisual.transform.position + carVisual.transform.up * 3, Color.red);
        Debug.DrawLine(carVisual.transform.position, carVisual.transform.position + (carVisual.transform.up - carVisual.transform.forward).normalized * 3, Color.red);

        Quaternion quat = Quaternion.identity;
        if (!stunting)
            quat = Quaternion.FromToRotation(transform.up, normal);



        xTarget = quat.eulerAngles.x;
        zTarget = quat.eulerAngles.z;


        transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * quat, angleStep);

        if (IsOnGround)
        {
            carVisual.transform.rotation = transform.rotation;
        }
    }
}
