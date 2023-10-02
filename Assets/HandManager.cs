using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandManager : MonoBehaviour
{
    public CardDisplay prefab;
    public GameObject drawingDeck;
    public GameObject animatorPrefab;
    public DeckCopy deckCopy;
    public List<CardDisplay> handCards;
    private List<CardDisplay> discardCards;
    private List<CardDisplay> expelCards;
    private CardDisplay currentCard;
    private RectTransform rectTransform;
    private float zRot = 1.0f;
    private float yOffset = 15.0f;
    private float xOffset = 200;
    public PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        expelCards = new List<CardDisplay>();
        handCards = new List<CardDisplay>();
        discardCards = new List<CardDisplay>();
    }

    public void DrawCards(int cardCount) {
        StartCoroutine(DrawCardsTimed(cardCount, cardsReturnValue => {}));
    }

    public IEnumerator DrawCardsTimed(int cardCount, System.Action<List<Card>> cardsCallback) {
        List<Card> cards = new List<Card>();
        for(int i = 0; i < cardCount; i++) {
            if(deckCopy.cardStack.Count < 1) {
                for (int j = 0; j < discardCards.Count; j = 0) {
                    CardDisplay cardDisplay = discardCards[j];
                    deckCopy.cardStack.Push(cardDisplay.card);
                    discardCards.Remove(cardDisplay);
                }
                deckCopy.Shuffle();
            }

            Card currentCard = deckCopy.cardStack.Pop();
            cards.Add(currentCard);

            prefab.card = currentCard;
            // GameObject animatorInstance = Instantiate(animatorPrefab, transform);
            // animatorInstance.transform.GetChild(0).GetComponent<CardDisplay>().card = currentCard;
            // CardDisplay cardInstance = animatorInstance.transform.GetChild(0).GetComponent<CardDisplay>();
            CardDisplay cardInstance = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
            SetCardDefaultScalePos(cardInstance);
            handCards.Add(cardInstance);
            SortCards();
            yield return new WaitForSeconds(0.2f);
        }

        cardsCallback(cards);
    }

    void Update() {
        // SortCards();
    }

    private void SetCardDefaultScalePos(CardDisplay cardInstance) {
        cardInstance.transform.position = drawingDeck.transform.position;
        // cardInstance.transform.localPosition = new Vector3(0,0,0);
        // cardInstance.transform.SetParent(this.transform);
        cardInstance.transform.localScale = new Vector3(3.0f, 3.0f, 0f);
    }

    // public void AddCard() {
    //     Card currentCard = deckCopy.cardStack.Pop();
        

    //     // CardDisplay newCard = Instantiate(prefab);
    //     // newCard.card = currentCard;

    //     prefab.card = currentCard;
    //     CardDisplay cardInstance = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
    //     SetCardDefaultScalePos(cardInstance);
    //     Debug.Log(cardInstance.transform.parent);
        
    //     // cardPrefab.card = currentCard;
    //     Debug.Log(cardInstance);
    //     handCards.Add(cardInstance);
    //     SortCards();
    // }

    public List<CardDisplay> GetDiscards() {
        return discardCards;
    }

    // inverse of y = x²(3-2x)
    float InverseSmoothstep( float x )
    {
        return 0.5f-(float)Math.Sin(Math.Asin(1.0f-2.0f*x)/3.0f);
    }

    public void PlayCard(CardDisplay cardDisplay) {
        // remove from hand, then sort
        handCards.Remove(cardDisplay);
        SortCards();
        // defer deletion & removal by 1.7s
        StartCoroutine(DeferCardDeletion(cardDisplay, 1.7f));
    }

    private IEnumerator DeferCardDeletion(CardDisplay cardDisplay, float time) {
        yield return new WaitForSeconds(time);
        if(cardDisplay.card.name == "Virus") {
            expelCards.Add(cardDisplay);
        }
        else {
            discardCards.Add(cardDisplay);
        }
        // next, need to remove GO from the Hand
        cardDisplay.gameObject.SetActive(false);
        Destroy(cardDisplay.GetComponent<CardMouseActions>().GetCursorFollowerInstance());
        Destroy(cardDisplay.gameObject);
    }

    private void NegativeDiscards(CardDisplay cardDisplay) {
        if(cardDisplay.card.name == "Virus") {
            // duplicate
            discardCards.Add(cardDisplay);
            // deal 5 damage to player
            playerStats.takeDamage(5);
        }
    }

    public void DiscardHand() {
        for(int i=0; i< handCards.Count; i=0) {
            CardDisplay cardDisplay = handCards[i];
            // negative cards effects for discard here
            NegativeDiscards(cardDisplay);
            handCards.Remove(cardDisplay);
            discardCards.Add(cardDisplay);
            cardDisplay.gameObject.SetActive(false);
            DestroyImmediate(cardDisplay.GetComponent<CardMouseActions>().GetCursorFollowerInstance());
            DestroyImmediate(cardDisplay.gameObject);
        }
    }

    private void SortCards() {

        for(int i = 0; i < handCards.Count; i++) {
            // if (handCards.Count > 1) {
                float alignResult = i / (handCards.Count - 1.0f);

                // if (handCards[i].transform.localPosition == null || handCards[i].transform.rotation == null) {

                //     // Debug.Log("alignResult: " + alignResult);
                //     Vector3 newPosition = handCards[i].transform.localPosition;
                //     // Debug.Log("newPosition: " + newPosition);
                //     // rectTransform = GetComponent<RectTransform>();
                //     handCards[i].transform.rotation = Quaternion.identity;
                //     // Debug.Log(Mathf.Lerp((handCards.Count-1) * zRot, (handCards.Count-1) * -zRot, Mathf.SmoothStep(0.0f, 1.0f, alignResult)));
                //     handCards[i].transform.Rotate(0.0f, 0.0f, Mathf.Lerp((handCards.Count-1) * zRot, (handCards.Count-1) * -zRot, alignResult), Space.Self);
                //     // newPosition.x = ((rectTransform.rect.width + this.transform.position.x) * alignResult) - rectTransform.rect.width/2;
                //     newPosition.x = Mathf.Lerp(-xOffset/2 * (handCards.Count-1), xOffset/2 * (handCards.Count-1), alignResult);
                //     newPosition.y = -Mathf.Abs(Mathf.Lerp((handCards.Count-1) * -yOffset, (handCards.Count-1) * yOffset, InverseSmoothstep(alignResult)));
                //     // newPosition.z = i;
                //     // Debug.Log("y position: " + newPosition.y);
                //     handCards[i].transform.localPosition = newPosition;
                // }
                // else {
                    StartCoroutine(MoveCard(handCards[i], alignResult, 0.1f));
                // }
            // }
            // else {
            //     handCards[i].transform.localPosition = new Vector3(0,0,0);
            // }
        }
    }

    private IEnumerator MoveCard(CardDisplay cardDisplay, float alignResult, float timeInterval) {

        int handCardsLess1 = handCards.Count - 1;
        if(handCards.Count == 1) {
            alignResult = 1;
            handCardsLess1 = 2;
        }
        for (float i = 0f; i <= 1f; i+= timeInterval) {

            Vector3 originalPosition = cardDisplay.transform.localPosition;
            float newXPos = Mathf.Lerp(-xOffset/2 * (handCardsLess1), xOffset/2 * (handCardsLess1), alignResult);
            float newYPos = -Mathf.Abs(Mathf.Lerp((handCardsLess1) * -yOffset, (handCardsLess1) * yOffset, InverseSmoothstep(alignResult)));

            cardDisplay.transform.localPosition = new Vector3(
                (Mathf.Lerp(originalPosition.x, newXPos, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(originalPosition.y, newYPos, Mathf.SmoothStep(0f, 1f, i))),
                0
            );

            float newRotationZ = Mathf.Lerp((handCardsLess1) * zRot, (handCardsLess1) * -zRot, alignResult);
            Quaternion originalRotation = cardDisplay.transform.rotation;

            // Debug.Log("newZRot: " + newRotationZ);
            // Debug.Log("originalRot: " + originalRotation.eulerAngles.z);

            Vector3 currentAngle = new Vector3(0f, 0f, Mathf.Lerp(WrapAngle(originalRotation.eulerAngles.z), newRotationZ, Mathf.SmoothStep(0, 1, i)));
            cardDisplay.transform.eulerAngles = currentAngle;

            yield return new WaitForSeconds(0.01f);
        }

        cardDisplay.gameObject.GetComponent<CardMouseActions>().originalPosition = cardDisplay.transform.localPosition;
    }

    private static float WrapAngle(float angle)
    {
        angle%=360;
        if(angle >180)
            return angle - 360;

        return angle;
    }
}
