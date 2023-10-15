using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckViewer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Deck deck;
    private bool mouse_over;
    public GameObject deckViewer;
    public bool isRemoval;
    public GameObject removalButton;
    public GameObject cancelButton;
    public RemovalUISelector removalUISelector;
    public ShopAudio shopAudio;
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
        if (mouse_over) {
            if (Input.GetMouseButtonUp(0)) {
                ToggleActive();
            }
        }
        if(isRemoval) {
            shopAudio = GameObject.Find("ShopAudio").GetComponent<ShopAudio>();
            removalButton.SetActive(true);
            cancelButton.SetActive(true);
        }
        else {
            removalButton.SetActive(false);
            cancelButton.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        mouse_over = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        mouse_over = false;
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

    public void CancelButtonClick() {
        ToggleActive();
    }

    public void RemovalButtonClick() {
        StartCoroutine(RemovalButtonTimed());
    }

    public IEnumerator RemovalButtonTimed() {
        if(MainManager.Instance.coinCount >= price) {
            shopAudio.PlayPurchaseAudio();
            MainManager.Instance.coinCount -= price;
            removalUISelector.selectedCard.GetComponent<RemovalUIActions>().CardPlay();
            yield return new WaitForSeconds(1.0f);
            deck.cardStack.items.Remove(removalUISelector.selectedCard.GetComponent<CardDisplay>().card);
            // ToggleActive();
        }
    }


}
