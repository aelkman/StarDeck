using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Stats stats;
    public GameDataManager gdm;
    public HealthBar healthBar;
    private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        // for now, gameData is ONLY the Stats object
        stats = gdm.gameData;
        healthBar.SetMaxHealth(stats.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int damage) {
        stats.health -= damage;
        healthBar.SetHealth(stats.health);
        if (stats.health <= 0) {
            isDead = true;
        }
    }
}
