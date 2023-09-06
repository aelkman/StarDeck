using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeckScript : MonoBehaviour
{
    public Stack<Card> cardStack;
    // Start is called before the first frame update
    void Start()
    {
        cardStack = new Stack<Card>();
        for(int i = 0; i < 10; i++) {
            Card attackBasic =  Resources.Load<Card>("StartingDeck/Laser Pistol/Laser Shot");
            cardStack.Push(attackBasic);
        }
        for(int i = 0; i < 10; i++) {
            Card attackBasic =  Resources.Load<Card>("StartingDeck/Defend");
            cardStack.Push(attackBasic);
        }
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
