using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PotionRewardButton : PotionShopButton, IPointerEnterHandler, IPointerExitHandler
{
    public AudioSource takeItemAudio;
    // Update is called once per frame
    void Update()
    {
        if (mouse_over) {
            if (Input.GetMouseButtonUp(0)) {
                TakePotion();
            }
        }
    }

    public void TakePotion() {
        if(MainManager.Instance.potions.Count <= 2 && MainManager.Instance.coinCount >= potion.price) {
            takeItemAudio.Play();
            // canvasGroup.alpha = 0.3f;
            // canvasGroup.interactable = false;
            // canvasGroup.blocksRaycasts = false;
            MainManager.Instance.potions.Add(potion);
            potionUI.EquipPotion(potion);
            gameObject.SetActive(false);
        }
    }
}
