using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class CardUtils
{

    public static string DescriptionParser(KeyValuePair<string, string> action) {
        string descriptionAdditional = "";
            switch(action.Key) {
                case "ATK":
                    if(action.Value != "DEF") {
                        int attack;
                        string multi;
                        if(action.Value.Contains("X") || action.Value.Contains("A")) {
                            var multiAttackStrings = action.Value.Split(',').ToList();
                            attack = Int32.Parse(multiAttackStrings[0]);
                            if(multiAttackStrings[1] != "X" && multiAttackStrings[1] != "A") {
                                throw new Exception("Invalid string found in ATK value! " + multiAttackStrings[1]);
                            }
                            else {
                                multi = multiAttackStrings[1];
                            }
                        }
                        else {
                            List<int> multiAttack = action.Value.Split(',').Select(int.Parse).ToList();
                            if (multiAttack.Count != 2) {
                                throw new Exception("Invalid ATK attributes! Must be 2 ints comma separated");
                            }
                            attack = multiAttack[0];
                            multi = multiAttack[1].ToString();
                        }

                        if (multi == "1") {
                            descriptionAdditional += "Deal " + attack + " damage";
                        }
                        else {
                            descriptionAdditional = "Deal " + attack + " damage " + multi + " times";
                        }
                    }
                    break;
                case "ATK_ALL":
                    List<int> attackAll = action.Value.Split(',').Select(int.Parse).ToList();
                    if (attackAll.Count != 2) {
                        throw new Exception("Invalid ATK_ALL attributes! Must be 2 ints comma separated");
                    }
                    descriptionAdditional += "Deal " + attackAll[0] + " damage to ALL enemies";
                    break;
                case "BLIND_ALL":
                    descriptionAdditional += "Blind ALL" + action.Value + " turn";
                    break;
                case "DEF":
                    if(action.Value == "2X") {
                        descriptionAdditional += "Double your block";
                    }
                    else {
                        descriptionAdditional += "Block " + action.Value + "";
                    }
                    break;
                case "ICE_STACK":
                    descriptionAdditional += "Apply " + action.Value + " ice stacks";
                    break;
                case "STN":
                    descriptionAdditional += "Stun " + action.Value + " turn";
                    break;
                case "STN_ALL":
                    descriptionAdditional += "Stun ALL " + action.Value + " turn";
                    break;  
                case "VULN":
                    descriptionAdditional += "Target vulnerable " + action.Value + " turn";
                    break;
                case "RELOAD":
                    descriptionAdditional += "Reload";
                    break;
                case "DRAW":
                    string cardText = " card";
                    if(Int32.Parse(action.Value) > 1) {
                        cardText += "s";
                    }
                    descriptionAdditional += "Draw " + action.Value + cardText;
                    break;
                case "SCRY":
                    descriptionAdditional += "Foresight " + action.Value;
                    break;
                case "EXPEL":
                    descriptionAdditional += "Expel";
                    break;
                case "COUNTER":
                    descriptionAdditional += "Counter";
                    break;
                case "WEAKEN":
                    descriptionAdditional += "Weaken target " + action.Value + " turn";
                    break;
                case "WEAKEN_ALL":
                    descriptionAdditional += "Weaken ALL " + action.Value + " turn";
                    break;
                default:
                    break;
            }

        return descriptionAdditional;
    }               

    public static int GetCardPrice(Card card) {
        int price = 0;

        if(card.rarity == "C") {
            price = 50;
        }
        else if(card.rarity == "U") {
            price = 100;
        }
        else if(card.rarity == "R") {
            price = 200;
        }

        if(MainManager.Instance.artifacts.Contains("CRED")) {
            price = (int)Math.Round(0.75 * price);
        }

        return price;
    }

}
