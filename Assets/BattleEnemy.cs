using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Enemy", menuName = "Enemy")]
public class BattleEnemy : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    public int maxHealth;
    public int health;
    public float xOffset;
    public float yOffset;
    public float nextMoveYOffset;
    public Material material;
    public Vector3 scale;
    public bool isMiniBoss;
    public bool isBoss;
}
