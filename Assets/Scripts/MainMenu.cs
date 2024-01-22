using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int selectedTrack = 0;

    public int selectedCar = 0;

    public List<GameObject> cars = new List<GameObject>();

    public List<Image> trackMinimaps = new List<Image>();

    public Transform carShowPoint;
    public GameObject currentCar;
    public Image trackMinimapImage;
    public Image trackMinimapImage2;

    public List<string> raceScenes = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        selectedTrack = PlayerPrefs.GetInt("Selected Track", 0);
        selectedCar = PlayerPrefs.GetInt("Selected Car", 0);
        ReplaceCar();
        ReplaceTrack();
        
    }


    #region Button Handlers
    public void NextTrackButtonHandler()
    {
        selectedTrack++;
        selectedTrack = (selectedTrack + trackMinimaps.Count) % trackMinimaps.Count;
        ReplaceTrack();
        PlayerPrefs.SetInt("Selected Track", selectedTrack);
    }

    public void PrevTrackButtonHandler()
    {
        selectedTrack--;
        selectedTrack = (selectedTrack + trackMinimaps.Count) % trackMinimaps.Count;
        ReplaceTrack();
        PlayerPrefs.SetInt("Selected Track", selectedTrack);
    }


    public void NextCarButtonHandler()
    {
        selectedCar++;
        selectedCar = (selectedCar + cars.Count) % cars.Count;
        ReplaceCar();
        PlayerPrefs.SetInt("Selected Car", selectedCar);
    }

    public void PrevCarButtonHandler()
    {
        selectedCar--;
        selectedCar = (selectedCar + cars.Count) % cars.Count;
        ReplaceCar();
        PlayerPrefs.SetInt("Selected Car", selectedCar);
    }



    public void StartRace()
    {
        SceneManager.LoadScene(raceScenes[selectedTrack], LoadSceneMode.Single);
    }


    #endregion

    public void ReplaceCar()
    {
        Quaternion t = currentCar.transform.rotation;
        Destroy(currentCar);
        currentCar = Instantiate(cars[selectedCar], carShowPoint.transform.position, t);
    }

    public void ReplaceTrack()
    {
        trackMinimapImage.sprite = trackMinimaps[selectedTrack].sprite;
        trackMinimapImage2.sprite = trackMinimapImage.sprite;
    }






    // Update is called once per frame
    void Update()
    {
        
    }
}
