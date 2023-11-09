using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExtractorScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject highlight;
    private DeckViewer deckViewer;
    private bool mouse_over;
    public ItemCost itemCost;
    // Start is called before the first frame update
    void Start()
    {
        deckViewer = PersistentHUD.Instance.deckViewer;
        mouse_over = false;
        highlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(mouse_over) {
            if(Input.GetMouseButtonUp(0)) {
                RemovalStart();
            }
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        mouse_over = true;
        highlight.SetActive(true);


    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        mouse_over = false;
        highlight.SetActive(false);

    }

    public void RemovalStart() {
        if(MainManager.Instance.coinCount >= itemCost.price) {
            deckViewer.StartRemoval(itemCost.price);
        }
        else {
            MainManager.Instance.NotEnoughMoney();
        }
    }
}
