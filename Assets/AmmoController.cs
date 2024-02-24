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
    public TextMeshProUGUI text;
    public PlayerStats playerStats;
    public GameObject mask;
	public Vector3 maskOrigPos;
	public GameObject batteryFill;
	public Vector3 batteryOrigPos;
	[Range(0.0f, 1.0f)]
    public float fill;
    // public GameObject charge1;
    // public GameObject charge2;
    // public GameObject charge3;
    void Awake() {
        charge = MainManager.Instance.maxCharges;
        maxCharge = MainManager.Instance.maxCharges;
    }


    // Start is called before the first frame update
    void Start()
    {
        if(playerStats.weapons.Contains("Blaster")) {
            userHasBlaster = true;
        }
        if (!userHasBlaster) {
            gameObject.SetActive(false);
        }


        // text.text = charge + "/" + maxCharge;
        // slider.value = charge/maxCharge;
        maskOrigPos = new Vector3(0f, 150f, 0f);
		Debug.Log("ammo mask orig: " + maskOrigPos);
		batteryOrigPos = new Vector3(0f, -132.1f, 0f);
		Debug.Log("ammo Battery orig: " + batteryOrigPos);
		SetCharge();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     charge = fill * 3;
    //     SetCharge();
    // }

    public bool IsAmmoFull() {
        return charge == maxCharge;
    }

    public void SetCharge() {
		fill = charge / maxCharge;
		Debug.Log("ammo fill: " + fill);
        Debug.Log("charge: " + charge + ", maxCharge: " + maxCharge);
		mask.transform.localPosition = new Vector3(maskOrigPos.x, maskOrigPos.y - (1 - fill) * 107.1f - 25, maskOrigPos.z);
		batteryFill.transform.localPosition = new Vector3(batteryOrigPos.x, batteryOrigPos.y + ((1 - fill) * 107.1f + 25) * (2 - mask.transform.localScale.y), batteryOrigPos.z);
		// fill.fillAmount = (float)mana/(float)maxMana;
        text.text = charge + "/" + maxCharge;
    }

    public void SetChargeNoText() {
		fill = charge / maxCharge;
		Debug.Log("ammo fill: " + fill);
        Debug.Log("charge: " + charge + ", maxCharge: " + maxCharge);
		mask.transform.localPosition = new Vector3(maskOrigPos.x, maskOrigPos.y - (1 - fill) * 107.1f - 25, maskOrigPos.z);
		batteryFill.transform.localPosition = new Vector3(batteryOrigPos.x, batteryOrigPos.y + ((1 - fill) * 107.1f + 25) * (2 - mask.transform.localScale.y), batteryOrigPos.z);
		// fill.fillAmount = (float)mana/(float)maxMana;
    }

    public void ExpandSlots(int expandSize, bool withReload) {
        maxCharge += expandSize;
        if(withReload) {
            charge = maxCharge;
        }
        SetCharge();
    }

    public void FullCharge() {
        charge = maxCharge;
        SetCharge();
        // charge1.SetActive(true);
        // charge2.SetActive(true);
        // charge3.SetActive(true);
    }

    public void Charge(int chargeAdded) {
        charge += chargeAdded;
        if(charge > maxCharge) {
            charge = maxCharge;
        }
        SetCharge();
    }

    public void UseCharge(int charge) {
        if(charge > this.charge) {
            throw new System.Exception("attempted charge of " + charge + "was greater than remaining charges of " + this.charge);
        }
        if(this.charge > 0) {
            this.charge -= charge;
        }
        else {
            throw new System.Exception("Invalid charge in UseCharge, charge started as " + this.charge);
        }
        SetCharge();
    }
}
