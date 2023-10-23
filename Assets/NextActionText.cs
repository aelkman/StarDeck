using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class NextActionText : MonoBehaviour
{
    public TextMeshPro nextActionText;
    public Dictionary<string, string> actions;
    public Card card;
    public BattleEnemyContainer battleEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetText(Card card) {
        this.card = card;
        nextActionText.text = CreateText();
    }

    private string CreateText() {
        var actionText = "";

        if(card != null) {
            foreach(KeyValuePair<string, string> entry in card.actions) {
                var description = "";
                var name = "";
                if(entry.Key == "ATK") {
                    name = "Attack";
                    var multiAttack = entry.Value.Split(',').Select(int.Parse).ToList();
                    if(multiAttack[1] == 1) {
                        description = multiAttack[0] + battleEnemy.atkMod + " damage";
                    }
                    else {
                        description = (multiAttack[0] + battleEnemy.atkMod) + "X" + multiAttack[1] + " damage";
                    }
                }
                else if(entry.Key == "ATK_RND") {
                    name = "Attack";
                    var randAttack = entry.Value.Split(',').Select(int.Parse).ToList();
                    description = (randAttack[0] + battleEnemy.atkMod) + "-" + (randAttack[1] + battleEnemy.atkMod) + " damage";
                }
                else if(entry.Key == "DEF_RND") {
                    name = "Block";
                    var randBlock = entry.Value.Split(',').Select(int.Parse).ToList();
                    description = randBlock[0] + "-" + randBlock[1] + " shield";
                }
                else if(entry.Key == "ATK_MOD") {
                    name = "Attack buff";
                    description = entry.Value;
                }
                else {
                    name = entry.Key;
                    description = entry.Value;
                }
                actionText = name + "<br>" + description;
            }
        }

        return actionText;
    }
}
