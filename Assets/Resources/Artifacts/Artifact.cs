using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Artifact", menuName = "Artifact")]
public class Artifact : ScriptableObject
{
    public string codeName;
    public new string name;
    public string description;
    public Sprite artwork;
}
