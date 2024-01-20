using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Texture2D mouseTexture;
    public Texture2D mouseClickTex;
    private CursorMode cursorMode = CursorMode.ForceSoftware;
    private Vector2 hotSpot;
    private static GameManager _instance;
    public bool demoComplete = false;
    public int currentSceneUID = 0;
    public bool cardHoverDetails = true;
    public Dictionary<string, int> weaponDamage = new Dictionary<string, int>();

    public static GameManager Instance 
    { 
        get { return _instance; } 
    } 

    void Start() {
        weaponDamage.Add("Hammer", 0);
        weaponDamage.Add("Blaster", 0);
        weaponDamage.Add("Artifacts", 0);
        weaponDamage.Add("General", 0);
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
        
        hotSpot = new Vector2(mouseTexture.width/2, mouseTexture.height/2);
        Cursor.SetCursor(mouseTexture, hotSpot, cursorMode);
    }

    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            Cursor.SetCursor(mouseClickTex, hotSpot, cursorMode);
        }
        if(Input.GetMouseButtonUp(0)) {
            Cursor.SetCursor(mouseTexture, hotSpot, cursorMode);
        }
    }

    public void ResetGame() {
        if(MainManager.Instance != null) {
            MainManager.Instance.Dispose();
        }
        if(MapSingleton.Instance != null) {
            MapSingleton.Instance.Dispose();
        }
        var allTransforms = FindObjectsOfType<Transform>().ToList();
        var mapCam = GameObject.Find("Map Camera");
        var crossFade = GameObject.Find("Crossfade");
        var sceneLoad = GameObject.Find("SceneLoad");

        var audioManager = GameObject.Find("AudioManager");
        var gameManager = GameObject.Find("GameManager");
        // var analytics = GameObject.Find("Analytics");
        var settings = GameObject.Find("Settings");
        var options = GameObject.Find("Options");

        // save all audio manager children
        HashSet<Transform> savedChildren = new HashSet<Transform>();
        foreach(Transform child in audioManager.transform) {
            savedChildren.Add(child);
        }

        
        foreach(Transform child in gameManager.GetComponentsInChildren<Transform>()) {
            savedChildren.Add(child);
        }

        allTransforms.RemoveAll(x => savedChildren.Contains(x));

        foreach( var tr in allTransforms)
        {
            if(tr.gameObject == audioManager || tr.gameObject == gameManager || tr.gameObject == settings
                || tr.gameObject == options ) {
                // Debug.Log("found an important object! not destroying");
            }
            else {
                Destroy(tr.gameObject);
            }
        }
    }

    public void RestartGame() {
        ResetGame();
        // then load the map again
        GameManager.Instance.LoadScene("Weapons");
    }

    public void RestartToScene(string sceneName, Animator transition) {
        ResetGame();
        GameManager.Instance.LoadScene(sceneName);
        // can't load transtion bc its dead :( )
        // StartCoroutine(LoadLevel(sceneName, transition));
    }

    private IEnumerator LoadLevel(string sceneName, Animator transition) {
        AudioManager.Instance.PlayButtonPress();
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1.0f);

        GameManager.Instance.LoadScene(sceneName);
    }

    public void LoadScene(string name) {
        currentSceneUID += 1;
        SceneManager.LoadScene(name);
    }
}
