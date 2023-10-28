using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class StatusHoverDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject descriptionHover;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI blindText;
    public TextMeshProUGUI blockText;
    public TextMeshProUGUI counterText;
    public TextMeshProUGUI stunText;
    public TextMeshProUGUI tauntText;
    public TextMeshProUGUI vulnText;
    public TextMeshProUGUI weakText;
    
    // Start is called before the first frame update
    void Start()
    {
        descriptionHover.GetComponent<Canvas>().sortingLayerName = "Character Layer";
        descriptionHover.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        descriptionHover.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        descriptionHover.SetActive(false);
    }
}
