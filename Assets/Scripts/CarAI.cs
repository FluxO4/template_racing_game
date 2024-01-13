using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    /*public Transform targetObject; // The target object the car will follow*/

    public GameObject targetParent; // Parent GameObject containing child targets
    private List<Transform> targets; // List to store all child targets
    private int currentTargetIndex = 0; // Index of the current target

    public float speed = 5f; // Speed of the car
    public float turnSpeed = 100f; // Turning speed of the car
    public float detectionDistance = 10f; // Detection distance for obstacles
    public LayerMask obstacleLayers; // Layers considered as obstacles

    private Rigidbody rb;
    private bool isAvoidingObstacle = false; // Flag for obstacle avoidance

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InitializeTargets();
    }

    void InitializeTargets()
    {
        targets = new List<Transform>();
        foreach (Transform child in targetParent.transform)
        {
            targets.Add(child);
        }
    }

    void FixedUpdate()
    {
        if (targets.Count == 0) return;

        Vector3 directionToTarget = GetDirectionToTarget();
        Vector3 avoidanceDirection = Vector3.zero;

        if (IsObstacleDetected())
        {
            isAvoidingObstacle = true;
            avoidanceDirection = GetWallAvoidanceDirection(directionToTarget);
        }
        else
        {
            isAvoidingObstacle = false;
        }

        Vector3 finalDirection = isAvoidingObstacle ? avoidanceDirection : directionToTarget;
        MoveCar(finalDirection);

        CheckIfTargetReached();
    }

    Vector3 GetDirectionToTarget()
    {
        Transform currentTarget = targets[currentTargetIndex];
        Vector3 targetPosition = currentTarget.position;
        targetPosition.y = transform.position.y; // Ignore Y-axis differences
        return (targetPosition - transform.position).normalized;
    }

    void CheckIfTargetReached()
    {
        Transform currentTarget = targets[currentTargetIndex];

        // Create new Vector3s ignoring the Y component for both car and target
        Vector3 carPositionXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetPositionXZ = new Vector3(currentTarget.position.x, 0, currentTarget.position.z);

        // Check the distance in the XZ plane only
        if (Vector3.Distance(carPositionXZ, targetPositionXZ) < 1f) // Threshold to consider target as reached
        {
            currentTargetIndex = (currentTargetIndex + 1) % targets.Count; // Move to the next target
            Debug.Log($"Switching target to {currentTargetIndex}");
        }
    }

    bool IsObstacleDetected()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance, obstacleLayers);
    }

    Vector3 GetWallAvoidanceDirection(Vector3 directionToTarget)
    {
        // Check for left and right side
        bool isLeftBlocked = Physics.Raycast(transform.position, -transform.right, detectionDistance, obstacleLayers);
        bool isRightBlocked = Physics.Raycast(transform.position, transform.right, detectionDistance, obstacleLayers);

        if (!isLeftBlocked)
        {
            return -transform.right; // Go left
        }
        else if (!isRightBlocked)
        {
            return transform.right; // Go right
        }
        else
        {
            // If both sides are blocked, use the original direction but slightly altered to try and find a way around
            return Quaternion.Euler(0, 45, 0) * directionToTarget;
        }
    }

    void MoveCar(Vector3 direction)
    {
       /* if (IsOnMeshCollider())*/
       if(true)
        {
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));
        }
    }

    bool IsOnMeshCollider()
    {
        RaycastHit hit;
        // Check if there's a MeshCollider directly below the car
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1f)) // 1f is the max distance to check, adjust as needed
        {
            if (hit.collider is MeshCollider)
            {
                return true;
            }
        }
        return false;
    }
}






//==========================MANUAL MOVEMENT ===============================================
/*       float moveVertical = Input.GetAxis("Vertical"); // W and S keys
       Vector3 movement = transform.forward * moveVertical * speed * Time.fixedDeltaTime;
       rb.MovePosition(rb.position + movement);

       // Turning
       float moveHorizontal = Input.GetAxis("Horizontal"); // A and D keys
       float turn = moveHorizontal * turnSpeed * Time.fixedDeltaTime;
       Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
       rb.MoveRotation(rb.rotation * turnRotation);

       // Braking
       if (Input.GetKey(KeyCode.Space))
       {
           Vector3 forwardVelocity = Vector3.Project(rb.velocity, transform.forward);
           rb.AddForce(-forwardVelocity * brakeForce); // Apply braking force against the forward velocity
       }*/
//==============================================