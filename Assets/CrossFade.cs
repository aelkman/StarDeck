using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFade : MonoBehaviour
{
    public MapManager mapManager;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        mapManager.transition = animator;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
