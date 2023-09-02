using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject targetManager;
    public GameObject hand;
    public GameObject battleEnemyManager;
    private SingleTargetManager STM;
    private CardLayout cardLayout;
    private BattleEnemyManager BEM;
    private bool isPlayerTurn;
    private bool isHandDealt = false;
    // Start is called before the first frame update
    void Start()
    {
        STM = targetManager.GetComponent<SingleTargetManager>();
        BEM = battleEnemyManager.GetComponent<BattleEnemyManager>();
        cardLayout = hand.GetComponent<CardLayout>();
        isPlayerTurn = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerTurn) {
            // enable the TargetManager
            // possibly use TargetManager prefab and SetActive(true)? 
            // then disable TargetManager otherwise
            if (!isHandDealt) {
                cardLayout.DrawCards(5);
                isHandDealt = true;
            }
        }
        else {
            List<BattleEnemyContainer> battleEnemies = BEM.GetEnemies();
            foreach (BattleEnemyContainer battleEnemy in battleEnemies) {
                if(!battleEnemy.isDead) {

                }
            }
        }
    }

    public void TargetCardAction(Card card) {
        Debug.Log("battlemanager TargetCardAcion");
        int attackDmg = Int32.Parse(card.actions["ATK"]);
        if(attackDmg > 0) {
            STM.GetTarget().TakeDamage(attackDmg);
        }
    }
}
