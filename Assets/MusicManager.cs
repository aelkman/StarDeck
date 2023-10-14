using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public GameObject regularBattle;
    public GameObject bossBattle;
    // Start is called before the first frame update
    void Start()
    {
        if(MainManager.Instance.isBossBattle) {
            regularBattle.SetActive(false);
            bossBattle.SetActive(true);
        }
        else {
            regularBattle.SetActive(true);
            bossBattle.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
