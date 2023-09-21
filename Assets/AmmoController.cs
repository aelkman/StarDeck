using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    public int charge = 3;
    public bool userHasBlaster = false;
    public PlayerStats playerStats;
    public GameObject charge1;
    public GameObject charge2;
    public GameObject charge3;

    // Start is called before the first frame update
    void Start()
    {
        if(playerStats.weapons.Contains("Blaster")) {
            userHasBlaster = true;
        }
        if (!userHasBlaster) {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FullCharge() {
        charge = 3;
        charge1.SetActive(true);
        charge2.SetActive(true);
        charge3.SetActive(true);
    }

    public void UseCharge(int charge) {
        this.charge -= charge;
        if(this.charge == 0) {
            charge1.SetActive(false);
            charge2.SetActive(false);
            charge3.SetActive(false);
        }
        else if(this.charge == 1) {
            charge1.SetActive(true);
            charge2.SetActive(false);
            charge3.SetActive(false);
        }
        else if(this.charge == 2) {
            charge1.SetActive(true);
            charge2.SetActive(true);
            charge3.SetActive(false);
        }
        else if(this.charge == 3) {
            charge1.SetActive(true);
            charge2.SetActive(true);
            charge3.SetActive(true);
        }
    }
}
