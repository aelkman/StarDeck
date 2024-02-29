using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Icon_Buttons : MonoBehaviour
{
    public bool settingsActive = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickSettingsIcon() {
        settingsActive = !settingsActive;
        if(OptionsMenu.Instance.menuActive) {
            OptionsMenu.Instance.ToggleButton();
        }
        AudioManager.Instance.PlayButtonPress();
        Settings.Instance.settingsMenu.SetActive(settingsActive);
    }

    public void ClickOptionsIcon() {
        if(settingsActive) {
            settingsActive = !settingsActive;
            Settings.Instance.settingsMenu.SetActive(settingsActive);
        }
        AudioManager.Instance.PlayButtonPress();
        OptionsMenu.Instance.ToggleButton();
    }

    public void CloseOptionsSettings() {
        if(settingsActive) {
            settingsActive = !settingsActive;
            Settings.Instance.settingsMenu.SetActive(settingsActive);
        }
        if(OptionsMenu.Instance.menuActive) {
            OptionsMenu.Instance.SetMenuActive(false);
        }
    }

    public void CloseSettings() {
        if(settingsActive) {
            settingsActive = !settingsActive;
            Settings.Instance.settingsMenu.SetActive(settingsActive);
        }
    }

}
