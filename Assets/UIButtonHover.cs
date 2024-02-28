using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverDescription;
    // Start is called before the first frame update
    void Start()
    {
        hoverDescription.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        hoverDescription.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        hoverDescription.SetActive(false);
    }
}
