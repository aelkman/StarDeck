using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplayEnemy : MonoBehaviour
{
    public TextMeshPro healthDisplay;
    // public GameDataManager gdm;
    public BattleEnemyContainer enemy;
    // Start is called before the first frame update
    void Start()
    {
        // for now, gameData is ONLY the Stats object
        enemy = transform.parent.parent.GetComponent<BattleEnemyContainer>();
        healthDisplay.text = enemy.getHealth().ToString() + "/" + enemy.getMaxHealth().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        healthDisplay.text = enemy.getHealth().ToString() + "/" + enemy.getMaxHealth().ToString();
    }
}
