using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    public float speed = 5f; // The speed of the sphere

    void Start()
    {
        
    }

    private void FixedUpdate()
    {

        float moveVertical = Input.GetAxis("Vertical");

        float rotateHorizontal = Input.GetAxis("Horizontal");

        Vector3 force = (Vector3.ProjectOnPlane(transform.forward, Vector3.up)).normalized * moveVertical * speed * Time.fixedDeltaTime;
        rb.AddForce(force * 50f);
    }


}
