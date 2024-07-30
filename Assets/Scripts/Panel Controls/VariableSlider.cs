using UnityEngine;
using UnityEngine.UI;

public class VariableSlider : MonoBehaviour
{
     public Slider slider;
     public Text nameText;
     public Text valueText;

     private string playerPrefKey;
     private System.Func<float> variableReference;

     public void SetupSlider(float minValue, float maxValue, System.Func<float> variableReference, string playerPrefKey)
     {
          this.playerPrefKey = playerPrefKey;
          this.variableReference = variableReference;

          slider.minValue = minValue;
          slider.maxValue = maxValue;

          // Load the value from PlayerPrefs
          float savedValue = PlayerPrefs.GetFloat(playerPrefKey, slider.minValue);
          slider.value = savedValue;

          // Set the initial value of the variable
          SetVariableValue(savedValue);

          // Update the UI texts
          nameText.text = playerPrefKey;
          valueText.text = savedValue.ToString("F2");

          // Add listener for value changes
          slider.onValueChanged.AddListener(OnSliderValueChanged);
     }

     private void OnSliderValueChanged(float value)
     {
          // Update the text field
          valueText.text = value.ToString("F2");

          // Save the value to PlayerPrefs
          PlayerPrefs.SetFloat(playerPrefKey, value);

          // Update the variable reference
          SetVariableValue(value);
     }

     private void SetVariableValue(float value)
     {
          if (variableReference != null)
          {
               var variableField = variableReference.Target.GetType().GetField(variableReference.Method.Name.Substring(4));
               if (variableField != null)
               {
                    variableField.SetValue(variableReference.Target, value);
               }
          }
     }
}
