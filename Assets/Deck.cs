using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck : MonoBehaviour
{
    public Stack<Card> cardStack;
    // Start is called before the first frame update

    void Start()
    {
        cardStack = new Stack<Card>();
        for(int i = 0; i < 3; i++) {
            Card card =  Resources.Load<Card>("Cards/Blaster/Energy Cell");
            cardStack.Push(card);
        }
        // for(int i = 0; i < 5; i++) {
        //     Card card =  Resources.Load<Card>("Cards/Blaster/Charge Shot");
        //     cardStack.Push(card);
        // }
        for(int i = 0; i < 5; i++) {
            Card card =  Resources.Load<Card>("Cards/Blaster/Laser Shot");
            cardStack.Push(card);
        }
        for(int i = 0; i < 2; i++) {
            Card card =  Resources.Load<Card>("Cards/Blaster/Double Shot");
            cardStack.Push(card);
        }
        // for(int i = 0; i < 2; i++) {
        //     Card card =  Resources.Load<Card>("Cards/Blaster/Shock Blast");
        //     cardStack.Push(card);
        // }
        // for(int i = 0; i < 1; i++) {
        //     Card card =  Resources.Load<Card>("Cards/Blaster/Homing Missile");
        //     cardStack.Push(card);
        // }
        for(int i = 0; i < 5; i++) {
            Card card =  Resources.Load<Card>("Cards/Force Field");
            cardStack.Push(card);
        }
        // for(int i = 0; i < 2; i++) {
        //     Card card =  Resources.Load<Card>("Cards/Soul Shield");
        //     cardStack.Push(card);
        // }
        Shuffle();
        Debug.Log("you have " + cardStack.Count + " cards in the deck");
    }

    public void Shuffle() {
        List<Card> cardList = cardStack.ToList();

        for (var i = cardList.Count - 1; i > 0; i--)
        {
            var temp = cardList[i];
            var index = UnityEngine.Random.Range(0, i + 1);
            cardList[i] = cardList[index];
            cardList[index] = temp;
        }
        cardStack = new Stack<Card>(cardList);
    }

    public void AddCard(Card card) {
        cardStack.Push(card);
        Debug.Log("added " + card.name + " to deck!");
        Debug.Log(cardStack.Count + " card in the deck");
    }
}
