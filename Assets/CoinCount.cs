using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCount : MonoBehaviour
{
    public int coinCount;
    public TextMeshProUGUI tmp;
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.Find("MainManager") != null) {
            coinCount = GameObject.Find("MainManager").GetComponent<MainManager>().coinCount;
        }
        else {
            coinCount = 100;
        }
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = coinCount.ToString();
    }
}
