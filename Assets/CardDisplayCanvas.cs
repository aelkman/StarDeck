using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplayCanvas : MonoBehaviour
{
    public Deck deck;
    public GameObject cardUIPrefab;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(() => deck.isInitialized);
        foreach(Card card in deck.cardStack.items) {
            GameObject cardInstance = Instantiate(cardUIPrefab, transform);
            cardInstance.GetComponent<CardDisplay>().card = card;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
