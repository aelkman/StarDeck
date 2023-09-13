using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public TextMeshPro healthDisplay;
    // public GameDataManager gdm;
    public BaseCharacterInfo character;
    // Start is called before the first frame update
    void Start()
    {
        // for now, gameData is ONLY the Stats object
        healthDisplay.text = character.health.ToString() + "/" + character.maxHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        healthDisplay.text = character.health.ToString() + "/" + character.maxHealth.ToString();
    }
}
