using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardLayout : MonoBehaviour
{
    public List<GameObject> cardList;
    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        sortCards();
    }

    void Update() {
        // sortCards();
    }

    private void sortCards() {

        for(int i = 0; i < cardList.Count; i++) {
            float alignResult = i / (cardList.Count - 1.0f);
            Vector3 newPosition = cardList[i].transform.localPosition;
            rectTransform = GetComponent<RectTransform>();
            newPosition.x = ((rectTransform.rect.width + this.transform.position.x) * alignResult) - rectTransform.rect.width/2;
            Debug.Log("x position: " + newPosition.x);
            cardList[i].transform.localPosition = newPosition;
        }
    }
}
