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
        var allTransforms = FindObjectsOfType<Transform>();
        foreach( var tr in allTransforms)
        {
            Destroy(tr.gameObject);
        }
        // then load the map again
        SceneManager.LoadScene("Map");
        MapSingleton.Instance.GetComponent<Canvas>().worldCamera = MapCameraSingleton.Instance.GetComponent<Camera>();
    }
}
