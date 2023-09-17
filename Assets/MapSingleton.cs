using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSingleton : MonoBehaviour
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
    }
}
