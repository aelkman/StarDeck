using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class DeckViewer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool mouse_over;
    public GameObject deckViewer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mouse_over) {
            if (Input.GetMouseButtonUp(0)) {
                ToggleActive();
            }
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
    }
}
