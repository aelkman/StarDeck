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
        AudioManager.Instance.PlayButtonPress();
        Settings.Instance.settingsMenu.SetActive(settingsActive);
    }
}
