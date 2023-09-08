using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandManager : MonoBehaviour
{
    public CardDisplay prefab;
    public GameObject animatorPrefab;
    public DeckScript deck;
    private List<CardDisplay> handCards;
    private List<CardDisplay> discardCards;
    private CardDisplay currentCard;
    private RectTransform rectTransform;
    private float zRot = 1.0f;
    private float yOffset = 15.0f;
    private float xOffset = 200;
    // Start is called before the first frame update
    void Start()
    {
        handCards = new List<CardDisplay>();
        discardCards = new List<CardDisplay>();
    }


    public void DrawCards(int cardCount) {
        for(int i = 0; i < cardCount; i++) {
            if(deck.cardStack.Count < 1) {
                for (int j = 0; j < discardCards.Count; j = 0) {
                    CardDisplay cardDisplay = discardCards[j];
                    deck.cardStack.Push(cardDisplay.card);
                    discardCards.Remove(cardDisplay);
                }
                deck.Shuffle();
            }

            Card currentCard = deck.cardStack.Pop();

            prefab.card = currentCard;
            // GameObject animatorInstance = Instantiate(animatorPrefab, transform);
            // animatorInstance.transform.GetChild(0).GetComponent<CardDisplay>().card = currentCard;
            // CardDisplay cardInstance = animatorInstance.transform.GetChild(0).GetComponent<CardDisplay>();
            CardDisplay cardInstance = Instantiate(prefab, transform);
            SetCardDefaultScalePos(cardInstance);
            handCards.Add(cardInstance);
        }
        SortCards();
    }

    void Update() {
        // SortCards();
    }

    private void SetCardDefaultScalePos(CardDisplay cardInstance) {
        cardInstance.transform.localPosition = new Vector3(0,0,0);
        // cardInstance.transform.SetParent(this.transform);
        cardInstance.transform.localScale = new Vector3(2.3879f, 3.462455f, 0f);
    }

    public void AddCard() {
        Card currentCard = deck.cardStack.Pop();
        

        // CardDisplay newCard = Instantiate(prefab);
        // newCard.card = currentCard;

        prefab.card = currentCard;
        CardDisplay cardInstance = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        SetCardDefaultScalePos(cardInstance);
        Debug.Log(cardInstance.transform.parent);
        
        // cardPrefab.card = currentCard;
        Debug.Log(cardInstance);
        handCards.Add(cardInstance);
        SortCards();
    }

    public List<CardDisplay> GetDiscards() {
        return discardCards;
    }

    // inverse of y = x²(3-2x)
    float InverseSmoothstep( float x )
    {
        return 0.5f-(float)Math.Sin(Math.Asin(1.0f-2.0f*x)/3.0f);
    }

    public void PlayCard(CardDisplay cardDisplay) {
        handCards.Remove(cardDisplay);
        discardCards.Add(cardDisplay);
        // next, need to remove GO from the Hand
        cardDisplay.gameObject.SetActive(false);
        Destroy(cardDisplay.GetComponent<CardMouseActions>().GetCursorFollowerInstance());
        Destroy(cardDisplay.gameObject);
        SortCards();
    }

    public void DiscardHand() {
        for(int i=0; i< handCards.Count; i=0) {
            CardDisplay cardDisplay = handCards[i];
            handCards.Remove(cardDisplay);
            discardCards.Add(cardDisplay);
            cardDisplay.gameObject.SetActive(false);
            Destroy(cardDisplay.GetComponent<CardMouseActions>().GetCursorFollowerInstance());
            Destroy(cardDisplay.gameObject);
        }
    }

    private void SortCards() {

        for(int i = 0; i < handCards.Count; i++) {
            if (handCards.Count > 1) {
                float alignResult = i / (handCards.Count - 1.0f);
                // Debug.Log("alignResult: " + alignResult);
                Vector3 newPosition = handCards[i].transform.localPosition;
                // Debug.Log("newPosition: " + newPosition);
                // rectTransform = GetComponent<RectTransform>();
                handCards[i].transform.rotation = Quaternion.identity;
                // Debug.Log(Mathf.Lerp((handCards.Count-1) * zRot, (handCards.Count-1) * -zRot, Mathf.SmoothStep(0.0f, 1.0f, alignResult)));
                handCards[i].transform.Rotate(0.0f, 0.0f, Mathf.Lerp((handCards.Count-1) * zRot, (handCards.Count-1) * -zRot, alignResult), Space.Self);
                // newPosition.x = ((rectTransform.rect.width + this.transform.position.x) * alignResult) - rectTransform.rect.width/2;
                newPosition.x = Mathf.Lerp(-xOffset/2 * (handCards.Count-1), xOffset/2 * (handCards.Count-1), alignResult);
                newPosition.y = -Mathf.Abs(Mathf.Lerp((handCards.Count-1) * -yOffset, (handCards.Count-1) * yOffset, InverseSmoothstep(alignResult)));
                // newPosition.z = i;
                // Debug.Log("y position: " + newPosition.y);
                handCards[i].transform.localPosition = newPosition;
            }
            else {
                handCards[i].transform.localPosition = new Vector3(0,0,0);
            }
        }
    }
}
