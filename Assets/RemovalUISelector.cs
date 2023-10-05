using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RemovalUISelector : MonoBehaviour
{
    public DeckCopy deckCopy;
    public GameObject selectedCard;
    // Start is called before the first frame update

    void Start() {
    }

    void OnEnable()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectCard(GameObject newCard) {
        if(selectedCard != null) {
            selectedCard.GetComponent<RemovalUIActions>().glowImage.SetActive(false);
            selectedCard.GetComponent<RemovalUIActions>().isSelected = false;
        }
        selectedCard = newCard;
    }

    
    public void ContinueClick() {
    }
}
