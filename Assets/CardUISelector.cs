using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUISelector : MonoBehaviour
{
    public Deck deck;
    private int addedCount = 0;
    public CardDisplay prefab;
    private Object[] cards;
    public int selectionCount = 3;
    public MainManager mainManager;
    // Start is called before the first frame update
    void Start()
    {
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        deck = GameObject.Find("Deck").GetComponent<Deck>();
        cards = Resources.LoadAll("Cards", typeof(Card));
        var cardsListFiltered = new List<Card>();
        foreach(Card card in cards) {
            if (card.category != "Starter") {
                cardsListFiltered.Add(card);
            }
        }
        for (int i = 0; i < selectionCount; i++) {
            CreateCard(GetRandomCard(cardsListFiltered), i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Card GetRandomCard(List<Card> cardList) {
        // Random.seed = System.DateTime.Now.Millisecond;
        // Random.Range with ints is (inclusive, exclusive)
        return (Card)cardList[Random.Range(0, cardList.Count)];
    }

    public void CreateCard(Card card, int i) {
        prefab.card = card;
        CardDisplay cardInstance = Instantiate(prefab, new Vector3(-20f, 30f, 0f), Quaternion.identity, transform.GetChild(0));
        float fract = (float)i/((float)selectionCount - 1f);
        Debug.Log("fract: " + fract);
        cardInstance.transform.localPosition = new Vector3(Mathf.Lerp(-305f, 265f, fract), cardInstance.transform.position.y, cardInstance.transform.position.z);
        cardInstance.transform.localScale = new Vector3(3.0f, 3.0f, 0f);
    }

    public bool AddToDeck(Card card) {
        if (addedCount < 1) {
            var cardInstance = Instantiate(card);
            deck.AddCard(cardInstance);
            addedCount++;
            return true;
        }
        else {
            Debug.Log("you already added a card!");
            return false;
        }
    }
}
