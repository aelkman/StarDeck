using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public TextMeshPro healthDisplay;
    // public GameDataManager gdm;
    public PlayerStats player;
    // Start is called before the first frame update
    void Start()
    {
        // for now, gameData is ONLY the Stats object
        healthDisplay.text = player.stats.health.ToString() + "/" + player.stats.maxHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        healthDisplay.text = player.stats.health.ToString() + "/" + player.stats.maxHealth.ToString();
    }
}
