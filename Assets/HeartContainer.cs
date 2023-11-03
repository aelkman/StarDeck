using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeartContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite highlighted;
    public Sprite regular;
    private bool mouseOver;
    public Image image;
    public int healthIncrease = 50;
    public GameObject hoverDescription;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "Heart Container - My heart burns for you<br><br>Increase Max HP by " + healthIncrease;
    }

    // Update is called once per frame
    void Update()
    {
        if(mouseOver && Input.GetMouseButtonUp(0)) {
            AudioManager.Instance.PlayHeal();
            MainManager.Instance.IncreaseMaxHealth(healthIncrease);
            gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        mouseOver = true;
        image.sprite = highlighted;
        hoverDescription.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        mouseOver = false;
        image.sprite = regular;
        hoverDescription.SetActive(false);
    }
}
