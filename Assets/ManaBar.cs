using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManaBar : MonoBehaviour
{

	public Slider slider;
	// public Image fill;
	public TextMeshProUGUI text;
	[Range(0.0f, 1.0f)]
	public float fill = 1.0f;
	public GameObject mask;
	public Vector3 maskOrigPos;
	public GameObject batteryFill;
	public Vector3 batteryOrigPos;

	void Start() {
		maskOrigPos = new Vector3(0f, 150f, 0f);
		Debug.Log("mana mask orig: " + maskOrigPos);
		batteryOrigPos = new Vector3(0f, -132.1f, 0f);
		Debug.Log("mana Battery orig: " + batteryOrigPos);
		SetManaNoText(1, 1);
	}

	public void SetMana(int mana, int maxMana) {
		// slider.maxValue = maxMana;
		// slider.value = mana;
		fill = (float)mana/(float)maxMana;
		Debug.Log(fill);
		mask.transform.localPosition = new Vector3(maskOrigPos.x, maskOrigPos.y - (1 - fill) * 107.1f - 25, maskOrigPos.z);
		batteryFill.transform.localPosition = new Vector3(batteryOrigPos.x, batteryOrigPos.y + ((1 - fill) * 107.1f + 25) * (2 - mask.transform.localScale.y), batteryOrigPos.z);
		// fill.fillAmount = (float)mana/(float)maxMana;
        text.text = mana.ToString() + "/" + maxMana.ToString();
	}

	public void SetManaNoText(int mana, int maxMana) {
		// slider.maxValue = maxMana;
		// slider.value = mana;
		fill = (float)mana/(float)maxMana;
		Debug.Log(fill);
		mask.transform.localPosition = new Vector3(maskOrigPos.x, maskOrigPos.y - (1 - fill) * 107.1f - 25, maskOrigPos.z);
		batteryFill.transform.localPosition = new Vector3(batteryOrigPos.x, batteryOrigPos.y + ((1 - fill) * 107.1f + 25) * (2 - mask.transform.localScale.y), batteryOrigPos.z);
		// fill.fillAmount = (float)mana/(float)maxMana;
	}



}
