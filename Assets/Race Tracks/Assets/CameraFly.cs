using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraFly : MonoBehaviour
{
    public float velocity = 1;
    public float angularVelocity = 1;
    private void Start()
    {
        

    }

    public Slider velocitySlider;
    public Slider angularVelocitySlider;

    public void setVelocity()
    {
        velocity = velocitySlider.value;
    }
    public void setAngularVelocity()
    {
        angularVelocity = angularVelocitySlider.value;
    }

    public bool movingForward = false;
    public bool movingBackward = false;
    public bool turningUpward = false;
    public bool turningDownward = false;
    public bool turningLeftward = false;
    public bool turningRightward = false;

    public void SetMovingForward(bool value)
    {
        movingForward = value;
    }

    public void SetMovingBackward(bool value)
    {
        movingBackward = value;
    }

    public void SetTurningUpward(bool value)
    {
        turningUpward = value;
    }

    public void SetTurningDownward(bool value)
    {
        turningDownward = value;
    }

    public void SetTurningLeftward(bool value)
    {
        turningLeftward = value;
    }

    public void SetTurningRightward(bool value)
    {
        turningRightward = value;
    }




    private void Update()
    {
        if (movingForward || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * velocity * Time.deltaTime;
        }
        if (movingBackward || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.position += -transform.forward * velocity * Time.deltaTime;
        }

        if (turningUpward || Input.GetKey(KeyCode.Space))
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x - Time.deltaTime * angularVelocity, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

        if (turningDownward || Input.GetKey(KeyCode.LeftShift))
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + Time.deltaTime * angularVelocity, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

        if (turningLeftward || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y - Time.deltaTime * angularVelocity, transform.localEulerAngles.z);
        }

        if (turningRightward || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + Time.deltaTime * angularVelocity, transform.localEulerAngles.z);
        }
    }
}
