using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject, ISerializationCallbackReceiver
{
    public new string name;
    public Sprite artwork;
    public int manaCost;
    public List<string> actionKeys;
    public List<string> actionValues;
    public Dictionary<string,string> actions;
    public string description;
    public string type;
    public string subType;
    public bool isTarget;
    public bool isAttack;
    public bool isPower;
    public bool isSkill;
    public bool isStarter;
    public string rarity;

    public void OnBeforeSerialize()
    {
        // actionKeys.Clear();
        // actionValues.Clear();

        // foreach (var kvp in actions)
        // {
        //     actionKeys.Add(kvp.Key);
        //     actionValues.Add(kvp.Value);
        // }
    }

    public void OnAfterDeserialize()
    {
        actions = new Dictionary<string, string>();

        for (int i = 0; i != Math.Min(actionKeys.Count, actionValues.Count); i++)
            actions.Add(actionKeys[i], actionValues[i]);
    }

    void OnGUI()
    {
        foreach (var kvp in actions)
            GUILayout.Label("Key: " + kvp.Key + " value: " + kvp.Value);
    }
}
