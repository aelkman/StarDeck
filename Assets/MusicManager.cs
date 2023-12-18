using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource battle1;
    public AudioSource battle2;
    public AudioSource battle3;
    public AudioSource bossBattle;
    public AudioSource kingbotIntro; 
    public AudioSource kingbotWin;
    public List<AudioSource> battleTracks;
    // Start is called before the first frame update
    void Start()
    {
        if(MainManager.Instance.isBossBattle) {
            kingbotIntro.volume = 0;
            kingbotIntro.Play();
            StartCoroutine(FadeAudioSource.StartFade(kingbotIntro, 1, 1));
            AudioManager.Instance.currentBattleMusic = kingbotIntro;
        }
        else {
            var track = battleTracks[Random.Range(0, battleTracks.Count)];
            while(AudioManager.Instance.currentBattleMusic != null && AudioManager.Instance.currentBattleMusic == track) {
                track = battleTracks[Random.Range(0, battleTracks.Count)];
            }
            track.volume = 0;
            track.Play();
            StartCoroutine(FadeAudioSource.StartFade(track, 1, 0.8f));
            AudioManager.Instance.currentBattleMusic = track;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeTracksInOut(float fadeOutTime, AudioSource fadeOut, AudioSource fadeIn) {

        yield return StartCoroutine(FadeAudioSource.StartFade(fadeOut, fadeOutTime/2, 0));
        fadeOut.Stop();
        fadeIn.volume = 0f;
        fadeIn.Play();
        StartCoroutine(FadeAudioSource.StartFade(fadeIn, fadeOutTime/2, 1));
    }
}
