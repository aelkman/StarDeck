using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class StatusHoverDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject descriptionHover;
    public CharacterHUD characterHUD;
    public TextMeshProUGUI powersText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI blindText;
    public TextMeshProUGUI blockText;
    public TextMeshProUGUI counterText;
    public TextMeshProUGUI stunText;
    public TextMeshProUGUI tauntText;
    public TextMeshProUGUI vulnText;
    public TextMeshProUGUI weakText;
    public bool isEmpty;
    
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
        if(characterHUD.statuses.Count > 0) {
            descriptionHover.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        descriptionHover.SetActive(false);
    }
}
