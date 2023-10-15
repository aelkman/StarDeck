using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Potion")]
public class Potion : ScriptableObject
{
    public Sprite spriteUI;
    public Sprite spriteUISelected;
    public Sprite spriteStore;
    public Sprite spriteStoreSelected;
    public Sprite spriteStoreDisabled;
    public new string name;
    public string codeName;
    public string description;
    public int price;
}
