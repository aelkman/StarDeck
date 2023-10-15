using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudio : MonoBehaviour
{
    public AudioSource discardAudio;
    public AudioSource potionAudio;
    public AudioSource reloadAudio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPotionAudio() {
        potionAudio.Play();
    }

    public void PlayDiscardAudio() {
        discardAudio.Play();
    }

    public void PlayReloadAudio() {
        reloadAudio.Play();
    }
}
