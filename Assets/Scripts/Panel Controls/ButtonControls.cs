using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControls : MonoBehaviour
{
     public void ResetScene()
     {
          // Get the active scene
          Scene currentScene = SceneManager.GetActiveScene();

          // Reload the active scene
          SceneManager.LoadScene(currentScene.name);
     }
}
