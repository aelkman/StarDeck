using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHoverDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public CardDisplay cardDisplay;
    public GameObject cardMiniPreview;
    private bool mouseOver = false;
    public bool pointerDown = false;
    // Start is called before the first frame update
    void Start()
    {
        cardDisplay.hoverTextGO.SetActive(false);
        cardMiniPreview.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(mouseOver && GameManager.Instance.cardHoverDetails) {
            if(pointerDown) {
                cardDisplay.hoverTextGO.SetActive(false);
                cardMiniPreview.SetActive(false);
            }
            else {
                if(cardDisplay.hoverText.text.Length > 0) {
                    cardDisplay.hoverTextGO.SetActive(true);
                }
                if(cardDisplay.previewCard != null) {
                    cardMiniPreview.SetActive(true);
                }
            }
        }
        else {
            cardDisplay.hoverTextGO.SetActive(false);
            cardMiniPreview.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        mouseOver = false;
    }

    public void OnPointerDown(PointerEventData pointerEventData) {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData) {
        pointerDown = false;
    }
}
