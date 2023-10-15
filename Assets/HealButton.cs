using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealButton : MonoBehaviour
{
    public CanvasGroup cardSelectorGroup;
    public TextMeshProUGUI descriptionText;
    private int twentyPercentMax;
    public Button healButton;
    public PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        twentyPercentMax = (int)System.Math.Round(MainManager.Instance.playerMaxHealth * 0.20);
        descriptionText.text += "+" + twentyPercentMax.ToString() + " HP";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HealClick() {
        MainManager.Instance.HealPlayer(0.20);
        MainManager.Instance.playerHealth = playerStats.health;
        healButton.interactable = false;
        cardSelectorGroup.alpha = 0.5f;
        // cardSelectorGroup.interactable = false;
        cardSelectorGroup.blocksRaycasts = false;
    }
}
