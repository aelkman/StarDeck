using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCardUISelector : CardUISelector
{
    public CoinCount coinCount;

    void Start() {
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        deck = GameObject.Find("Deck").GetComponent<Deck>();
        cards = Resources.LoadAll("Cards", typeof(Card));
        var cardsListFiltered = new List<Card>();
        foreach(Card card in cards) {
            if (card.category != "Starter") {
                cardsListFiltered.Add(card);
            }
        }
        CreateCard(GetRandomRarity(cardsListFiltered, "C"), 0);
        CreateCard(GetRandomRarity(cardsListFiltered, "U"), 1);
        CreateCard(GetRandomRarity(cardsListFiltered, "R"), 2);
    }
    public new void CreateCard(Card card, int i) {
        prefab.card = card;
        CardDisplay cardInstance = Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity, transform.GetChild(0));
        float fract = (float)i/((float)selectionCount - 1f);
        Debug.Log("fract: " + fract);
        cardInstance.transform.localPosition = new Vector3(Mathf.Lerp(-285f, 285f, fract), cardInstance.transform.position.y, cardInstance.transform.position.z);
        cardInstance.transform.localScale = new Vector3(3.0f, 3.0f, 0f);
    }

    public bool AddToDeck(Card card, int cost) {
        if (mainManager.coinCount >= cost) {
            deck.AddCard(card);
            coinCount.SpendCoins(cost);
            return true;
        }
        else {
            return false;
        }
    }
}
