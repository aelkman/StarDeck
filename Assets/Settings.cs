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
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Screen.fullScreenMode = mode;
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
