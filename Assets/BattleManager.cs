using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject targetManager;
    private bool isPlayerTurn;
    // Start is called before the first frame update
    void Start()
    {

        isPlayerTurn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerTurn) {
            // enable the TargetManager
            // possibly use TargetManager prefab and SetActive(true)? 
            // then disable TargetManager otherwise
        }
    }
}
