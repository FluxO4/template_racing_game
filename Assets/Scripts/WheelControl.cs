using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelControl : MonoBehaviour
{
    public Transform wheelModel;

    [HideInInspector] public WheelCollider WheelCollider;

    public bool steerable;
    public bool motorized;

    Quaternion basicRotation;

    // Start is called before the first frame update
    private void Start()
    {
        basicRotation = wheelModel.rotation;
        wheelModel.transform.rotation = basicRotation;
        WheelCollider = GetComponent<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        WheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        wheelModel.transform.position = position;
        wheelModel.transform.rotation = rotation * basicRotation;

        // wheelModel.transform.rotation = Quaternion.RotateTowards(wheelModel.transform.rotation, rotation * basicRotation, 150 * Time.deltaTime);
    }
}
