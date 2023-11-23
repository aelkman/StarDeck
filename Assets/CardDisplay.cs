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
    public GameObject hoverTextGO;
    public TextMeshProUGUI hoverText;
    public CardHoverDescription cardHoverDescription;
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
                    if(attackAll[1] != 1) {
                        descriptionAdditional += "<br>Deal " + attackAll[0] + " damage  " + attackAll[1] + " times to ALL enemies";
                    }
                    else {
                        descriptionAdditional += "<br>Deal " + attackAll[0] + " damage to ALL enemies";
                    }
                    break;
                case "ATK_MOD":
                    descriptionAdditional += "<br>Gain +" + item.Value + " attack";
                    break;
                case "BLIND_ALL":
                    descriptionAdditional += "<br>Blind ALL " + item.Value + " turn";
                    hoverText.text += "Blind - 50% chance to miss all attacks<br><br>";
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
                    hoverText.text += "Ice Stacks - When a character gains 3 ice stacks, they are frozen for 1 turn<br><br>";
                    break;
                case "STN":
                    descriptionAdditional += "<br>Stun " + item.Value + " turn";
                    hoverText.text += "Stun - Target cannot act their current turn<br><br>";
                    break;
                case "STN_ALL":
                    descriptionAdditional += "<br>Stun ALL " + item.Value + " turn";
                    break;  
                case "VULN":
                    if(card.isTarget || card.actions.ContainsKey("COUNTER")) {
                        descriptionAdditional += "<br>Target vulnerable " + item.Value + " turn";
                    }
                    else {
                        descriptionAdditional += "<br>Self vulnerable " + item.Value + " turn";
                    }
                    hoverText.text += "Vulnerable - Take " + ((MainManager.Instance.vulnerableModifier - 1) * 100) + "% more damage<br><br>";
                    break;
                case "RELOAD":
                    descriptionAdditional += "<br>Reload";
                    hoverText.text += "Reload - Replenish your ammo to max capacity<br><br>";
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
                    hoverText.text += "Foresight - Look at the next X cards in your deck, discard as many as you want<br><br>";
                    break;
                case "EXPEL":
                    descriptionAdditional += "<br>Expel";
                    hoverText.text += "Expel - When played, remove this card from deck for the rest of battle<br><br>";
                    break;
                case "COUNTER":
                    descriptionAdditional += "<br>Counter";
                    hoverText.text += "Counter - Action text below Counter on this card will be played BEFORE next enemy attack<br><br>";
                    break;
                case "WEAKEN":
                    descriptionAdditional += "<br>Weaken target " + item.Value + " turn";
                    hoverText.text += "Weaken - Deal " + (MainManager.Instance.weakenedModifier * 100) + "% less damage<br><br>";
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
