using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSingleton : MonoBehaviour
{
    private static TransitionSingleton _instance;

    public static TransitionSingleton Instance 
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
