using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadSingleton : MonoBehaviour
{
    // public Animator transition;
    // private CanvasGroup cg;
    public GameObject mapCanvas;

    private static SceneLoadSingleton _instance;

    public static SceneLoadSingleton Instance 
    { 
        get { return _instance; } 
    } 


    void Awake()
    {
        if (_instance != null && _instance != this) 
        { 
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        mapCanvas = GameObject.Find("Map Canvas");
        // if (transition == null) {
        //     transition = GameObject.Find("Crossfade").GetComponent<Animator>();
        // }
        // // reset animator to Entry state
        // transition.Rebind();
        // transition.Update(0f);
        // cg = transition.gameObject.GetComponent<CanvasGroup>();
        // cg.alpha = 1;
        // cg.interactable = true;
        // cg.blocksRaycasts = true;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("Start: SceneLoaded1");
    }

        // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);  
        
        if (scene.name == "Map") {
            mapCanvas.SetActive(true);
            // cg.alpha = 1;
            // cg.interactable = true;
            // cg.blocksRaycasts = true;
        }
        Debug.Log(mode);
    }

    private void OnSceneUnloaded(Scene current)
    {
        Debug.Log("OnSceneUnloaded: " + current);
        if (current.name == "Map") {
            mapCanvas.SetActive(false);
            // cg.alpha = 0;
            // cg.interactable = false;
            // cg.blocksRaycasts = false;
        }
        // else if (current.name == "Battle") {
        //     mapCanvas.SetActive(true);
        //     // cg.alpha = 1;
        //     // cg.interactable = true;
        //     // cg.blocksRaycasts = true;
        // }
        // else if (current.name == "Shop") {
        //     mapCanvas.SetActive(true);
        // }
    }

    private void OnSceneLoaded(Scene current) {
        Debug.Log("OnSceneLoaded: " + current);

    }

    void Update()
    {
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy");
    }
}
