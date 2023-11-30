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
    public List<string> onDrawKeys;
    public List<string> onDrawValues;
    public Dictionary<string,string> actions;
    public Dictionary<string,string> onDrawActions;
    public string description;
    public string flavorText;
    public string type;
    public string subType;
    public bool isTarget;
    public bool isAttack;
    public bool isPower;
    public bool isSkill;
    public bool isStarter;
    public bool isFinalBlow;
    public string rarity;
    public BaseCharacterInfo target;

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
        onDrawActions = new Dictionary<string, string>();

        for (int i = 0; i != Math.Min(actionKeys.Count, actionValues.Count); i++) {
            actions.Add(actionKeys[i], actionValues[i]);
        }
        for (int i = 0; i != Math.Min(onDrawKeys.Count, onDrawValues.Count); i++) {
            onDrawActions.Add(onDrawKeys[i], onDrawValues[i]);
        }
    }

    void OnGUI()
    {
        foreach (var kvp in actions)
            GUILayout.Label("Key: " + kvp.Key + " value: " + kvp.Value);
    }
}
