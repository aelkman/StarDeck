using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource regularBattle;
    public AudioSource bossBattle;
    public AudioSource kingbotIntro; 
    public AudioSource kingbotWin;
    // Start is called before the first frame update
    void Start()
    {
        if(MainManager.Instance.isBossBattle) {
            kingbotIntro.Play();
        }
        else {
            regularBattle.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeTracksInOut(float fadeOutTime, AudioSource fadeOut, AudioSource fadeIn) {

        yield return StartCoroutine(FadeAudioSource.StartFade(fadeOut, fadeOutTime/2, 0));
        fadeIn.volume = 0f;
        fadeIn.Play();
        StartCoroutine(FadeAudioSource.StartFade(fadeIn, fadeOutTime/2, 1));
        fadeOut.Stop();
    }
}
