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
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defendText;
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
        nameText.text = card.name;
        descriptionText.text = card.description + "<br>" + DescriptionParser();
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
                    List<int> multiAttack = card.actions["ATK"].Split(',').Select(int.Parse).ToList();
                    if (multiAttack.Count != 2) {
                        throw new Exception("Invalid ATK attributes! Must be 2 ints comma separated.");
                    }
                    if (multiAttack[1] == 1) {
                        descriptionAdditional += "<br>Deal " + multiAttack[0] + " damage.";
                    }
                    else {
                        descriptionAdditional = "<br>Deal " + multiAttack[0] + " damage " + multiAttack[1] + " times.";
                    }
                    break;
                case "DEF":
                    descriptionAdditional += "<br>Block " + item.Value + ".";
                    break;
                case "STN":
                    descriptionAdditional += "<br>Stun " + item.Value + " turn.";
                    break;
                case "VULN":
                    descriptionAdditional += "<br>Vulnerable " + item.Value + " turn.";
                    break;
                default:
                    break;
            }
        }

        return descriptionAdditional;
    }               

}
