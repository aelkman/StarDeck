using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeckSingleton : MonoBehaviour
{

    private static DeckSingleton _instance;

    public static DeckSingleton Instance 
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
