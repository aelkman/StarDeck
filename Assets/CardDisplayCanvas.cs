using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplayCanvas : MonoBehaviour
{
    public Deck deck;
    public GameObject cardUIPrefab;
    public Dictionary<Card, GameObject> cardInstances;
    public GameObject canvasGO;
    public int dictCount;
    // Start is called before the first frame update
    void Start()
    {
        // cardInstances = new Dictionary<Card, GameObject>();
        // yield return new WaitUntil(() => deck.isInitialized);
        // foreach(Card card in deck.cardStack.items) {
        //     GameObject cardInstance = Instantiate(cardUIPrefab, canvasGO.transform);
        //     cardInstance.GetComponent<CardDisplay>().card = card;
        //     cardInstances.Add(card, cardInstance);
        // }
    }

    void Update() {

        if(cardInstances != null) {
            dictCount = cardInstances.Count;
        }
        else {
            dictCount = 0;
        }
    }

    public void RemoveCard(Card card) {
        var removal = cardInstances[card];
        cardInstances.Remove(card);
        Destroy(removal);
        // now remove from deck
    }

    public void AddCard(Card card) {
        if(cardInstances == null) {
            SetupCanvas();
        }
        // check if card is already instantiated, there may be cases where this happens
        if(!cardInstances.ContainsKey(card)) {
            GameObject cardInstance = Instantiate(cardUIPrefab, canvasGO.transform);
            // Instantiate the card here so it's unique
            Card cardCopy = Instantiate(card);
            cardInstance.GetComponent<CardDisplay>().card = cardCopy;
            cardInstances.Add(cardCopy, cardInstance);
        }
    }

    private void SetupCanvas() {
        cardInstances = new Dictionary<Card, GameObject>();
        foreach(Card card in deck.cardStack.items) {
            GameObject cardInstance = Instantiate(cardUIPrefab, canvasGO.transform);
            cardInstance.GetComponent<CardDisplay>().card = card;
            cardInstances.Add(card, cardInstance);
        }
    }
}
