using UnityEngine;
using UnityEngine.EventSystems;

public class NitroControl : MonoBehaviour, IPointerUpHandler
{
     public CarHybrid carHybrid;


     public void OnPointerUp(PointerEventData eventData)
     {
          if (carHybrid != null)
          {
            carHybrid.ActivateNitro();
               Debug.Log("Nitro is pressed: " + carHybrid.nitro);
          }
     }
}
