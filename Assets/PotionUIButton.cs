using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PotionUIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // private Button button;
    private Image image;
    public Potion potion;
    public GameObject hoverTextGO;
    public GameObject clickOptionsGO;
    public TextMeshProUGUI hoverDescription;
    private bool mouse_over = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnEnable() {
        image = GetComponent<Image>();
        // button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mouse_over) {
            if (Input.GetMouseButtonUp(0)) {
                hoverTextGO.SetActive(false);
                clickOptionsGO.SetActive(true);
                transform.SetSiblingIndex(4);
            }
        }
    }

    public void SetPotion(Potion potion) {
        this.potion = potion;
        image.sprite = potion.spriteUI;
        // SpriteState ss = new SpriteState();
        // ss.highlightedSprite = potion.spriteUISelected;
        // ss.selectedSprite = potion.spriteUISelected;
        // ss.pressedSprite = potion.spriteUISelected;
        // button.spriteState = ss;
    }

    public void RemovePotion() {
        this.potion = null;
        if(image != null) {
            image.sprite = null;
        }
        // if(button != null) {
        //     SpriteState ss = new SpriteState();
        //     ss.highlightedSprite = null;
        //     ss.selectedSprite = null;
        //     ss.pressedSprite = null;
        //     button.spriteState = ss;
        // }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        mouse_over = true;
        hoverTextGO.SetActive(true);
        image.sprite = potion.spriteUISelected;
        hoverDescription.text = potion.name + " - " + potion.description;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        mouse_over = false;
        image.sprite = potion.spriteUI;
        hoverTextGO.SetActive(false);
    }

    public void ClickButton() {

    }

    public void UsePotion() {

    }
}
