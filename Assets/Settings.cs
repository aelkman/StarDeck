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
    public Slider sfxSlider;
    public Slider musicSlider;
    public Toggle cardHoverToggle;
    public Toggle damageAnalyticsToggle;
    public Toggle screenShakeToggle;
    public Toggle vignetteToggle;
    public Toggle chromeAberToggle;
    public Toggle fullScreenToggle;
    private bool showDamageAnalytics = false;
    public bool showScreenShake = true;
    public bool showVignette = true;
    public bool showChromAber = true;
    private bool isFullScreen = true;
    public float defaultMusicVolume = 0.7f;
    private float sfxFadeInTime = 0.5f;
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
        var musicFloat = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        musicSlider.value = musicFloat;
        musicMixer.SetFloat("MusicVolume", Mathf.Log10(musicFloat) * 20);

        StartCoroutine(delaySFXChange(sfxFadeInTime));

        var cardHover = PlayerPrefs.GetInt("CardHover", 1) == 1 ? true : false;
        cardHoverToggle.isOn = cardHover;
        GameManager.Instance.cardHoverDetails = cardHover;

        var da = PlayerPrefs.GetInt("DamageAnalytics", 0) == 1 ? true : false;
        damageAnalyticsToggle.isOn = da;
        showDamageAnalytics = da;
        damageAnalytics.SetActive(da);
        
        var ss = PlayerPrefs.GetInt("ScreenShake", 1) == 1 ? true : false;
        screenShakeToggle.isOn = ss;
        showScreenShake = ss;

        var vin = PlayerPrefs.GetInt("Vignette", 1) == 1 ? true : false;
        vignetteToggle.isOn = vin;
        showVignette = vin;

        var ca = PlayerPrefs.GetInt("ChromAber", 1) == 1 ? true : false;
        chromeAberToggle.isOn = ca;
        showChromAber = ca;

        var fs = PlayerPrefs.GetInt("FullScreen", 1) == 1 ? true : false;
        fullScreenToggle.isOn = !fs;
        isFullScreen = fs;
        FullScreenMode mode;
        if(fs) {
            mode = FullScreenMode.ExclusiveFullScreen;
        }
        else {
            mode = FullScreenMode.Windowed;
        }
        Screen.SetResolution(1920, 1080, mode);

    }

    private IEnumerator delaySFXChange(float time) {
        // sfxMixer.SetFloat("SFXVolume", Mathf.Log10(0.0001f) * 20);

        yield return new WaitForSeconds(time);

        var sfxFloat = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        sfxSlider.value = sfxFloat;
        sfxMixer.SetFloat("SFXVolume", Mathf.Log10(sfxFloat) * 20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleCardHoverDetails() {
        if(!GameManager.Instance.cardHoverDetails) {
            PlayerPrefs.SetInt("CardHover", 1);
        }
        else {
            PlayerPrefs.SetInt("CardHover", 0);
        }
        AudioManager.Instance.PlayButtonPress();
        GameManager.Instance.cardHoverDetails = !GameManager.Instance.cardHoverDetails;
    }

    public void ToggleAnalyticsDamage() {
        if(!showDamageAnalytics) {
            PlayerPrefs.SetInt("DamageAnalytics", 1);
        }
        else {
            PlayerPrefs.SetInt("DamageAnalytics", 0);
        }
        AudioManager.Instance.PlayButtonPress();
        showDamageAnalytics = !showDamageAnalytics;
        damageAnalytics.SetActive(showDamageAnalytics);
    }

    public void ToggleScreenShake() {
        if(!showScreenShake) {
            PlayerPrefs.SetInt("ScreenShake", 1);
        }
        else {
            PlayerPrefs.SetInt("ScreenShake", 0);
        }
        AudioManager.Instance.PlayButtonPress();
        showScreenShake = !showScreenShake;
    }

    public void ToggleVignette() {
        if(!showVignette) {
            PlayerPrefs.SetInt("Vignette", 1);
        }
        else {
            PlayerPrefs.SetInt("Vignette", 0);
        }
        AudioManager.Instance.PlayButtonPress();
        showVignette = !showVignette;
    }

    public void ToggleChromaticAbberation() {
        if(!showChromAber) {
            PlayerPrefs.SetInt("ChromAber", 1);
        }
        else {
            PlayerPrefs.SetInt("ChromAber", 0);
        }
        AudioManager.Instance.PlayButtonPress();
        showChromAber = !showChromAber;
    }

    public void ToggleWindowed() {
        if(!isFullScreen) {
            PlayerPrefs.SetInt("FullScreen", 1);
        }
        else {
            PlayerPrefs.SetInt("FullScreen", 0);
        }
        AudioManager.Instance.PlayButtonPress();
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
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);
    }

    public void SetMusicVolume(float sliderValue) {
        musicMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
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
