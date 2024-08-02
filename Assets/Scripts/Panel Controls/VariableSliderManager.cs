using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VariableSliderManager : MonoBehaviour
{
     public GameObject variableSliderPrefab;
     public CarHybrid carHybrid;  // Reference to your CarHybrid script
     public CameraTiltAndShift cameraTiltAndShift;  // Reference to your CameraTiltAndShift script

    public Transform sliderParent;

     [System.Serializable]
     public class SliderConfig
     {
          public string variableName;
          public float minValue;
          public float maxValue;
        public float defaultValue;
          public string playerPrefKey;
     }

     public SliderConfig[] sliderConfigs;
    public List<VariableSlider> sliderObjects = new List<VariableSlider>();

     void Start()
     {
        sliderConfigs = new SliderConfig[]
        {
            new SliderConfig {
                 variableName = "acceleration",
                 minValue = 0,
                 maxValue = 100,
                 defaultValue = 17f,
                 playerPrefKey = "Acceleration"
            },
            new SliderConfig {
                 variableName = "angularVelocity",
                 minValue = -5,
                 maxValue = 5,
                 defaultValue = -2.2f,
                 playerPrefKey = "AngularVelocity"
            },
            new SliderConfig {
                 variableName = "steeringA",
                 minValue = -5,
                 maxValue = 5,
                 defaultValue = 1.5f,
                 playerPrefKey = "SteeringA"
            },
            new SliderConfig {
                 variableName = "steeringB",
                 minValue = -0.1f,
                 maxValue = 0.1f,
                 defaultValue = 0.05f,
                 playerPrefKey = "SteeringB"
            },
            new SliderConfig {
                 variableName = "steeringC",
                 minValue = -0.5f,
                 maxValue = 0.5f,
                 defaultValue = 0.297f,
                 playerPrefKey = "SteeringC"
            },
            new SliderConfig {
                 variableName = "nitroFactor",
                 minValue = 1,
                 maxValue = 5,
                 defaultValue = 2f,
                 playerPrefKey = "NitroFactor"
            },
            new SliderConfig {
                 variableName = "nitroDuration",
                 minValue = 0,
                 maxValue = 10,
                 defaultValue = 2f,
                 playerPrefKey = "NitroDuration"
            },
            new SliderConfig {
                 variableName = "nitroCoolDown",
                 minValue = 0,
                 maxValue = 10,
                 defaultValue = 5,
                 playerPrefKey = "NitroCoolDown"
            },
            new SliderConfig {
                 variableName = "forwardsFriction",
                 minValue = 0,
                 maxValue = 0.1f,
                 defaultValue = 0.04f,
                 playerPrefKey = "ForwardsFriction"
            },
            new SliderConfig {
                 variableName = "sidewaysFriction",
                 minValue = 0,
                 maxValue = 20,
                 defaultValue = 10,
                 playerPrefKey = "SidewaysFriction"
            },
            new SliderConfig {
                 variableName = "airResistance",
                 minValue = 0,
                 maxValue = 0.05f,
                 defaultValue = 0.003f,
                 playerPrefKey = "AirResistance"
            },
            new SliderConfig {
                 variableName = "gravityValue",
                 minValue = 0,
                 maxValue = 30,
                 defaultValue = 15.685f,
                 playerPrefKey = "GravityValue"
            },
            new SliderConfig {
                 variableName = "turnBiasFactor",
                 minValue = 0f,
                 maxValue = 5,
                 defaultValue = 0.0f,
                 playerPrefKey = "TurnBiasFactor"
            },
            new SliderConfig {
                 variableName = "turnBiasBias",
                 minValue = 0,
                 maxValue = 5,
                 defaultValue = 0.513f,
                 playerPrefKey = "TurnBiasBias"
            },
            new SliderConfig {
                 variableName = "suspensionDistance",
                 minValue = 0,
                 maxValue = 1,
                 defaultValue = 0.5f,
                 playerPrefKey = "SuspensionDistance"
            },
            //slider configs for tilt amount, shift amount and shift lerp rate
            new SliderConfig
            {
               variableName = "tiltAmount",
               minValue = 0,
               maxValue = 180,
               defaultValue = 96.83f,
               playerPrefKey = "TiltAmount"
            },
            new SliderConfig
            {
               variableName = "shiftAmount",
               minValue = 0,
               maxValue = 20,
               defaultValue = 3.372f,
               playerPrefKey = "ShiftAmount"
            },
            new SliderConfig
            {
               variableName = "shiftLerpRate",
               minValue = 0,
               maxValue = 20,
               defaultValue = 3.519f,
               playerPrefKey = "ShiftLerpRate"
            }
          };

          foreach (var config in sliderConfigs)
          {
               InstantiateSlider(config);
          }
     }

    public void resetValuesToDefault()
    {
        foreach(var sliderObject in sliderObjects)
        {
            sliderObject.SetValueDirectly(sliderObject.myConfig.defaultValue);
        }
    }

    public void copyValuesToClipboard()
    {
        string toCopy = "";
        foreach(var sliderObject in sliderObjects)
        {
            toCopy += sliderObject.myConfig.variableName + ": " + sliderObject.slider.value;
            toCopy += "\n";
        }

        GUIUtility.systemCopyBuffer = toCopy;
        Debug.Log("Data copied to clipboard!");
    }

    void InstantiateSlider(SliderConfig config)
     {
          GameObject sliderInstance = Instantiate(variableSliderPrefab, sliderParent);
          VariableSlider variableSlider = sliderInstance.GetComponent<VariableSlider>();
          sliderObjects.Add(variableSlider);

          variableSlider.SetupSlider(config.minValue, config.maxValue, GetVariableSetter(config.variableName), config.playerPrefKey, config);

     }

     System.Action<float> GetVariableSetter(string variableName)
     {
          switch (variableName)
          {
               case "acceleration":
                    return value => carHybrid.acceleration = value;
               case "angularVelocity":
                    return value => carHybrid.angularVelocity = value;
               case "steeringA":
                    return value => carHybrid.steeringA = value;
               case "steeringB":
                    return value => carHybrid.steeringB = value;
               case "steeringC":
                    return value => carHybrid.steeringC = value;
               case "nitroFactor":
                    return value => carHybrid.nitroFactor = value;
               case "nitroDuration":
                    return value => carHybrid.nitroDuration = value;
               case "nitroCoolDown":
                    return value => carHybrid.nitroCoolDown = value;
               case "forwardsFriction":
                    return value => carHybrid.forwardsFriction = value;
               case "sidewaysFriction":
                    return value => carHybrid.sidewaysFriction = value;
               case "airResistance":
                    return value => carHybrid.airResistance = value;
               case "gravityValue":
                    return value => carHybrid.gravityValue = value;
               case "turnBiasFactor":
                    return value => carHybrid.turnBiasFactor = value;
               case "turnBiasBias":
                    return value => carHybrid.turnBiasBias = value;
               case "suspensionDistance":
                    return value =>
                    {
                         foreach (var wheel in carHybrid.wheelColliderss)
                         {
                              wheel.suspensionDistance = value;
                         }
                    };
               case "tiltAmount":
                    return value => cameraTiltAndShift.tiltAmount = value;
               case "shiftAmount":
                    return value => cameraTiltAndShift.shiftAmount = value;
               case "shiftLerpRate":
                    return value => cameraTiltAndShift.shiftLerpRate = value;
               default:
                    Debug.LogError("Variable not found: " + variableName);
                    return null;
          }
     }
}