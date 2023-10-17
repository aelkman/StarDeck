using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PotionShopButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // private Button button;
    private Image image;
    public Potion potion;
    public GameObject hoverTextGO;
    public TextMeshProUGUI hoverDescription;
    public TextMeshProUGUI priceText;
    public PotionUI potionUI;
    public int price;
    public CanvasGroup canvasGroup;
    public ShopAudio shopAudio;
    public bool mouse_over = false;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        potionUI = GameObject.Find("PotionUI").GetComponent<PotionUI>();
        if(potion != null) {
            SetPotion(potion);
            price = potion.price;
            if(MainManager.Instance.artifacts.Contains("CRED")) {
                price = (int)Math.Round(0.75 * price);
            }
            priceText.text = potion.price.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mouse_over) {
            if (Input.GetMouseButtonUp(0)) {
                PurchasePotion();
            }
        }
    }

    public void SetPotion(Potion potion) {
        this.potion = potion;
        image = GetComponent<Image>();
        image.sprite = potion.spriteStore;
        // button = GetComponent<Button>();
        // SpriteState ss = new SpriteState();
        // ss.highlightedSprite = potion.spriteStoreSelected;
        // // ss.disabledSprite = potion.spriteStoreDisabled;
        // // ss.selectedSprite = potion.spriteStoreSelected;
        // // ss.pressedSprite = potion.spriteStoreSelected;
        // button.spriteState = ss;
    }

    public void RemovePotion() {
        this.potion = null;
        image.sprite = null;
        // SpriteState ss = new SpriteState();
        // ss.highlightedSprite = null;
        // // ss.disabledSprite = null;
        // // ss.selectedSprite = null;
        // // ss.pressedSprite = null;
        // button.spriteState = ss;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        mouse_over = true;
        image.sprite = potion.spriteStoreSelected;
        hoverTextGO.SetActive(true);
        hoverDescription.text = potion.name + " - " + potion.description;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        mouse_over = false;
        image.sprite = potion.spriteStore;
        hoverTextGO.SetActive(false);
    }

    public void PurchasePotion() {
        if(MainManager.Instance.potions.Count <= 2 && MainManager.Instance.coinCount >= potion.price) {
            shopAudio.PlayPurchaseAudio();
            canvasGroup.alpha = 0.3f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            MainManager.Instance.potions.Add(potion);
            potionUI.EquipPotion(potion);
            MainManager.Instance.coinCount -= price;
        }
    }
}
