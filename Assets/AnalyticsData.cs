using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnalyticsData : MonoBehaviour
{
    public TextMeshProUGUI hammerDamage;
    public TextMeshProUGUI blasterDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hammerDamage.text = "Hammer Damage:  " + GameManager.Instance.weaponDamage["Hammer"];
        blasterDamage.text = "Blaster Damage: " + GameManager.Instance.weaponDamage["Blaster"];
    }
}
