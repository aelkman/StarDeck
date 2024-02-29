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
    public HUD_Icon_Buttons hudIconButtons;
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
        if(mapOpen && (OptionsMenu.Instance.menuActive || hudIconButtons.settingsActive || deckViewer.deckViewer.activeSelf)) {
            // do nothing
        }
        else {
            if(SceneManager.GetActiveScene().name != "Map") {
                // Debug.Log("Scene name: " + SceneManager.GetActiveScene());
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
                    if(SceneManager.GetActiveScene().name == "Battle") {
                        HandManager handManager = GameObject.Find("HandManager").GetComponent<HandManager>();
                        handManager.canvasGroup.blocksRaycasts = false;
                    }

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
                    if(SceneManager.GetActiveScene().name == "Battle") {
                        HandManager handManager = GameObject.Find("HandManager").GetComponent<HandManager>();
                        handManager.canvasGroup.blocksRaycasts = true;
                    }
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
        hudIconButtons.CloseOptionsSettings();
        if(deckViewer.deckViewer.activeSelf) {
            deckViewer.ToggleActive();
        }
    }
}
