using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionUI : MonoBehaviour
{
    public PotionUIButton potion1;
    public GameObject potion1slot;
    public PotionUIButton potion2;
    public GameObject potion2slot;
    public PotionUIButton potion3;
    public GameObject potion3slot;
    
    // Start is called before the first frame update
    void Start()
    {
        UsePotion(0);
        UsePotion(1);
        UsePotion(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipPotion(int slot, Potion potion) {
        if(slot == 0) {
            potion1.gameObject.SetActive(true);
            potion1.SetPotion(potion);
            potion1slot.SetActive(false);
        }
        else if(slot == 1) {
            potion2.gameObject.SetActive(true);
            potion2.SetPotion(potion);
            potion2slot.SetActive(false);
        }
        else if(slot == 2){
            potion3.gameObject.SetActive(true);
            potion3.SetPotion(potion);
            potion3slot.SetActive(false);
        }
    }

    public void UsePotion(int slot) {
        if(slot == 0) {
            potion1.RemovePotion();
            potion1.gameObject.SetActive(false);
            potion1slot.SetActive(true);
        }
        else if(slot == 1) {
            potion2.RemovePotion();
            potion2.gameObject.SetActive(false);
            potion2slot.SetActive(true);
        }
        else if(slot == 2){
            potion3.RemovePotion();
            potion3.gameObject.SetActive(false);
            potion3slot.SetActive(true);
        }
    }
}
