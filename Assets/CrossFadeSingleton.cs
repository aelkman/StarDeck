using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFadeSingleton : MonoBehaviour
{
    private static CrossFadeSingleton _instance;

    public static CrossFadeSingleton Instance 
    { 
        get { return _instance; } 
    }

    public Animator crossfadeAnimator;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
