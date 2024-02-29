using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource buttonPress;
    public AudioSource cardRustling;
    public AudioSource heal;
    public AudioSource coins;
    public AudioSource blood;
    public AudioSource frozen;
    public AudioSource freeze;
    public AudioSource arcanePower;
    public AudioSource shield;
    public AudioSource glassBreak;
    public AudioSource counter;
    public AudioSource munchin;
    public AudioSource purchase;
    public AudioSource ping;
    public AudioSource negativeFeedback;
    public AudioSource hammer;
    public AudioSource currentBattleMusic;
    public AudioSource sword;
    public AudioSource blind;
    public AudioSource taunt;
    public AudioSource mapHover;
    public AudioSource vaporwaveSong1;
    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
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

    public void PlayButtonPress() {
        buttonPress.Stop();
        buttonPress.Play();
    }

    public void PlayCardRustling() {
        cardRustling.Play();
    }

    public void PlayHeal() {
        heal.Play();
    }

    public void PlayCoins() {
        coins.Play();
    }

    public void PlayBlood() {
        blood.Play();
    }

    public void PlayFrozen() {
        frozen.Play();
    }

    public void PlayFreeze() {
        freeze.Play();
    }

    public void PlayArcanePower() {
        arcanePower.Play();
    }

    public void PlayShield() {
        shield.Play();
    }

    public void PlayGlassBreak() {
        glassBreak.Play();
    }

    public void PlayCounter() {
        counter.Play();
    }

    public void PlayMunchin() {
        munchin.Play();
    }

    public void PlayPurchase() {
        purchase.Play();
    }

    public void PlayPing() {
        ping.Play();
    }

    public void PlayNegativeFeedback() {
        negativeFeedback.Play();
    }

    public void PlayHammerAudio() {
        hammer.Play();
    }

    public void PlayBlind() {
        blind.Play();
    }

    public void PlaySword() {
        sword.Play();
    }

    public void PlayTaunt() {
        taunt.Play();
    }

    public void PlayMapHover() {
        mapHover.Play();
    }

    public void PlayVaporwaveSong1() {
        vaporwaveSong1.volume = 0f;
        vaporwaveSong1.Play();
        StartCoroutine(FadeAudioSource.StartFade(vaporwaveSong1, 1, 1));
    }

    public void StopVaporwaveSong1() {
        StartCoroutine(FadeAudioSource.StartFadeToStop(vaporwaveSong1, 1, 0));
    }
}
