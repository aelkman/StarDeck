using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightHoverSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool mouse_over = false;
    public Sprite regular;
    public Sprite highlighted;
    private Image image;
    public Image buttonImage;
    private bool selected = false;
    public WeaponsManager weaponsManager;
    public string weaponName;
    private Color buttonImageColor;
    // Start is called before the first frame update
    void Start()
    {
        buttonImageColor = buttonImage.color;
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mouse_over && Input.GetMouseButtonUp(0)) {
            selected = !selected;
            AudioManager.Instance.PlayButtonPress();
            if(selected) {
                buttonImageColor.a = 1f;
                buttonImage.color = buttonImageColor;
                weaponsManager.SetWeaponWindow(weaponName);
            }
            else {
                buttonImageColor.a = 0.561f;
                buttonImage.color = buttonImageColor;
                weaponsManager.RemoveWeaponWindow(weaponName);
            }
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        mouse_over = true;
        image.sprite = highlighted;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        mouse_over = false;
        if(!selected) {
            image.sprite = regular;
        }
    }
}
