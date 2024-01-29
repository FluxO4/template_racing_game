using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceController : MonoBehaviour
{
    // Start is called before the first frame update




    public List<GameObject> playerCarPrefabs = new List<GameObject>();
    public GameObject aiCarPrefab;
    public GameObject playerCameraPrefab;
    public GameObject carTokenPrefab;

    public CarController playerCar;
    public FollowerCamera playerCamera;


    public List<CarController> CarsInRace = new List<CarController>();
    public List<Transform> TokensInRace = new List<Transform>();

    


    //RaceController's Start is supposed to be run after all other starts
    void Start()
    {
        
        int selectedCar = PlayerPrefs.GetInt("Selected Car", 0);
        int lastRaceRank = PlayerPrefs.GetInt("Last Race Rank", 1);
        int playerStartSpot = 0;

        //Find a player car in the game, if not, instantiate a new one
        GameObject tempPlayerCar = GameObject.FindGameObjectWithTag("Player Car");
   
        if (!tempPlayerCar)
        {
            playerCar = Instantiate(playerCarPrefabs[selectedCar]).GetComponent<CarController>();
        }
        else
        {
            playerCar = tempPlayerCar.GetComponent<CarController>();
        }

        playerCamera = Camera.main.GetComponent<FollowerCamera>();
        if (!playerCamera)
        {
            Destroy(Camera.main);
            playerCamera = Instantiate(playerCameraPrefab).GetComponent<FollowerCamera>();
            playerCamera.gameObject.tag = "MainCamera";
        }
        playerCamera.target = playerCar.transform;
        playerCar.playerCar = true;


        //If a track exists and has start spots, place the player and generate AI cars in the start spots
        if (Track.i)
        {
            Track.i.Initialise();

            if(Track.i.startSpots.Count > 0)
            {
                if(Track.i.startSpots.Count >= lastRaceRank)
                {
                    playerStartSpot = lastRaceRank-1;
                }

                for(int i = 0; i < Track.i.startSpots.Count; i++)
                {
                    if(i != playerStartSpot)
                    {
                        CarController t = Instantiate(aiCarPrefab).GetComponent<CarController>();
                        t.transform.position = Track.i.startSpots[i].transform.position;
                        t.transform.rotation = Track.i.startSpots[i].transform.rotation;
                        playerCamera.transform.position = playerCar.transform.position - playerCar.transform.forward * 5f + playerCar.transform.up * 2f;
                        CarsInRace.Add(t);
                    }
                    else
                    {
                        playerCar.transform.position = Track.i.startSpots[i].transform.position;
                        playerCar.transform.rotation = Track.i.startSpots[i].transform.rotation;
                        CarsInRace.Add(playerCar);
                    }
                }

                for(int i = 0; i < CarsInRace.Count; i++)
                {
                    Transform t = Instantiate(carTokenPrefab).transform;
                    Vector3 newPos = CarsInRace[i].transform.position;
                    newPos.y = -40;
                    t.transform.position = newPos;
                    TokensInRace.Add(t);

                }
            }
        }
    }

    public void CrossedLine(Car car)
    {
        Debug.Log("Car crossed finish line!");
    }

    public void Respawn(Car car)
    {

    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < CarsInRace.Count; i++)
        {
            Vector3 newPos = CarsInRace[i].transform.position;
            newPos.y = -40;
            TokensInRace[i].transform.position = newPos;
        }
    }

    



    #region Singleton stuff
    public static RaceController i
    {
        get
        {
            return instance;
        }
    }
    public static RaceController instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Two Controllers in one scene, destroying one");
            Destroy(gameObject);
        }
    }

    #endregion
}

