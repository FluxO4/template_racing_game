using UnityEngine;
using UnityEngine.EventSystems;

public class BrakeControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
     public CarHybrid carHybrid;

     public void OnPointerDown(PointerEventData eventData)
     {
          if (carHybrid != null)
          {
               //carHybrid.accelerating = false;
               carHybrid.backing = true;
               Debug.Log("Brake is pressed: " + carHybrid.vertical);
          }
     }

     public void OnPointerUp(PointerEventData eventData)
     {
          if (carHybrid != null)
          {
               //carHybrid.accelerating = true;
               carHybrid.backing = false;
               Debug.Log("Brake is pressed: " + carHybrid.vertical);
          }
     }
     // Public methods to be called by Event Trigger
     public void PointerDown()
     {
          OnPointerDown(null);
     }

     public void PointerUp()
     {
          OnPointerUp(null);
     }
}
