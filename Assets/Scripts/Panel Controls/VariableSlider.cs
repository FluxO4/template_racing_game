using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class VariableSlider : MonoBehaviour
{
     public Slider slider;
     public Text nameText;
     public Text valueText;
    public VariableSliderManager.SliderConfig myConfig;

     private string playerPrefKey;
     private System.Action<float> setVariableAction;

     public void SetupSlider(float minValue, float maxValue, System.Action<float> setVariableAction, string playerPrefKey)
     {
          this.playerPrefKey = playerPrefKey;
          this.setVariableAction = setVariableAction;

          slider.minValue = minValue;
          slider.maxValue = maxValue;

          // Load the value from the file
          float savedValue = LoadValueFromFile(playerPrefKey, slider.minValue);
          slider.value = savedValue;

          // Set the initial value of the variable
          setVariableAction(savedValue);

          // Update the UI texts
          nameText.text = playerPrefKey;
          valueText.text = savedValue.ToString("F3");

          // Add listener for value changes
          slider.onValueChanged.AddListener(OnSliderValueChanged);
     }


     public void SetValueDirectly(float value)
    {
        slider.value = value;
        slider.SetValueWithoutNotify(value);
        OnSliderValueChanged(value);
    }

     private void OnSliderValueChanged(float value)
     {
          // Update the text field
          valueText.text = value.ToString("F3");

          // Save the value to the file
          SaveValueToFile(playerPrefKey, value);

          // Update the variable reference
          setVariableAction(value);
     }

     private float LoadValueFromFile(string key, float defaultValue)
     {
          string path = Path.Combine(Application.streamingAssetsPath, "SliderValues.txt");
          if (File.Exists(path))
          {
               string[] lines = File.ReadAllLines(path);
               foreach (var line in lines)
               {
                    string[] parts = line.Split('=');
                    if (parts.Length == 2 && parts[0].Trim() == key)
                    {
                         if (float.TryParse(parts[1].Trim(), out float value))
                         {
                              return value;
                         }
                    }
               }
          }
          return defaultValue;
     }

     private void SaveValueToFile(string key, float value)
     {
          string path = Path.Combine(Application.streamingAssetsPath, "SliderValues.txt");
          string[] lines = File.Exists(path) ? File.ReadAllLines(path) : new string[0];
          bool found = false;
          for (int i = 0; i < lines.Length; i++)
          {
               string[] parts = lines[i].Split('=');
               if (parts.Length == 2 && parts[0].Trim() == key)
               {
                    lines[i] = $"{key} = {value}";
                    found = true;
                    break;
               }
          }
          if (!found)
          {
               using (StreamWriter sw = new StreamWriter(path, true))
               {
                    sw.WriteLine($"{key} = {value}");
               }
          }
          else
          {
               File.WriteAllLines(path, lines);
          }
     }
}
