using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCost : MonoBehaviour
{
    public int price;
    public GameObject item;
    public TextMeshProUGUI tmp;
    // Start is called before the first frame update
    void Start()
    {
        if (item.GetComponent<CardDisplay>() != null) {
            var card = item.GetComponent<CardDisplay>();
            if(card.card.rarity == "C") {
                price = 50;
            }
            else if (card.card.rarity == "U") {
                price = 100;
            }
            else if (card.card.rarity == "R") {
                price = 200;
            }
            if(MainManager.Instance.artifacts.Contains("CRED")) {
                price = (int)Math.Round(0.75 * price);
            }
        }
        else if (item.GetComponent<ExtractorScript>() != null) {
            price = 75;
            if(MainManager.Instance.artifacts.Contains("CRED")) {
                price = (int)Math.Round(0.75 * price);
            }
        }
        tmp.text = price.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
