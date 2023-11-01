using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck : MonoBehaviour
{
    public StartingDeck startingDeck;
    public StackList<Card> cardStack;
    public CardDisplayCanvas cardDisplayCanvas;
    public bool isInitialized = false;
    public List<Card> cards;
    // Start is called before the first frame update

    void Start()
    {
        cardStack = new StackList<Card>();
        cards = cardStack.items;
        foreach(Card card in startingDeck.cardList) {
            Card cardInstance = Instantiate(card);
            cardStack.Push(cardInstance);
            cardDisplayCanvas.AddCard(cardInstance);
        }
        Debug.Log("you have " + cardStack.Count() + " cards in the deck");
        isInitialized = true;
    }

    public void Shuffle() {
        // List<Card> cardList = cardStack;

        for (var i = cardStack.Count() - 1; i > 0; i--)
        {
            var temp = cardStack.items[i];
            var index = UnityEngine.Random.Range(0, i + 1);
            cardStack.items[i] = cardStack.items[index];
            cardStack.items[index] = temp;
        }
        // cardStack = new Stack<Card>(cardList);
    }

    public void AddCard(Card card) {
        cardStack.Push(card);
        cardDisplayCanvas.AddCard(card);
        Debug.Log("added " + card.name + " to deck!");
        Debug.Log(cardStack.Count() + " card in the deck");
    }
}

public class StackList<T>
{
    public List<T> items = new List<T>();

    public void Push(T item)
    {
        items.Add(item);
    }
    public T Pop()
    {
        if (items.Count > 0)
        {
            T temp = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            return temp;
        }
        else
            return default(T);
    }
    public void Remove(int itemAtPosition)
    {
        items.RemoveAt(itemAtPosition);
    }

    public int Count() {
        return items.Count;
    }
}