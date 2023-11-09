using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CoinsEarned : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int coinsEarned;
    public Sprite coinSelected;
    public Sprite coinRegular;
    private Image image;

    private bool mouse_over = false;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mouse_over) {
            if (Input.GetMouseButtonUp(0)) {
                AudioManager.Instance.PlayCoins();
                gameObject.SetActive(false);
                MainManager.Instance.coinCount += coinsEarned;
            }
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData) {
        mouse_over = true;
        image.sprite = coinSelected;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        mouse_over = false;
        image.sprite = coinRegular;
    }
}
