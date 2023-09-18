using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckCopy : Deck
{
    private Deck deckOriginal;
    // overrides the original deck Start()
    void Start()
    {
        deckOriginal = GameObject.Find("Deck").GetComponent<Deck>();
        cardStack = new Stack<Card>();
        foreach (Card card in deckOriginal.cardStack) {
            cardStack.Push(card);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
