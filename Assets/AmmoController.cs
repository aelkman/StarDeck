using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoController : MonoBehaviour
{
    public float charge = 3;
    public float maxCharge = 3;
    public bool userHasBlaster = false;
    // public Image fill;
    public Slider slider;
    public TextMeshProUGUI tmp;
    public PlayerStats playerStats;
    // public GameObject charge1;
    // public GameObject charge2;
    // public GameObject charge3;

    // Start is called before the first frame update
    void Start()
    {
        if(playerStats.weapons.Contains("Blaster")) {
            userHasBlaster = true;
        }
        if (!userHasBlaster) {
            gameObject.SetActive(false);
        }

        tmp.text = charge + "/" + maxCharge;
        slider.value = charge/maxCharge;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsAmmoFull() {
        return charge == maxCharge;
    }

    public void ExpandSlots(int expandSize) {
        maxCharge += expandSize;
        charge = maxCharge;
        slider.value = 1;
        tmp.text = charge + "/" + maxCharge;
    }

    public void FullCharge() {
        charge = maxCharge;
        slider.value = charge/maxCharge;
        tmp.text = charge + "/" + maxCharge;
        // charge1.SetActive(true);
        // charge2.SetActive(true);
        // charge3.SetActive(true);
    }

    public void UseCharge(int charge) {
        if(charge > 0) {
            this.charge -= charge;
        }
        slider.value = this.charge/maxCharge;
        tmp.text = this.charge + "/" + maxCharge;
        // if(this.charge == 0) {
        //     charge1.SetActive(false);
        //     charge2.SetActive(false);
        //     charge3.SetActive(false);
        // }
        // else if(this.charge == 1) {
        //     charge1.SetActive(true);
        //     charge2.SetActive(false);
        //     charge3.SetActive(false);
        // }
        // else if(this.charge == 2) {
        //     charge1.SetActive(true);
        //     charge2.SetActive(true);
        //     charge3.SetActive(false);
        // }
        // else if(this.charge == 3) {
        //     charge1.SetActive(true);
        //     charge2.SetActive(true);
        //     charge3.SetActive(true);
        // }
    }
}
