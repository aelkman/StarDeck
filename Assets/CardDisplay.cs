using System.Collections;
using System.Collections.Generic;
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
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defendText;
    // Start is called before the first frame update
    void Start()
    {
        nameText.text = card.name;
        descriptionText.text = card.description;
        artworkImage.sprite = card.artwork;
        manaText.text = card.manaCost.ToString();
        if (card.actions.ContainsKey("ATK")) {
            attackText.text = card.actions["ATK"];
        }
        else {
            attackText.text = "";
        }
        if (card.actions.ContainsKey("DEF")) {
            defendText.text = card.actions["DEF"];
        }
        else {
            defendText.text = "";
        }
    }

    public void BaseToBack() {
        var sprite  = Resources.Load<Sprite>("Card Back");
        descriptionText.gameObject.SetActive(false);
        manaText.gameObject.SetActive(false);
        nameText.gameObject.SetActive(false);
        artworkImage.gameObject.SetActive(false);
        cardBase.sprite = sprite;
    }
}
