using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PersistentHUD : MonoBehaviour
{
    private static PersistentHUD _instance;
    public DeckViewer deckViewer;
    public GameObject discardLocation;
    private bool mapOpen = false;
    private Camera mainCamera;
    private MapSingleton mapSingleton;
    private MapManager mapManager;
    private List<Light2D> lights;
    private GameObject eventMain;
    // private string lastScene = "";
    // private bool sameScene = false;

    public static PersistentHUD Instance 
    { 
        get { return _instance; } 
    } 


    void Awake()
    {
        mapManager = MapManager.Instance;
        if (_instance != null && _instance != this) 
        { 
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void MapButtonClick() {

        AudioManager.Instance.PlayButtonPress();
        // if(lastScene == SceneManager.GetActiveScene().name) {
        //     sameScene = true;
        // }
        // else {
        //     sameScene = false;
        // }
        // lastScene = SceneManager.GetActiveScene().name;

        // if in any other than map, change the camera settings to show map
        // else, we'll have to do something with the dice roller refactor (later)
        if(SceneManager.GetActiveScene().name != "Map") {

            if(MapSingleton.Instance != null) {
                mapSingleton = MapSingleton.Instance;
            }
            if(Camera.main != null) {
                mainCamera = Camera.main;
            }

            mapOpen = !mapOpen;

            // if(!sameScene) {
            lights = new List<Light2D>();
            foreach (Light2D light in FindObjectsOfType(typeof(Light2D))) {
                lights.Add(light);
            }
            // }

            if(GameObject.Find("Main") != null) {
                eventMain = GameObject.Find("Main");
            }

            // enable/disable the map
            if(mapOpen) {
                // eventMain is for events, and the Shop
                if(eventMain != null) {
                    eventMain.GetComponent<Canvas>().targetDisplay = 1;
                }
                mapManager.destinationsClickable = false;
                // remove lights if they exist
                if(lights != null) {
                    foreach (Light2D light in lights) {
                        // light.gameObject.SetActive(false);
                        light.enabled = false;
                    }
                }
                mainCamera.gameObject.SetActive(false);
                mapSingleton.gameObject.SetActive(true);
            }
            else {
                if(eventMain != null) {
                    eventMain.GetComponent<Canvas>().targetDisplay = 0;
                }
                mapManager.destinationsClickable = true;
                if(lights != null) {
                    foreach (Light2D light in lights) {
                        // light.gameObject.SetActive(true);
                        light.enabled = true;
                    }
                }
                mainCamera.gameObject.SetActive(true);
                mapSingleton.gameObject.SetActive(false);
            }
        }
        else {
            mapManager.destinationsClickable = true;
        }
    }
}
