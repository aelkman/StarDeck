using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemiesEnum {
    MiniBot,
    Rob,
    GoldBot,
    HealBot,
    ArmorBot,
    DroneBot,
    KingBot
}

[CreateAssetMenu(fileName = "New Enemy Group", menuName = "Enemy Group")]
public class EnemyGroup : ScriptableObject
{
    public List<EnemiesEnum> enemies;
    // private string miniBot = "MiniBot";
    // private string rob = "Rob";
    // private string goldBot = "GoldBot";

}
