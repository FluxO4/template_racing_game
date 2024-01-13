using UnityEngine;

public class FollowerCamera : MonoBehaviour
{
    public float followDistance;
    public float followHeight;
    
    public float rotationLerpRate;
    public float heightLerpRate;

    public Transform target;

    public bool flipWithMovement = false;

    Vector3 rotation = new Vector3();

    void FixedUpdate()
    {
        Vector3 targetMotionDir = target.InverseTransformDirection(target.GetComponent<Rigidbody>().velocity);
        rotation.y = target.eulerAngles.y + (targetMotionDir.z > -0.1f || !flipWithMovement ? 0 : 180f); // flip camera if the car goes backwards

        float cameraAngle = Mathf.LerpAngle(transform.eulerAngles.y, rotation.y, rotationLerpRate * Time.fixedDeltaTime);
        transform.position = target.position - Quaternion.Euler(0, cameraAngle, 0) * Vector3.forward * followDistance;

        float cameraHeight = Mathf.Lerp(transform.position.y, target.position.y + followHeight, heightLerpRate * Time.fixedDeltaTime);
        transform.position = new Vector3(transform.position.x, cameraHeight, transform.position.z);

        transform.LookAt(target);
    }
}
