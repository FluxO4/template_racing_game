using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckPreventionTrigger : MonoBehaviour
{
    public CarHybrid carHybrid;

    private void OnTriggerStay(Collider other)
    {
        carHybrid.stuckTriggered = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        carHybrid.stuckTriggered = false;
    }
    
    //Is there a way to ensure that carHybrid.stuckTriggered is definitely false when there is no collision?

}
