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
        deckOriginal = GameObject.Find("Deck").GetComponent<Deck>();
        cardStack = new StackList<Card>();
        foreach (Card card in deckOriginal.cardStack.items) {
            cardStack.Push(card);
        }
        Shuffle();
        isInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DiscardCard(Card card) {

    }
}
