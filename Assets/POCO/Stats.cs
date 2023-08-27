using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Stats {

    public int health;
    public int maxHealth;

    public Stats() {
        BaseStats baseStats = new BaseStats();
        Debug.Log("baseStatsHealth: " + baseStats.health);
        health = baseStats.health;
        maxHealth = baseStats.maxHealth;
    }
}