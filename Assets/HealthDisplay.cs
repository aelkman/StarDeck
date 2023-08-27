using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public TextMeshProUGUI healthDisplay;
    public GameDataManager gdm;
    // Start is called before the first frame update
    void Start()
    {
        // for now, gameData is ONLY the Stats object
        healthDisplay.text = "Health: " + gdm.gameData.health.ToString() + "/" + gdm.gameData.maxHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        healthDisplay.text = "Health: " + gdm.gameData.health.ToString() + "/" + gdm.gameData.maxHealth.ToString();
    }
}
