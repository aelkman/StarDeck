using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public static Settings Instance;
    public AudioMixer sfxMixer;
    public AudioMixer musicMixer;
    public GameObject settingsMenu;
    public GameObject damageAnalytics;
    private bool showDamageAnalytics = false;
    private bool isFullScreen = true;
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
        if(Screen.currentResolution.width != 1920 && Screen.currentResolution.height != 1080) {
            if(isFullScreen) {
                Screen.SetResolution(1920, 1080, true);
            }
            else {
                Screen.SetResolution(1920, 1080, false);
            }
        }
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CardHoverDetailsToggle() {
        GameManager.Instance.cardHoverDetails = !GameManager.Instance.cardHoverDetails;
    }

    public void AnalyticsDamageToggle() {
        showDamageAnalytics = !showDamageAnalytics;
        damageAnalytics.SetActive(showDamageAnalytics);
    }

    public void WindowedToggle() {
        // Screen.fullScreen = !Screen.fullScreen;
        isFullScreen = !isFullScreen;
        FullScreenMode mode;
        if(isFullScreen) {
            mode = FullScreenMode.ExclusiveFullScreen;
        }
        else {
            mode = FullScreenMode.Windowed;
        }
        Screen.SetResolution(1920, 1080, mode);
        // Screen.fullScreenMode = mode;
        // StartCoroutine(SwitchToWindowed(mode));
    }

    private IEnumerator SwitchToWindowed(FullScreenMode mode)
    {
        Screen.SetResolution(1920, 1080, mode);
        yield return null;
        // App.LogWarning("Screen mode has been set to: " + Screen.fullScreenMode);
    }

    public void SetSfxVolume(float sliderValue) {
        sfxMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusicVolume(float sliderValue) {
        musicMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void LeaveButton() {
        if(GameObject.Find("ButtonsRight") != null) {
            var hudButtons = GameObject.Find("ButtonsRight").GetComponent<HUD_Icon_Buttons>();
            hudButtons.settingsActive = !hudButtons.settingsActive;
        }
        AudioManager.Instance.PlayButtonPress();
        settingsMenu.SetActive(false);
    }
}
