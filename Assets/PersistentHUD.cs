using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentHUD : MonoBehaviour
{
    private static PersistentHUD _instance;
    public DeckViewer deckViewer;
    public GameObject discardLocation;

    public static PersistentHUD Instance 
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
