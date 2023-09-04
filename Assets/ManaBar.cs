using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManaBar : MonoBehaviour
{

	public Slider slider;
	public Image fill;
	public TextMeshPro text;

	public void SetMana(int mana, int maxMana) {
		slider.maxValue = maxMana;
		slider.value = mana;
        text.text = mana.ToString() + "/" + maxMana.ToString();
	}

}
