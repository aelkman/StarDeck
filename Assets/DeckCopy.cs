using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckCopy : Deck
{
    private Deck deckOriginal;
    // overrides the original deck Start()
    void Start()
    {
        isInitialized = false;
        cardDisplayCanvas = GameObject.Find("CardDisplayCanvas").GetComponent<CardDisplayCanvas>();
        deckOriginal = GameObject.Find("Deck").GetComponent<Deck>();
        cardStack = new StackList<Card>();
        cards = cardStack.items;
        foreach (Card card in deckOriginal.cardStack.items) {
            var newInstance = Instantiate(card);
            cardStack.Push(newInstance);
        }
        Shuffle();
        isInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public new void AddCard(Card card) {
        cardStack.Push(card);
        // cardDisplayCanvas.AddCard(card);
        Debug.Log("added " + card.name + " to deckCopy!");
        Debug.Log(cardStack.Count() + " card in the deckCopy");
    }

    // public void ResetDeckCopy() {
    //     deckOriginal = GameObject.Find("Deck").GetComponent<Deck>();
    //     cardStack = new StackList<Card>();
    //     foreach (Card card in deckOriginal.cardStack.items) {
    //         cardStack.Push(card);
    //     }
    //     Shuffle();
    // }

    public void DiscardCard(Card card) {

    }
}
