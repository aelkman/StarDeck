using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraSingleton : MonoBehaviour
{
    private static MapCameraSingleton _instance;

    public static MapCameraSingleton Instance 
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
}
