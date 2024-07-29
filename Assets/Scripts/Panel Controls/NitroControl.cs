using UnityEngine;
using UnityEngine.EventSystems;

public class NitroControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
     public CarHybrid carHybrid;

     public void OnPointerDown(PointerEventData eventData)
     {
          if (carHybrid != null)
          {
               carHybrid.nitro = true;
               Debug.Log("Nitro is pressed: " + carHybrid.nitro);
          }
     }

     public void OnPointerUp(PointerEventData eventData)
     {
          if (carHybrid != null)
          {
               carHybrid.nitro = false;
               Debug.Log("Nitro is pressed: " + carHybrid.nitro);
          }
     }
}
