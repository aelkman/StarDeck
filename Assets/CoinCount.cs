using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCount : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    // private MainManager mainManager;
    // Start is called before the first frame update
    void Start()
    {
        // if(GameObject.Find("MainManager") != null) {
        //     mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        // }
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = MainManager.Instance.coinCount.ToString();
    }

    public void SpendCoins(int coins) {
        MainManager.Instance.coinCount -= coins;
    }
}
