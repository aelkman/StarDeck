using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public Stack<Card> cardStack;
    // Start is called before the first frame update
    void Start()
    {
        cardStack = new Stack<Card>();
        for(int i = 0; i < 10; i++) {
            Card attackBasic =  Resources.Load<Card>("StartingDeck/Attack");
            cardStack.Push(attackBasic);
        }
        Debug.Log("you have " + cardStack.Count + " cards in the deck");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
