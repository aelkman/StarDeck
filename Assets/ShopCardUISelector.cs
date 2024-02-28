using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCardUISelector : CardUISelector
{
    public CoinCount coinCount;
    public bool areSpecificCards = false;
    public List<Card> specificCards;
    public int cardsPurchased = 0;
    private List<CardDisplay> instantiatedCards;
    public int purchaseLimit = 99;
    public bool limitReached = false;
    public bool isTest = false;

    void Start() {
        instantiatedCards = new List<CardDisplay>();
        if(!isTest) {
            if(MainManager.Instance.artifacts.Contains("GOLDEN_TICKET")) {
                canvasGroup.alpha = 0.5f;
                canvasGroup.blocksRaycasts = false;
            }
            deck = GameObject.Find("Deck").GetComponent<Deck>();
        }
        cards = Resources.LoadAll("Cards", typeof(Card));
        var cardsListFiltered = new List<Card>();
        if(!areSpecificCards) {
            foreach(Card card in cards) {
                if (!card.isStarter) {
                    cardsListFiltered.Add(card);
                }
            }
            CreateCard(GetRandomRarity(cardsListFiltered, "C"), 0);
            CreateCard(GetRandomRarity(cardsListFiltered, "U"), 1);
            CreateCard(GetRandomRarity(cardsListFiltered, "R"), 2);
        }
        else {
            for(int i = 0; i < specificCards.Count; i++) {
                CreateCard(specificCards[i], i);
            }
        }
    }

    public new void CreateCard(Card card, int i) {
        prefab.card = card;
        CardDisplay cardInstance = Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity, transform.GetChild(0));
        float fract = (float)i/((float)selectionCount - 1f);
        // Debug.Log("fract: " + fract);
        cardInstance.transform.localPosition = new Vector3(Mathf.Lerp(-285f, 285f, fract), cardInstance.transform.position.y, cardInstance.transform.position.z);
        cardInstance.transform.localScale = new Vector3(3.0f, 3.0f, 0f);
        instantiatedCards.Add(cardInstance);
    }

    public bool AddToDeck(Card card, int cost) {
        if (MainManager.Instance.coinCount >= cost) {
            MainManager.Instance.coinCount -= cost;
            AudioManager.Instance.PlayPurchase();
            var newCard = Instantiate(card);
            deck.AddCard(newCard);
            cardsPurchased += 1;
            if(purchaseLimit == cardsPurchased) {
                canvasGroup.alpha = 0.5f;
                canvasGroup.blocksRaycasts = false;
                limitReached = true;
            }
            return true;
        }
        else {
            // MainManager.Instance.NotEnoughMoney();
            return false;
        }
    }
}
