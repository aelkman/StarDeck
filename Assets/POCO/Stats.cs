using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Stats {

    public int health;
    public int maxHealth;
    public int mana;
    public int maxMana;
    public List<string> weapons;

    public Stats() {
        weapons = new List<string>();
        weapons.Add("Blaster");
        BaseStats baseStats = new BaseStats();
        // Debug.Log("baseStatsHealth: " + baseStats.health);
        health = baseStats.health;
        maxHealth = baseStats.maxHealth;
        mana = baseStats.mana;
        maxMana = baseStats.mana;
    }
}