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

    public static GameManager Instance 
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
        MainManager.Instance.Dispose();
        MapSingleton.Instance.Dispose();
        var allTransforms = FindObjectsOfType<Transform>().ToList();
        var mapCam = GameObject.Find("Map Camera");
        var crossFade = GameObject.Find("Crossfade");
        var sceneLoad = GameObject.Find("SceneLoad");

        var audioManager = GameObject.Find("AudioManager");
        var gameManager = GameObject.Find("GameManager");

        HashSet<Transform> audioChildren = new HashSet<Transform>();
        foreach(Transform child in audioManager.transform) {
            audioChildren.Add(child);
        }

        allTransforms.RemoveAll(x => audioChildren.Contains(x));

        foreach( var tr in allTransforms)
        {
            if(tr.gameObject == audioManager || tr.gameObject == gameManager) {
                Debug.Log("found audio or game manager! not destroying");
            }
            else {
                Destroy(tr.gameObject);
            }
        }

    }

    public void RestartGame() {
        ResetGame();
        // then load the map again
        SceneManager.LoadScene("Map");
    }

    public void RestartToScene(string sceneName, Animator transition) {
        ResetGame();
        SceneManager.LoadScene(sceneName);
        // can't load transtion bc its dead :( )
        // StartCoroutine(LoadLevel(sceneName, transition));
    }

    private IEnumerator LoadLevel(string sceneName, Animator transition) {
        AudioManager.Instance.PlayButtonPress();
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene(sceneName);
    }
}
