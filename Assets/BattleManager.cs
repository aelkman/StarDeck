using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject targetManager;
    public GameObject hand;
    public GameObject battleEnemyManager;
    public PlayerStats playerStats;
    private SingleTargetManager STM;
    private CardLayout cardLayout;
    private BattleEnemyManager BEM;
    private List<Tuple<BattleEnemyContainer, Card>> enemyActions;
    private bool areEnemyActionsDecided = false;
    private List<BattleEnemyContainer> battleEnemies;
    private bool isPlayerTurn;
    private bool isHandDealt = false;
    // Start is called before the first frame update
    void Start()
    {
        enemyActions = new List<Tuple<BattleEnemyContainer, Card>>();
        STM = targetManager.GetComponent<SingleTargetManager>();
        BEM = battleEnemyManager.GetComponent<BattleEnemyManager>();
        cardLayout = hand.GetComponent<CardLayout>();
        isPlayerTurn = true;
    }

    // Update is called once per frame
    void Update()
    {
        battleEnemies = BEM.GetEnemies();

        if(isPlayerTurn) {
            // enable the TargetManager
            // possibly use TargetManager prefab and SetActive(true)? 
            // then disable TargetManager otherwise
            if(!areEnemyActionsDecided) {
                GenerateEnemyActions(battleEnemies);
                areEnemyActionsDecided = true;
            }
            if (!isHandDealt) {
                cardLayout.DrawCards(5);
                isHandDealt = true;
            }
        }
        else {
            // enemy turn
            // make GetEnemies filter out dead enemies
            StartCoroutine(ProcessEnemyAction(battleEnemies));
            isPlayerTurn = true;
        }
    }

    public void TargetCardAction(Card card) {
        Debug.Log("battlemanager TargetCardAcion");
        int attackDmg = Int32.Parse(card.actions["ATK"]);
        if(attackDmg > 0) {
            STM.GetTarget().TakeDamage(attackDmg);
        }
    }

    private void GenerateEnemyActions(List<BattleEnemyContainer> battleEnemies) {
        foreach (BattleEnemyContainer battleEnemy in battleEnemies) {
            Card randomAction = battleEnemy.RandomAction();
            // pass the action back to the enemy to display
            string actionText = randomAction.name;
            foreach(KeyValuePair<string, string> entry in randomAction.actions) {
                actionText += "<br>" + entry.Key + " " + entry.Value;
            }
            battleEnemy.SetNextActionText(actionText);
            enemyActions.Add(new Tuple<BattleEnemyContainer,Card>(battleEnemy, randomAction));
        }
    }

    private IEnumerator ProcessEnemyAction(List<BattleEnemyContainer> battleEnemies) {
        foreach (Tuple<BattleEnemyContainer,Card> enemyActionPair in enemyActions) {

            Card randomAction = enemyActionPair.Item2;

            if(randomAction.actions.ContainsKey("ATK_RND")) {
                // parse ATK_RND params
                List<int> randAttack = randomAction.actions["ATK_RND"].Split(',').Select(int.Parse).ToList();

                if (randAttack.Count != 2) {
                    throw new Exception("Invalid ATK_RND attributes! Must be 2 ints comma separated.");
                }
                else {
                    int atkDmg = UnityEngine.Random.Range(randAttack[0] - randAttack[1], randAttack[0] + randAttack[1]);
                    Debug.Log("attack action: " + atkDmg);
                    playerStats.takeDamage(atkDmg);
                }
            }
            if(randomAction.actions.ContainsKey("ATK_MOD")) {
                Debug.Log("attack mod: ");
            }

            yield return new WaitForSeconds(1.5f);

        }

        EndTurn();
    }

    public void EndPlayerTurn() {
        isPlayerTurn = false;
    }

    public void EndTurn() {
        enemyActions = new List<Tuple<BattleEnemyContainer, Card>>();
        areEnemyActionsDecided = false;
    }
}
