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
    private float zRot = 2.0f;
    private float yOffset = 14.0f;
    private float xOffset = 50;
    // Start is called before the first frame update
    void Start()
    {
        cardList = new List<CardDisplay>();

    }


    public void drawCards(int cardCount) {
        for(int i = 0; i < cardCount; i++) {
            Card currentCard = deck.cardStack.Pop();

            prefab.card = currentCard;
            CardDisplay cardPrefab = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);

            cardList.Add(cardPrefab);
        }
        Debug.Log(cardList);
        sortCards();
    }

    void Update() {
        // sortCards();
    }

    public void addCard() {
        Card currentCard = deck.cardStack.Pop();
        

        // CardDisplay newCard = Instantiate(prefab);
        // newCard.card = currentCard;

        prefab.card = currentCard;
        CardDisplay cardPrefab = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        
        cardPrefab.transform.localPosition = new Vector3(0,0,0);
        cardPrefab.transform.SetParent(this.transform);
                cardPrefab.transform.localScale = new Vector3(2.3879f, 3.462455f, 0f);
        Debug.Log(cardPrefab.transform.parent);
        
        // cardPrefab.card = currentCard;
        Debug.Log(cardPrefab);
        cardList.Add(cardPrefab);
        sortCards();
    }

    private void sortCards() {

        for(int i = 0; i < cardList.Count; i++) {
            if (cardList.Count > 1) {
                float alignResult = i / (cardList.Count - 1.0f);
                Debug.Log("alignResult: " + alignResult);
                Vector3 newPosition = cardList[i].transform.localPosition;
                Debug.Log("newPosition: " + newPosition);
                // rectTransform = GetComponent<RectTransform>();
                Debug.Log(Mathf.Lerp(cardList.Count * zRot, cardList.Count * -zRot, alignResult));
                // cardList[i].transform.Rotate(0.0f, 0.0f, Mathf.Lerp(cardList.Count * zRot, cardList.Count * -zRot, alignResult), Space.Self);
                // newPosition.x = ((rectTransform.rect.width + this.transform.position.x) * alignResult) - rectTransform.rect.width/2;
                newPosition.x = Mathf.Lerp(cardList.Count * -xOffset, cardList.Count * xOffset, alignResult);
                newPosition.y = -Mathf.Abs(Mathf.Lerp(5 * -yOffset, 5 * yOffset, alignResult));
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
