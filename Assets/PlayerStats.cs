using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Stats stats;
    public GameDataManager gdm;
    public HealthBar healthBar;
    public ManaBar manaBar;
    private int block = 0;
    private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        // for now, gameData is ONLY the Stats object
        stats = gdm.gameData;
        healthBar.SetMaxHealth(stats.maxHealth);
        manaBar.SetMana(stats.maxMana, stats.mana);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void useMana(int manaCost) {
        stats.mana -= manaCost;
        manaBar.SetMana(stats.mana, stats.maxMana);
    }

    public void resetMana() {
        stats.mana = stats.maxMana;
        manaBar.SetMana(stats.mana, stats.maxMana);
    }

    public void takeDamage(int damage) {
        stats.health -= damage;
        healthBar.SetHealth(stats.health);
        if (stats.health <= 0) {
            isDead = true;
        }
    }

    public void addBlock(int block) {
        this.block += block;
        Debug.Log("block is: " + this.block);
    }

    public void setBlock(int block) {
        this.block = block;
        Debug.Log("block is: " + this.block);
    }

    public int getBlock() {
        return block;
    }

    public void resetBlock() {
        block = 0;
    }

    public bool hasBlock() {
        if(block > 0) {
            return true;
        }
        else return false;
    }
}
