using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Enemy", menuName = "Enemy")]
public class BattleEnemy : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    public float maxHealth;
    public float health;
    public float xOffset;
    public Material material;
    public Vector3 scale;
}
