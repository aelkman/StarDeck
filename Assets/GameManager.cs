using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

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
    }

    public void RestartGame() {
        MainManager.Instance.Dispose();
        MapSingleton.Instance.Dispose();
        var allTransforms = FindObjectsOfType<Transform>();
        var mapCam = GameObject.Find("Map Camera");
        var crossFade = GameObject.Find("Crossfade");
        var sceneLoad = GameObject.Find("SceneLoad");
        foreach( var tr in allTransforms)
        {
            // if(tr.gameObject == mapCam || tr.gameObject == sceneLoad) {
            //     Debug.Log("found map camera! not destroying");
            // }
            // else {
                Destroy(tr.gameObject);
            // }
        }
        // then load the map again
        SceneManager.LoadScene("Map");
    }
}
