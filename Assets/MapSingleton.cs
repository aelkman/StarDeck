using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapSingleton : MonoBehaviour, IDisposable
{
    private static MapSingleton _instance;

    public static MapSingleton Instance 
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
        var mapCam = GameObject.Find("Map Camera");
        Instance.GetComponent<Canvas>().worldCamera = mapCam.GetComponent<Camera>();
    }

    public void Dispose() {
        _instance = null;
        Destroy(this.gameObject);
    }
}
