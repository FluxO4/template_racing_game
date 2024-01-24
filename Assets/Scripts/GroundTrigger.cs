using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    CarController parentCar;

    void Awake()
    {
        parentCar = transform.parent.GetComponent<CarController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Car")
            parentCar.IsOnGround = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Car")
            parentCar.IsOnGround = true;
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Car")
            parentCar.IsOnGround = false;

    }

}
