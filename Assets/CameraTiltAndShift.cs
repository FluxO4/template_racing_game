using UnityEngine;

public class CameraTiltAndShift : MonoBehaviour
{
     public float tiltAmount = 90f; // Maximum tilt angle based on accelerometer input
     public float shiftAmount = 5f; // Maximum shift amount based on accelerometer input
    public float tiltLerpRate = 0.5f;
     public float shiftLerpRate = 2f; // Lerp rate for smooth shifting

     private float shift = 0f; // Global shift variable
     private float currentShift = 0f; // Current shift amount
     private Transform target; // Reference to the target

    private float currentTilt = 0f;

     private Vector3 initialLocalPosition; // Initial local position of the camera

     private void Start()
     {
          // Store the initial local position of the camera
          initialLocalPosition = transform.localPosition;

          // Find the parent object and get the FollowerCamera script to set the target
          FollowerCamera followerCamera = GetComponentInParent<FollowerCamera>();
          if (followerCamera != null)
          {
               target = followerCamera.target;
          }
     }

     void FixedUpdate()
     {
          if (target == null) return;

          // Get accelerometer input for tilt and shift
          Vector3 accelerometerInput = Input.acceleration;

          // Calculate tilt angle based on accelerometer input
          float tiltAngle = tiltAmount * accelerometerInput.x;

        float tiltDifference = Mathf.Abs(currentTilt - tiltAngle);

          currentTilt = Mathf.Lerp(currentTilt, tiltAmount, tiltLerpRate * Time.fixedDeltaTime * tiltDifference);

          // Apply tilt to the camera
          transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, tiltAngle);

          // Calculate shift based on accelerometer input
          shift = shiftAmount * accelerometerInput.x; // Update the global shift variable

          // Calculate the difference in shift
          float shiftDifference = Mathf.Abs(shift - currentShift);

          // Adjust current shift based on the shiftDifference and shiftLerpRate
          currentShift = Mathf.Lerp(currentShift, shift, shiftLerpRate * Time.fixedDeltaTime * shiftDifference);

          // Calculate the new local position for the camera
          Vector3 newLocalPosition = initialLocalPosition + transform.right * currentShift;

          // Move the camera smoothly to the new local position
          transform.localPosition = Vector3.Lerp(transform.localPosition, newLocalPosition, Time.fixedDeltaTime * shiftLerpRate);
     }
}
