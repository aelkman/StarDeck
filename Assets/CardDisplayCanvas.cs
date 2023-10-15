using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplayCanvas : MonoBehaviour
{
    public Deck deck;
    public GameObject cardUIPrefab;
    public Dictionary<Card, GameObject> cardInstances;
    public GameObject canvasGO;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        cardInstances = new Dictionary<Card, GameObject>();
        yield return new WaitUntil(() => deck.isInitialized);
        foreach(Card card in deck.cardStack.items) {
            GameObject cardInstance = Instantiate(cardUIPrefab, canvasGO.transform);
            cardInstance.GetComponent<CardDisplay>().card = card;
            cardInstances.Add(card, cardInstance);
        }
    }

    public void RemoveCard(Card card) {
        Destroy(cardInstances[card]);
    }

    public void AddCard(Card card) {
        if(cardInstances == null) {
            cardInstances = new Dictionary<Card, GameObject>();
        }
        GameObject cardInstance = Instantiate(cardUIPrefab, canvasGO.transform);
        // Instantiate the card here so it's unique
        Card cardCopy = Instantiate(card);
        cardInstance.GetComponent<CardDisplay>().card = cardCopy;
        cardInstances.Add(cardCopy, cardInstance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
