using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTriggerAlt : MonoBehaviour
{
    // Start is called before the first frame update

    public CarSwiftPhys parentCar;

    void Awake()
    {
        parentCar = transform.parent.GetComponent<CarSwiftPhys>();
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
