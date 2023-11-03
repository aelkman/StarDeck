using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image artworkImage;
    public Image cardBase;
    public Image rarityImage;
    public Image glowImage;
    public Image typeImage;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defendText;
    public GameObject pointerBoundary;
    // Start is called before the first frame update
    void Start()
    {
        if(card.rarity == "C") {
            cardBase.sprite = Resources.Load<Sprite>("Card_base_blue");
        }
        else if (card.rarity == "U") {
            cardBase.sprite = Resources.Load<Sprite>("Card_base_purple");
        }
        else if (card.rarity == "R") {
            cardBase.sprite = Resources.Load<Sprite>("Card_base_gold_2");
        }
        else if (card.rarity == "N") {
            descriptionText.color = Color.white;
            nameText.color = Color.white;
            manaText.color = Color.white;
            cardBase.sprite = Resources.Load<Sprite>("Card_base_black");
        }
        else if (card.rarity == "E") {
            cardBase.sprite = Resources.Load<Sprite>("Card_base_event");
        }

        if(card.type == "Hammer") {
            typeImage.sprite = Resources.Load<Sprite>("Images/hammer icon");
        }
        else if(card.type == "Blaster") {
            typeImage.sprite = Resources.Load<Sprite>("Images/blaster icon");
        }
        else {
            typeImage.enabled = false;
        }

        nameText.text = card.name;
        if(card.description != "") {
            descriptionText.text = card.description + "<br>";
        }
        else {
            descriptionText.text = "";
        }
        descriptionText.text += DescriptionParser();
        artworkImage.sprite = card.artwork;
        manaText.text = card.manaCost.ToString();
        // if (card.actions.ContainsKey("ATK")) {
        //     attackText.text = card.actions["ATK"];
        // }
        // else {
        //     attackText.text = "";
        // }
        // if (card.actions.ContainsKey("DEF")) {
        //     defendText.text = card.actions["DEF"];
        // }
        // else {
        //     defendText.text = "";
        // }
    }

    public void BaseToBack() {
        var sprite  = Resources.Load<Sprite>("Card Back");
        descriptionText.gameObject.SetActive(false);
        manaText.gameObject.SetActive(false);
        nameText.gameObject.SetActive(false);
        artworkImage.gameObject.SetActive(false);
        cardBase.sprite = sprite;
    }

    private string DescriptionParser() {
        string descriptionAdditional = "";
        foreach(var item in card.actions) {
            switch(item.Key) {
                case "ATK":
                    if(item.Value != "DEF") {
                        int attack;
                        string multi;
                        if(item.Value.Contains("X") || item.Value.Contains("A")) {
                            var multiAttackStrings = item.Value.Split(',').ToList();
                            attack = Int32.Parse(multiAttackStrings[0]);
                            if(multiAttackStrings[1] != "X" && multiAttackStrings[1] != "A") {
                                throw new Exception("Invalid string found in ATK value! " + multiAttackStrings[1]);
                            }
                            else {
                                multi = multiAttackStrings[1];
                            }
                        }
                        else {
                            List<int> multiAttack = card.actions["ATK"].Split(',').Select(int.Parse).ToList();
                            if (multiAttack.Count != 2) {
                                throw new Exception("Invalid ATK attributes! Must be 2 ints comma separated");
                            }
                            attack = multiAttack[0];
                            multi = multiAttack[1].ToString();
                        }

                        if (multi == "1") {
                            descriptionAdditional += "<br>Deal " + attack + " damage";
                        }
                        else {
                            descriptionAdditional = "<br>Deal " + attack + " damage " + multi + " times";
                        }
                    }
                    break;
                case "ATK_ALL":
                    List<int> attackAll = card.actions["ATK_ALL"].Split(',').Select(int.Parse).ToList();
                    if (attackAll.Count != 2) {
                        throw new Exception("Invalid ATK_ALL attributes! Must be 2 ints comma separated");
                    }
                    descriptionAdditional += "<br>Deal " + attackAll[0] + " damage to ALL enemies";
                    break;
                case "ATK_MOD":
                    descriptionAdditional += "<br>Gain +" + item.Value + " attack";
                    break;
                case "BLIND_ALL":
                    descriptionAdditional += "<br>Blind ALL " + item.Value + " turn";
                    break;
                case "CLEAR_DEBUFF":
                    descriptionAdditional += "<br>Clear all debuffs";
                    break;
                case "DEF":
                    if(item.Value == "2X") {
                        descriptionAdditional += "<br>Double your block";
                    }
                    else {
                        descriptionAdditional += "<br>Block " + item.Value + "";
                    }
                    break;
                case "ICE_STACK":
                    descriptionAdditional += "<br>Apply " + item.Value + " ice stacks";
                    break;
                case "STN":
                    descriptionAdditional += "<br>Stun " + item.Value + " turn";
                    break;
                case "STN_ALL":
                    descriptionAdditional += "<br>Stun ALL " + item.Value + " turn";
                    break;  
                case "VULN":
                    if(card.isTarget) {
                        descriptionAdditional += "<br>Target vulnerable " + item.Value + " turn";
                    }
                    else {
                        descriptionAdditional += "<br>Self vulnerable " + item.Value + " turn";
                    }
                    break;
                case "RELOAD":
                    descriptionAdditional += "<br>Reload";
                    break;
                case "DRAW":
                    string cardText = " card";
                    if(Int32.Parse(item.Value) > 1) {
                        cardText += "s";
                    }
                    descriptionAdditional += "<br>Draw " + item.Value + cardText;
                    break;
                case "SCRY":
                    descriptionAdditional += "<br>Foresight " + item.Value;
                    break;
                case "EXPEL":
                    descriptionAdditional += "<br>Expel";
                    break;
                case "COUNTER":
                    descriptionAdditional += "<br>Counter";
                    break;
                case "WEAKEN":
                    descriptionAdditional += "<br>Weaken target " + item.Value + " turn";
                    break;
                case "WEAKEN_ALL":
                    descriptionAdditional += "<br>Weaken ALL " + item.Value + " turn";
                    break;
                case "HEAL":
                    descriptionAdditional += "<br>Heal " + item.Value;
                    break;
                default:
                    break;
            }
        }

        return descriptionAdditional;
    }               

}
