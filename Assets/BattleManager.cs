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
    public PlayerHUD playerHUD;
    private SingleTargetManager STM;
    private HandManager handManager;
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
        handManager = hand.GetComponent<HandManager>();
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
                handManager.DrawCards(5);
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

    public bool CheckCanAct(Card card) {
        if(playerStats.stats.mana >= card.manaCost) {
            return true;
        }
        else return false;
    }

    public void TargetCardAction(CardDisplay cardDisplay) {
        Debug.Log("battlemanager TargetCardAcion");
        bool canAct = CheckCanAct(cardDisplay.card);
        Debug.Log("canAct: " + canAct);
        if(canAct) {
            // now tell the HandManager to play and remove the card
            int attackDmg = Int32.Parse(cardDisplay.card.actions["ATK"]);
            playerStats.useMana(cardDisplay.card.manaCost);
            if(attackDmg > 0) {
                STM.GetTarget().TakeDamage(attackDmg);
                STM.GetTarget().transform.parent.GetComponent<EnemyAnimator>().TakeDamageAnimation();
            }
            handManager.PlayCard(cardDisplay);
            playerStats.transform.parent.GetComponent<PlayerAnimator>().AttackAnimation();
        }
        else {
            Debug.Log("card could not be played, not enough mana!");
        }
    }

    public void CardAction(CardDisplay cardDisplay) {
        Debug.Log("battleManager CardAction");
        bool canAct = CheckCanAct(cardDisplay.card);
        Debug.Log("canAct: " + canAct);
        if(canAct) {
            playerStats.useMana(cardDisplay.card.manaCost);
            foreach(var item in cardDisplay.card.actions) {
                switch(item.Key) {
                    case "DEF":
                        playerStats.addBlock(Int32.Parse(item.Value));
                        playerHUD.ActivateBlockUI();
                        playerHUD.blockText.BlockAnimation();
                        handManager.PlayCard(cardDisplay);
                        break;
                    default:
                        break;
                }
            }
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
     
            yield return new WaitForSecondsRealtime(0.75f);

            Card randomAction = enemyActionPair.Item2;
            BattleEnemyContainer battleEnemy = enemyActionPair.Item1;

            if(randomAction.actions.ContainsKey("ATK_RND")) {
                // parse ATK_RND params
                List<int> randAttack = randomAction.actions["ATK_RND"].Split(',').Select(int.Parse).ToList();

                if (randAttack.Count != 2) {
                    throw new Exception("Invalid ATK_RND attributes! Must be 2 ints comma separated.");
                }
                else {
                    int atkDmg = UnityEngine.Random.Range(randAttack[0] - randAttack[1], randAttack[0] + randAttack[1]);
                    atkDmg += battleEnemy.getAtkMod();
                    Debug.Log("attack action: " + atkDmg);
                    battleEnemy.transform.parent.GetComponent<EnemyAnimator>().AttackAnimation();
                    if(playerStats.hasBlock()) {
                        if(playerStats.getBlock() <= atkDmg) {
                            atkDmg = atkDmg - playerStats.getBlock();
                            playerStats.setBlock(0);
                        }
                        else {
                            playerStats.setBlock(playerStats.getBlock() - atkDmg);
                            atkDmg = 0;
                        }
                    }
                    playerStats.takeDamage(atkDmg);
                    playerStats.transform.parent.GetComponent<PlayerAnimator>().DamageAnimation();
                }
            }
            if(randomAction.actions.ContainsKey("ATK_MOD")) {
                int atkMod = Int32.Parse(randomAction.actions["ATK_MOD"]);
                Debug.Log("attack mod: " + atkMod.ToString());
                battleEnemy.modifyAtk(atkMod);
                battleEnemy.transform.parent.GetComponent<EnemyAnimator>().CastAnimation();
            }

            yield return new WaitForSecondsRealtime(0.75f);

        }

        EndTurn();
    }

    public void EndPlayerTurn() {
        isPlayerTurn = false;
    }

    public void EndTurn() {
        enemyActions = new List<Tuple<BattleEnemyContainer, Card>>();
        areEnemyActionsDecided = false;
        handManager.DiscardHand();
        handManager.DrawCards(5);
        playerStats.resetMana();
        playerStats.resetBlock();
    }
}
