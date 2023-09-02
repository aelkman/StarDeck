using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardLayout : MonoBehaviour
{
    public CardDisplay prefab;
    public DeckScript deck;
    private List<CardDisplay> cardList;
    private RectTransform rectTransform;
    private float zRot = 1.0f;
    private float yOffset = 15.0f;
    private float xOffset = 200;
    // Start is called before the first frame update
    void Start()
    {
        cardList = new List<CardDisplay>();
    }


    public void DrawCards(int cardCount) {
        for(int i = 0; i < cardCount; i++) {
            Card currentCard = deck.cardStack.Pop();

            prefab.card = currentCard;
            CardDisplay cardInstance = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            SetCardDefaultScalePos(cardInstance);
            cardList.Add(cardInstance);
        }
        Debug.Log(cardList);
        SortCards();
    }

    void Update() {
        // SortCards();
    }

    private void SetCardDefaultScalePos(CardDisplay cardInstance) {
        cardInstance.transform.localPosition = new Vector3(0,0,0);
        cardInstance.transform.SetParent(this.transform);
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
        cardList.Add(cardInstance);
        SortCards();
    }

    // inverse of y = x²(3-2x)
    float InverseSmoothstep( float x )
    {
        return 0.5f-(float)Math.Sin(Math.Asin(1.0f-2.0f*x)/3.0f);
    }

    private void SortCards() {

        for(int i = 0; i < cardList.Count; i++) {
            if (cardList.Count > 1) {
                float alignResult = i / (cardList.Count - 1.0f);
                Debug.Log("alignResult: " + alignResult);
                Vector3 newPosition = cardList[i].transform.localPosition;
                Debug.Log("newPosition: " + newPosition);
                // rectTransform = GetComponent<RectTransform>();
                cardList[i].transform.rotation = Quaternion.identity;
                Debug.Log(Mathf.Lerp((cardList.Count-1) * zRot, (cardList.Count-1) * -zRot, Mathf.SmoothStep(0.0f, 1.0f, alignResult)));
                cardList[i].transform.Rotate(0.0f, 0.0f, Mathf.Lerp((cardList.Count-1) * zRot, (cardList.Count-1) * -zRot, alignResult), Space.Self);
                // newPosition.x = ((rectTransform.rect.width + this.transform.position.x) * alignResult) - rectTransform.rect.width/2;
                newPosition.x = Mathf.Lerp(-xOffset/2 * (cardList.Count-1), xOffset/2 * (cardList.Count-1), alignResult);
                newPosition.y = -Mathf.Abs(Mathf.Lerp((cardList.Count-1) * -yOffset, (cardList.Count-1) * yOffset, InverseSmoothstep(alignResult)));
                // newPosition.z = i;
                Debug.Log("y position: " + newPosition.y);
                cardList[i].transform.localPosition = newPosition;
            }
            else {
                cardList[i].transform.localPosition = new Vector3(0,0,0);
            }
        }
    }
}
