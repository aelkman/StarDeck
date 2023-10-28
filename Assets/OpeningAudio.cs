using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class OpeningAudio : MonoBehaviour
{
    public AudioSource song1;
    public AudioSource song2;
    public AudioSource song3;
    public AudioSource hit1;
    public AudioSource hit2;

    
    public float magnitude = 10f;
    public float roughness = 10f;
    public float fadeInTime = 0.1f;
    public float fadeOutTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySong1() {
        song1.Play();
    }

    public void PlaySong2WithFade(float fadeOutTime) {
        StartCoroutine(PlaySong2Timed(fadeOutTime));
    }

    public IEnumerator PlaySong2Timed(float fadeOutTime) {
        yield return StartCoroutine(FadeAudioSource.StartFade(song1, fadeOutTime/2, 0));
        song2.volume = 0f;
        song2.Play();
        StartCoroutine(FadeAudioSource.StartFade(song2, fadeOutTime/2, 1));
    }

    public void PlayHits() {
        StartCoroutine(PlayHitSequence());
    }

    private IEnumerator PlayHitSequence() {
        yield return new WaitForSeconds(0.2f);
        CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
        hit1.Play();
        yield return new WaitForSeconds(1.5f);
        CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
        hit2.Play();
        yield return new WaitForSeconds(0.3f);
        hit2.Stop();
        CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
        hit2.Play();
    }

    public void PlayHit2() {
        hit2.Play();
    }
}
