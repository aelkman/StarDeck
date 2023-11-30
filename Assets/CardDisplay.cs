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
    public Card previewCard;
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

        descriptionText.alignment = TextAlignmentOptions.Center;
        descriptionText.text = "";

        if(card.flavorText != null && card.flavorText.Length > 1) {
            descriptionText.text = "<i>" + card.flavorText + "</i>";
            descriptionText.text += "<line-height=140%><br></line-height>";
        }

        if(card.description.Length > 1) {
            descriptionText.text += card.description + "<br>";
        }
        descriptionText.text += DescriptionParser(card);
        artworkImage.sprite = card.artwork;
        manaText.text = card.manaCost.ToString();
    }

    public void UpdateCard() {
        typeImage.enabled = true;
        hoverText.text = "";
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

        descriptionText.alignment = TextAlignmentOptions.Center;
        descriptionText.text = "";

        if(card.flavorText != null && card.flavorText.Length > 1) {
            descriptionText.text = "<i>" + card.flavorText + "</i>";
            descriptionText.text += "<line-height=140%><br></line-height>";
        }

        if(card.description.Length > 1) {
            descriptionText.text += card.description + "<br>";
        }
        descriptionText.text += DescriptionParser(card);
        artworkImage.sprite = card.artwork;
        manaText.text = card.manaCost.ToString();
    }

    public void BaseToBack() {
        var sprite  = Resources.Load<Sprite>("Card Back");
        descriptionText.gameObject.SetActive(false);
        manaText.gameObject.SetActive(false);
        nameText.gameObject.SetActive(false);
        artworkImage.gameObject.SetActive(false);
        cardBase.sprite = sprite;
    }

    private string DescriptionParser(Card card) {
        string descriptionAdditional = "";
        foreach(var item in card.actions) {
            switch(item.Key) {
                case "ATK":
                    if(item.Value != "DEF") {
                        int attack;
                        string multi;
                        List<string> multiAttack = card.actions["ATK"].Split(',').ToList();
                        attack = Int32.Parse(multiAttack[0]);
                        multi = multiAttack[1].ToString();
                        // }

                        if (multi == "1") {
                            descriptionAdditional += "Deal " + attack + " damage";
                        }
                        else {
                            descriptionAdditional = "Deal " + attack + " damage " + multi + " times";
                        }

                        if(card.isFinalBlow) {
                            Card newCard = ScriptableObject.CreateInstance<Card>();
                            newCard.type = card.type;
                            newCard.manaCost = 0;
                            newCard.actions = new Dictionary<string, string>();
                            newCard.actions.Add(multiAttack[2], multiAttack[3]);
                            // recursion
                            
                            descriptionAdditional += "<br>Final Blow: " + DescriptionParser(newCard);
                            hoverText.text += "Final Blow - If you defeat an enemy with this attack, perform the following action<br><br>";
                        }
                    }
                    break;
                case "ATK_ALL":
                    List<int> attackAll = card.actions["ATK_ALL"].Split(',').Select(int.Parse).ToList();
                    if (attackAll.Count != 2) {
                        throw new Exception("Invalid ATK_ALL attributes! Must be 2 ints comma separated");
                    }
                    if(attackAll[1] != 1) {
                        descriptionAdditional += "Deal " + attackAll[0] + " damage  " + attackAll[1] + " times to ALL enemies";
                    }
                    else {
                        descriptionAdditional += "Deal " + attackAll[0] + " damage to ALL enemies";
                    }
                    break;
                case "ATK_MOD":
                    descriptionAdditional += "Gain +" + item.Value + " attack";
                    break;
                case "BLIND_ALL":
                    descriptionAdditional += "Blind ALL " + item.Value + " turn";
                    hoverText.text += "Blind - 50% chance to miss all attacks<br><br>";
                    break;
                case "CLEAR_DEBUFF":
                    descriptionAdditional += "Clear all debuffs";
                    break;
                case "DEF":
                    if(item.Value == "2X") {
                        descriptionAdditional += "Double your block";
                    }
                    else {
                        descriptionAdditional += "Block " + item.Value + "";
                    }
                    break;
                case "ICE_STACK":
                    descriptionAdditional += "Apply " + item.Value + " ice stacks";
                    hoverText.text += "Ice Stacks - When a character gains 3 ice stacks, they are frozen for 1 turn<br><br>";
                    break;
                case "STN":
                    descriptionAdditional += "Stun " + item.Value + " turn";
                    hoverText.text += "Stun - Target cannot act their current turn<br><br>";
                    break;
                case "STN_ALL":
                    descriptionAdditional += "Stun ALL " + item.Value + " turn";
                    break;  
                case "VULN":
                    if(card.isTarget || card.actions.ContainsKey("COUNTER")) {
                        descriptionAdditional += "Target vulnerable " + item.Value + " turn";
                    }
                    else {
                        descriptionAdditional += "Self vulnerable " + item.Value + " turn";
                    }
                    hoverText.text += "Vulnerable - Take " + ((MainManager.Instance.vulnerableModifier - 1) * 100) + "% more damage<br><br>";
                    break;
                case "RELOAD":
                    descriptionAdditional += "Reload";
                    hoverText.text += "Reload - Replenish your ammo to max capacity<br><br>";
                    break;
                case "DRAW":
                    string cardText = " card";
                    if(Int32.Parse(item.Value) > 1) {
                        cardText += "s";
                    }
                    descriptionAdditional += "Draw " + item.Value + cardText;
                    break;
                case "SCRY":
                    descriptionAdditional += "Foresight " + item.Value;
                    hoverText.text += "Foresight - Look at the next X cards in your deck, discard as many as you want<br><br>";
                    break;
                case "EXPEL":
                    descriptionAdditional += "Expel";
                    hoverText.text += "Expel - When played, remove this card from deck for the rest of battle<br><br>";
                    break;
                case "CARD_PREVIEW":
                    Card previewName = Resources.Load<Card>(item.Value);
                    previewCard = previewName;
                    cardHoverDescription.cardMiniPreview.GetComponent<CardDisplay>().card = previewCard;
                    break;
                case "COUNTER":
                    descriptionAdditional += "Counter";
                    hoverText.text += "Counter - Action text below Counter on this card will be played BEFORE next enemy attack<br><br>";
                    break;
                case "DRAW_SPECIFIC":
                    // drawInfo[0]: resource location
                    // drawInfo[1]: number to draw
                    var drawInfo = item.Value.Split(',').ToList();
                    Card drawName = Resources.Load<Card>(drawInfo[0]);
                    previewCard = drawName;
                    cardHoverDescription.cardMiniPreview.GetComponent<CardDisplay>().card = previewCard;
                    descriptionAdditional += "When played, draw " + drawInfo[1] + " " + drawName.name;
                    break;
                case "WEAKEN":
                    descriptionAdditional += "Weaken target " + item.Value + " turn";
                    hoverText.text += "Weaken - Deal " + (MainManager.Instance.weakenedModifier * 100) + "% less damage<br><br>";
                    break;
                case "WEAKEN_ALL":
                    descriptionAdditional += "Weaken ALL " + item.Value + " turn";
                    break;
                case "HEAL":
                    descriptionAdditional += "Heal " + item.Value;
                    break;
                case "BLAST_MULT":
                    descriptionAdditional += item.Value + "X damage on next Blaster attack";
                    break;
                case "BLAST_ADD":
                    descriptionAdditional += "Blaster attacks do +" + item.Value + " damage this turn";
                    break;
                default:
                    break;
            }
            if(item.Key != "POWER" && item.Key != "ATK_MOD_TEMP" && item.Key != "CAST") {
                descriptionAdditional += "<br>";
            }
        }

        return descriptionAdditional;
    }               

}
