using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAudio : MonoBehaviour
{
    public AudioSource hitDestination;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayHitDestination() {
        hitDestination.Play();
    }
}
