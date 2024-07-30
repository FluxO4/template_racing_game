using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VariableSliderManager : MonoBehaviour
{
     public GameObject variableSliderPrefab;
     public CarHybrid carHybrid;  // Reference to your Hybrid Car script

     [System.Serializable]
     public class SliderConfig
     {
          public string variableName;
          public float minValue;
          public float maxValue;
          public string playerPrefKey;
     }

     public SliderConfig[] sliderConfigs;

     System.Func<float> GetVariableReference(string variableName)
     {
          switch (variableName)
          {
               case "acceleration":
                    return () => carHybrid.acceleration;
               case "angularVelocity":
                    return () => carHybrid.angularVelocity;
               case "steeringA":
                    return () => carHybrid.steeringA;
               case "steeringB":
                    return () => carHybrid.steeringB;
               case "steeringC":
                    return () => carHybrid.steeringC;
               case "nitroFactor":
                    return () => carHybrid.nitroFactor;
               case "nitroDuration":
                    return () => carHybrid.nitroDuration;
               case "nitroCoolDown":
                    return () => carHybrid.nitroCoolDown;
               case "forwardsFriction":
                    return () => carHybrid.forwardsFriction;
               case "sidewaysFriction":
                    return () => carHybrid.sidewaysFriction;
               case "airResistance":
                    return () => carHybrid.airResistance;
               case "gravityValue":
                    return () => carHybrid.gravityValue;
               case "turnBiasFactor":
                    return () => carHybrid.turnBiasFactor;
               case "turnBiasBias":
                    return () => carHybrid.turnBiasBias;
               default:
                    Debug.LogError("Variable not found: " + variableName);
                    return null;
          }
     }


     void Start()
     {
          sliderConfigs = new SliderConfig[]
    {
       new SliderConfig {
            variableName = "acceleration",
            minValue = 0,
            maxValue = 100,
            playerPrefKey = "Acceleration"
       },
       new SliderConfig {
            variableName = "angularVelocity",
            minValue = -5,
            maxValue = 5,
            playerPrefKey = "AngularVelocity"
       },
       new SliderConfig {
            variableName = "steeringA",
            minValue = -5,
            maxValue = 5,
            playerPrefKey = "SteeringA"
       },
       new SliderConfig {
            variableName = "steeringB",
            minValue = -0.1f,
            maxValue = 0.1f,
            playerPrefKey = "SteeringB"
       },
       new SliderConfig {
            variableName = "steeringC",
            minValue = -0.5f,
            maxValue = 0.5f,
            playerPrefKey = "SteeringC"
       },
       new SliderConfig {
            variableName = "nitroFactor",
            minValue = 1,
            maxValue = 2,
            playerPrefKey = "NitroFactor"
       },
       new SliderConfig {
            variableName = "nitroDuration",
            minValue = 0,
            maxValue = 10,
            playerPrefKey = "NitroDuration"
       },
       new SliderConfig {
            variableName = "nitroCoolDown",
            minValue = 0,
            maxValue = 10,
            playerPrefKey = "NitroCoolDown"
       },
       new SliderConfig {
            variableName = "forwardsFriction",
            minValue = 0,
            maxValue = 0.1f,
            playerPrefKey = "ForwardsFriction"
       },
       new SliderConfig {
            variableName = "sidewaysFriction",
            minValue = 0,
            maxValue = 20,
            playerPrefKey = "SidewaysFriction"
       },
       new SliderConfig {
            variableName = "airResistance",
            minValue = 0,
            maxValue = 0.05f,
            playerPrefKey = "AirResistance"
       },
       new SliderConfig {
            variableName = "gravityValue",
            minValue = 0,
            maxValue = 30,
            playerPrefKey = "GravityValue"
       },
       new SliderConfig {
            variableName = "turnBiasFactor",
            minValue = 0.5f,
            maxValue = 5,
            playerPrefKey = "TurnBiasFactor"
       },
       new SliderConfig {
            variableName = "turnBiasBias",
            minValue = 0,
            maxValue = 1,
            playerPrefKey = "TurnBiasBias"
       },
    };
          foreach (var config in sliderConfigs)
          {
               InstantiateSlider(config);
          }
     }

     void InstantiateSlider(SliderConfig config)
     {
          GameObject sliderInstance = Instantiate(variableSliderPrefab, transform);
          VariableSlider variableSlider = sliderInstance.GetComponent<VariableSlider>();

          variableSlider.SetupSlider(config.minValue, config.maxValue, GetVariableReference(config.variableName), config.playerPrefKey);
     }

}
