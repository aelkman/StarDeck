using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class NextActionText : MonoBehaviour
{
    public TextMeshProUGUI nextActionText;
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
                        description = multiAttack[0] + " damage";
                    }
                    else {
                        description = (multiAttack[0]) + "X" + multiAttack[1] + " damage";
                    }
                }
                else if(entry.Key == "ATK_RND") {
                    name = "Attack";
                    var randAttack = entry.Value.Split(',').Select(int.Parse).ToList();
                    description = (randAttack[0]) + "-" + (randAttack[1]) + " damage";
                }
                else if(entry.Key == "DEF_RND") {
                    name = "Block";
                    var randBlock = entry.Value.Split(',').Select(int.Parse).ToList();
                    description = randBlock[0] + "-" + randBlock[1] + " shield";
                }
                else if(entry.Key == "ATK_MOD") {
                    string sign = Int32.Parse(entry.Value) > 0 ? "+" : "";
                    name = "Attack buff";
                    description = sign + entry.Value;
                }
                else if(entry.Key == "REMOVE_DEBUFF") {
                    name = "Remove Debuffs";
                    description = "";
                }
                else if(entry.Key == "ANTI_STUN") {
                    if(name != "") {
                        name += ", ";
                    }
                    name += "Anti-Stun";
                    if(description != "") {
                        description += ", ";
                    }
                    description += "2 turns";
                }
                else if(entry.Key == "BLIND") {
                    name = "Blind";
                    bool isSingle = Int32.Parse(entry.Value) == 1 ? true : false;
                    string turns = isSingle ? " turn" : " turns";
                    description = entry.Value + turns;
                }
                else if(entry.Key == "TAUNT") {
                    name = "Taunt";
                    bool isSingle = Int32.Parse(entry.Value) == 1 ? true : false;
                    string turns = isSingle ? " turn" : " turns";
                    description = entry.Value + turns;
                }
                else if(entry.Key == "WEAKEN") {
                    name = "Weaken";
                    bool isSingle = Int32.Parse(entry.Value) == 1 ? true : false;
                    string turns = isSingle ? " turn" : " turns";
                    description = entry.Value + turns;
                }
                else if(entry.Key == "VULN") {
                    name = "Mark vulnerable";
                    bool isSingle = Int32.Parse(entry.Value) == 1 ? true : false;
                    string turns = isSingle ? " turn" : " turns";
                    description = entry.Value + turns;
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
