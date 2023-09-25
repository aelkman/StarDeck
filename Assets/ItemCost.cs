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
                price = 40;
            }
            else if (card.card.rarity == "U") {
                price = 80;
            }
            else if (card.card.rarity == "R") {
                price = 160;
            }
        }
        tmp.text = price.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
