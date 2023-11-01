using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealButton : MonoBehaviour
{
    public CanvasGroup cardSelectorGroup;
    public TextMeshProUGUI descriptionText;
    private int healthHealing;
    public Button healButton;
    public PlayerStats playerStats;
    public float healPercent = 0.3333f;
    // Start is called before the first frame update
    void Start()
    {
        healthHealing = (int)System.Math.Round(MainManager.Instance.playerMaxHealth * healPercent);
        descriptionText.text = "Gain 1/3 max health<br>";
        descriptionText.text += "+" + healthHealing.ToString() + " HP";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HealClick() {
        AudioManager.Instance.PlayHeal();
        MainManager.Instance.HealPlayerPercent(healPercent);
        // playerStats.health = MainManager.Instance.playerHealth;
        healButton.interactable = false;
        cardSelectorGroup.alpha = 0.5f;
        // cardSelectorGroup.interactable = false;
        cardSelectorGroup.blocksRaycasts = false;
    }
}
