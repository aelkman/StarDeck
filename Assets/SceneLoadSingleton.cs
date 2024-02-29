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
        mapCanvas = GameObject.Find("Map Canvas");

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
        // Debug.Log("Start: SceneLoaded1");
    }

    void OnDestroy() {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

        // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Debug.Log("OnSceneLoaded: " + scene.name);  
        Debug.Log("lastScene: " + GameManager.Instance.lastSceneName);

        if (scene.name == "Map") {
            if(mapCanvas != null) {
                Debug.Log("setting map canvas active!");
                mapCanvas.SetActive(true);
            }
            if(GameManager.Instance.lastSceneName != "Weapons") {
                GameObject.Find("Crossfade").GetComponent<Animator>().SetTrigger("FadeIn");
            }
            // cg.alpha = 1;
            // cg.interactable = true;
            // cg.blocksRaycasts = true;
        }

        GameManager.Instance.lastSceneName = scene.name;

    }

    private void OnSceneUnloaded(Scene current)
    {
        // Debug.Log("OnSceneUnloaded: " + current);
        if (current.name == "Map") {
            if(mapCanvas != null) {
                mapCanvas.SetActive(false);
            }
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

    void Update()
    {
    }
}
