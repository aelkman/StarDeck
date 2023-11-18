using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DeckViewer : MonoBehaviour
{
    public Deck deck;
    private bool mouse_over;
    public GameObject deckViewer;
    public bool isRemoval;
    public bool isCloner;
    public GameObject removalButton;
    public GameObject clonerButton;
    public GameObject cancelButton;
    public GameObject cloneCoins;
    public TextMeshProUGUI cloneCost;
    public RemovalUISelector removalUISelector;
    public bool isRemovalEvent = false;
    int price = 0;
    // Start is called before the first frame update
    void Start()
    {
        deck = GameObject.Find("Deck").GetComponent<Deck>();
        isRemoval = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isRemoval) {
            cloneCoins.SetActive(false);
            clonerButton.SetActive(false);
            removalButton.SetActive(true);
            cancelButton.SetActive(true);

            if(removalUISelector.selectedCard == null) {
                removalButton.GetComponent<Button>().interactable = false;
            }
            else {
                removalButton.GetComponent<Button>().interactable = true;
            }
        }
        else if(isCloner) {
            removalButton.SetActive(false);
            clonerButton.SetActive(true);
            cancelButton.SetActive(true);

            if(removalUISelector.selectedCard == null) {
                cloneCoins.SetActive(false);
                clonerButton.GetComponent<Button>().interactable = false;
            }
            else {
                cloneCoins.SetActive(true);
                var card = removalUISelector.selectedCard.GetComponent<CardDisplay>().card;
                // cloner costs 0.75 * the card's cost in the shop
                price = (int)Math.Round(CardUtils.GetCardPrice(card) * 0.75);
                cloneCost.text = price.ToString();
                if(MainManager.Instance.coinCount >= price) {
                    clonerButton.GetComponent<Button>().interactable = true;
                }
                else {
                    clonerButton.GetComponent<Button>().interactable = false;
                }
            }
        }
        else {
            cloneCoins.SetActive(false);
            clonerButton.SetActive(false);
            removalButton.SetActive(false);
            cancelButton.SetActive(false);
        }
    }

    private void ToggleActive() {
        deckViewer.SetActive(!deckViewer.activeSelf);
        isRemoval = false;
    }

    public void StartRemoval(int price) {
        this.price = price;
        isRemoval = true;
        deckViewer.SetActive(true);
    }

    public void StartCloner() {
        isCloner = true;
        deckViewer.SetActive(true);
    }

    public void CancelButtonClick() {
        isRemoval = false;
        isCloner = false;
        ToggleActive();
    }

    public void RemovalButtonClick() {
        StartCoroutine(RemovalButtonTimed());
    }

    public void DeckIconClick() {
        AudioManager.Instance.PlayButtonPress();
        ToggleActive();
    }

    public IEnumerator RemovalButtonTimed() {
        if(MainManager.Instance.coinCount >= price) {
            if(isRemovalEvent) {
                AudioManager.Instance.PlayCardRustling();
            }
            else {
                AudioManager.Instance.PlayCoins();
            }
            MainManager.Instance.coinCount -= price;
            removalUISelector.selectedCard.GetComponent<RemovalUIActions>().CardPlay();
            deck.cardStack.items.Remove(removalUISelector.selectedCard.GetComponent<CardDisplay>().card);
            yield return new WaitForSeconds(1.0f);
            // ToggleActive();
            if(isRemovalEvent) {
                yield return new WaitForSeconds(0.5f);
                ToggleActive();
                // now, callback to the new card window
                var tradeCardWindow = GameObject.Find("NightMarket").GetComponent<NightMarket>().tradeCardViewer;
                tradeCardWindow.SetActive(true);
            }
        }
        else {
            MainManager.Instance.NotEnoughMoney();
        }
    }

    public void CloneButtonClicked() {
        CloneButton();
    }

    public void CloneButton() {
        if(MainManager.Instance.coinCount >= price) {
            AudioManager.Instance.PlayCoins();
            MainManager.Instance.coinCount -= price;
            // removalUISelector.selectedCard.GetComponent<RemovalUIActions>().CardPlay();
            var clonedCard = Instantiate(removalUISelector.selectedCard.GetComponent<CardDisplay>().card);
            deck.AddCard(clonedCard);
            // yield return new WaitForSeconds(1.0f);
            // ToggleActive();
            // if(isRemovalEvent) {
            //     yield return new WaitForSeconds(0.5f);
            //     ToggleActive();
            //     // now, callback to the new card window
            //     var tradeCardWindow = GameObject.Find("NightMarket").GetComponent<NightMarket>().tradeCardViewer;
            //     tradeCardWindow.SetActive(true);
            // }
        }
        else {
            MainManager.Instance.NotEnoughMoney();
        }
    }


}
