using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CardUISelector : MonoBehaviour
{
    public Deck deck;
    public int addedCount = 0;
    public CardDisplay prefab;
    public Object[] cards;
    public List<Card> cardsSelectable;
    public int selectionCount = 3;
    public CanvasGroup canvasGroup;
    public Button healButton;
    // Start is called before the first frame update
    void Start()
    {
        cardsSelectable = new List<Card>();
        if(MainManager.Instance.artifacts.Contains("DEC_FAT")) {
            selectionCount += 1;
        }
        deck = GameObject.Find("Deck").GetComponent<Deck>();
        cards = Resources.LoadAll("Cards", typeof(Card));
        var cardsListFiltered = new List<Card>();
        foreach(Card card in cards) {
            if (!card.isStarter) {
                cardsListFiltered.Add(card);
            }
        }
        for (int i = 0; i < selectionCount; i++) {
            // don't allow duplicate cards
            Card card = GetRandomCard(cardsListFiltered);
            while(cardsSelectable.Contains(card)) {
                card = GetRandomCard(cardsListFiltered);
            }
            cardsSelectable.Add(card);
            CreateCard(card, i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Card GetRandomCard(List<Card> cardList) {
        var commonCards = cardList.Where(c => c.rarity == "C").ToList();
        var uncommonCards = cardList.Where(c => c.rarity == "U").ToList();
        var rareCards = cardList.Where(c => c.rarity == "R").ToList();
        int rarityDiff_C = 0;
        int rarityDiff_U = 0;
        if(MainManager.Instance.artifacts.Contains("GOLDEN_TICKET")) {
            rarityDiff_C = 10;
            rarityDiff_U = 5;
        }

        // for bosses, rewards are all rare
        if(MainManager.Instance.isBossBattle) {
            return (Card)rareCards[Random.Range(0, rareCards.Count)];
        }

        // current odds: 70/25/5
        // golden ticket odds: 60/30/10

        var rarityIndex = Random.Range(0, 99);
        // Debug.Log("random card index: " + rarityIndex);
        if(rarityIndex >= 0 && rarityIndex <= 69 - rarityDiff_C) {
            // common type
            return (Card)commonCards[Random.Range(0, commonCards.Count)];
        }
        else if(rarityIndex >= 70 - rarityDiff_C && rarityIndex <= 94 - rarityDiff_U) {
            // uncommon type
            return (Card)uncommonCards[Random.Range(0, uncommonCards.Count)];
        }
        else if(rarityIndex >= 95 - rarityDiff_U) {
            // rare type
            return (Card)rareCards[Random.Range(0, rareCards.Count)];
        }
        else {
            // Debug.Log("GetRandomCard shouldn't be here!");
            return (Card)cardList[Random.Range(0, cardList.Count)];
        }
    }

    public Card GetRandomRarity(List<Card> cardList, string rarity) {
        var commonCards = cardList.Where(c => c.rarity == "C").ToList();
        var uncommonCards = cardList.Where(c => c.rarity == "U").ToList();
        var rareCards = cardList.Where(c => c.rarity == "R").ToList();

        if(rarity == "C") {
            // common type
            return (Card)commonCards[Random.Range(0, commonCards.Count)];
        }
        else if(rarity == "U") {
            // uncommon type
            return (Card)uncommonCards[Random.Range(0, uncommonCards.Count)];
        }
        else if(rarity == "R") {
            // rare type
            return (Card)rareCards[Random.Range(0, rareCards.Count)];
        }
        else {
            // Debug.Log("GetRandomCardRarity shouldn't be here! Invalid rarity string");
            return (Card)cardList[Random.Range(0, cardList.Count)];
        }
    }

    public void CreateCard(Card card, int i) {
        prefab.card = card;
        CardDisplay cardInstance = Instantiate(prefab, new Vector3(-20f, 30f, 0f), Quaternion.identity, transform.GetChild(0));
        float fract = (float)i/((float)selectionCount - 1f);
        // Debug.Log("fract: " + fract);
        cardInstance.transform.localPosition = new Vector3(Mathf.Lerp(-305f, 265f, fract), cardInstance.transform.position.y, cardInstance.transform.position.z);
        cardInstance.transform.localScale = new Vector3(3.0f, 3.0f, 0f);
    }

    public bool AddToDeck(Card card) {
        if (addedCount < 1) {
            AudioManager.Instance.PlayCardRustling();
            var cardInstance = Instantiate(card);
            deck.AddCard(cardInstance);
            addedCount++;
            canvasGroup.alpha = 0.5f;
            canvasGroup.blocksRaycasts = false;
            if(healButton != null) {
                healButton.interactable = false;
            }
            return true;
        }
        else {
            // Debug.Log("you already added a card!");
            return false;
        }
    }
}
