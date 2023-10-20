using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class PotionRewardButton : PotionShopButton, IPointerEnterHandler, IPointerExitHandler
{
    public AudioSource takeItemAudio;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        potionUI = GameObject.Find("PotionUI").GetComponent<PotionUI>();
        potion = GetRandomPotion();
        SetPotion(potion);
    }

    // Update is called once per frame
    void Update()
    {
        if (mouse_over) {
            if (Input.GetMouseButtonUp(0)) {
                TakePotion();
            }
        }
    }

    private Potion GetRandomPotion() {
        var potions = Resources.LoadAll<Potion>("Consumeables/General").ToList();
        foreach(var weapon in MainManager.Instance.weapons) {
            if(weapon == "Blaster") {
                var blastPots = Resources.LoadAll<Potion>("Consumeables/Blaster");
                foreach(var pot in blastPots) {
                    potions.Add(pot);
                }
            }
        }
        int randomIndex = UnityEngine.Random.Range(0, potions.Count);
        return potions[randomIndex];
    }

    public void TakePotion() {
        if(MainManager.Instance.potions.Count <= 2) {
            takeItemAudio.Play();
            // canvasGroup.alpha = 0.3f;
            // canvasGroup.interactable = false;
            // canvasGroup.blocksRaycasts = false;
            MainManager.Instance.EquipPotion(potion);
            gameObject.SetActive(false);
        }
    }
}
