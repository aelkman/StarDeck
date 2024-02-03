using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;

    public static MusicManager Instance 
    { 
        get { return _instance; } 
    } 
    public AudioSource bossBattle;
    public AudioSource kingbotIntro; 
    public AudioSource kingbotWin;
    public List<AudioSource> battleTracks;
    private int currentTrack = 0;
    public List<float> battleVolumes = new List<float> { 0.8f, 0.6f, 0.8f, 0.8f, 0.8f };

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

    public void PlaySong() {
        if(SceneManager.GetActiveScene().name == "Battle") {
            if(MainManager.Instance.isBossBattle) {
                kingbotIntro.volume = 0;
                kingbotIntro.Play();
                StartCoroutine(FadeAudioSource.StartFade(kingbotIntro, 1, 1));
                AudioManager.Instance.currentBattleMusic = kingbotIntro;
            }
            else {
                // if(AudioManager.Instance.currentBattleMusic != null) {
                //     Debug.Log("last song:" + AudioManager.Instance.currentBattleMusic.name);
                // }

                // remove random approach
                // var track = battleTracks[Random.Range(0, battleTracks.Count)];
                // while(AudioManager.Instance.currentBattleMusic != null && AudioManager.Instance.currentBattleMusic == track) {
                //     // Debug.Log("currentBattleMusic: " + AudioManager.Instance.currentBattleMusic.name);
                //     // Debug.Log("track: " + track.name);
                //     track = battleTracks[Random.Range(0, battleTracks.Count)];
                //     // Debug.Log("new track: " + track);
                // }

                var track = battleTracks[currentTrack];
                track.volume = 0;
                track.Play();
                StartCoroutine(FadeAudioSource.StartFade(track, 1, battleVolumes[currentTrack]));
                AudioManager.Instance.currentBattleMusic = track;
                // loop through tracklist
                currentTrack++;
                if(currentTrack == battleTracks.Count) {
                    currentTrack = 0;
                }
                // Debug.Log("currentBattleMusic: " + AudioManager.Instance.currentBattleMusic.name);
            }
        }
    }

    public IEnumerator FadeTracksInOut(float fadeOutTime, AudioSource fadeOut, AudioSource fadeIn) {

        yield return StartCoroutine(FadeAudioSource.StartFade(fadeOut, fadeOutTime/2, 0));
        fadeOut.Stop();
        fadeIn.volume = 0f;
        fadeIn.Play();
        StartCoroutine(FadeAudioSource.StartFade(fadeIn, fadeOutTime/2, 1));
    }
}
