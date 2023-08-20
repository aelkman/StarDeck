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
        cardPrefab.transform.parent = this.rectTransform;
        // cardPrefab.card = currentCard;
        Debug.Log(cardPrefab);
        cardList.Add(cardPrefab);
        sortCards();
    }

    private void sortCards() {

        for(int i = 0; i < cardList.Count; i++) {
            float alignResult = i / (cardList.Count - 1.0f);
            Vector3 newPosition = cardList[i].transform.localPosition;
            rectTransform = GetComponent<RectTransform>();
            Debug.Log(Mathf.Lerp(cardList.Count * zRot, cardList.Count * -zRot, alignResult));
            cardList[i].transform.Rotate(0.0f, 0.0f, Mathf.Lerp(cardList.Count * zRot, cardList.Count * -zRot, alignResult), Space.Self);
            // newPosition.x = ((rectTransform.rect.width + this.transform.position.x) * alignResult) - rectTransform.rect.width/2;
            newPosition.x = Mathf.Lerp(cardList.Count * -xOffset, cardList.Count * xOffset, alignResult);
            newPosition.y = -Mathf.Abs(Mathf.Lerp(5 * -yOffset, 5 * yOffset, alignResult));
            // newPosition.z = i;
            Debug.Log("y position: " + newPosition.y);
            cardList[i].transform.localPosition = newPosition;
        }
    }
}
