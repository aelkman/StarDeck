using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthHUD : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    // Start is called before the first frame update
    void Start()
    {
        tmp.text = MainManager.Instance.playerHealth + "/" + MainManager.Instance.playerMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = MainManager.Instance.playerHealth + "/" + MainManager.Instance.playerMaxHealth;
    }
}
