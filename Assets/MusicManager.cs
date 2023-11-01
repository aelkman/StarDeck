using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource regularBattle;
    public AudioSource bossBattle;
    public AudioSource kingbotIntro; 
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

    public IEnumerator StartKingbotBattle(float fadeOutTime) {

        yield return StartCoroutine(FadeAudioSource.StartFade(kingbotIntro, fadeOutTime/2, 0));
        bossBattle.volume = 0f;
        bossBattle.Play();
        StartCoroutine(FadeAudioSource.StartFade(bossBattle, fadeOutTime/2, 1));
        kingbotIntro.Stop();
        bossBattle.Play();
    }
}
