using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Effect", menuName = "Attack Effect")]
public class AttackEffect : ScriptableObject
{
    public GameObject prefab;
    public Vector3 scale;
}
