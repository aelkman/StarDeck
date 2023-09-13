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
    public GameObject discardDeck;
    public GameObject laserAttack;
    public GameObject characterSpace;
    public GameOver gameOver;
    public BattleWon battleWon;
    public PlayerStats playerStats;
    private SingleTargetManager STM;
    private HandManager handManager;
    private BattleEnemyManager BEM;
    private List<Tuple<BattleEnemyContainer, Card>> enemyActions;
    private bool areEnemyActionsDecided = false;
    private List<BattleEnemyContainer> battleEnemies;
    private bool isPlayerTurn;
    private bool isHandDealt = false;
    private bool isGameOver = false;
    private bool isBattleWon = false;
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

        if(!isBattleWon) {
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
    }

    public void GameOver() {
        isGameOver = true;
        gameOver.Initiate();
    }

    public void BattleWin() {
        isBattleWon = true;
        battleWon.Initiate();
    }

    public bool CheckCanAct(Card card) {
        if(STM.GetTarget() == null && card.isTarget) {
            return false;
        }
        if(playerStats.stats.mana >= card.manaCost) {
            return true;
        }
        else return false;
    }

    public IEnumerator CardAction(CardDisplay cardDisplay) {
        Debug.Log("battlemanager TargetCardAcion");
        bool canAct = CheckCanAct(cardDisplay.card);
        Debug.Log("canAct: " + canAct);
        if(canAct) {
            // first, consume card mana
            playerStats.useMana(cardDisplay.card.manaCost);

            foreach(var item in cardDisplay.card.actions) {
                switch(item.Key) {
                    case "ATK":
                        List<int> multiAttack = cardDisplay.card.actions["ATK"].Split(',').Select(int.Parse).ToList();

                        if (multiAttack.Count != 2) {
                            throw new Exception("Invalid ATK attributes! Must be 2 ints comma separated.");
                        }

                        // StartCoroutine(DelayCardDeletion(cardDisplay));

                        // perform multi attack
                        // StartCoroutine(MultiAttack(0.5f, multiAttack, cardDisplay.card.type));
                        string cardType = cardDisplay.card.type;

                        for (int i = 0; i < multiAttack[1]; i++) {
                            bool isLast = i == (multiAttack[1] - 1);
                            playerStats.transform.parent.GetComponent<PlayerAnimator>().AttackAnimation();
                            // weapon animations here
                            switch(cardType) {
                                case "Blaster":
                                    StartCoroutine(LaserAttack(0.1f));
                                    break;
                                default:
                                    break;
                            }
                            STM.GetTarget().TakeDamage(multiAttack[0]);
                            if (!isLast) {
                                yield return new WaitForSeconds(0.5f);
                            }
                        }
                        break;
                    case "DEF":
                        playerStats.addBlock(Int32.Parse(item.Value));
                        playerStats.shieldAnimator.StartForceField();
                        // StartCoroutine(DelayCardDeletion(cardDisplay));
                        break;
                    case "STN":
                        STM.GetTarget().stunnedTurns += Int32.Parse(item.Value);
                        Debug.Log("target stunned: " + STM.GetTarget().stunnedTurns);
                        STM.GetTarget().ShockAnimation();
                        // StartCoroutine(DelayCardDeletion(cardDisplay));
                        break;
                    case "WEAK":
                        playerStats.AddWeak(Int32.Parse(item.Value));
                        break;
                    default:
                        break;
                }
            }
            // unlock the target
            STM.SetTarget(null);
            STM.targetLocked = false;
        }
        else {
            Debug.Log("card could not be played, not enough mana!");
        }
    }

    private IEnumerator LaserAttack(float timeInterval) {
        Vector3 startingPosition = playerStats.transform.position;
        Vector3 endPosition = STM.GetTarget().transform.position;
        GameObject newLaser = Instantiate(laserAttack, startingPosition, Quaternion.identity, characterSpace.transform);
        for (float i = 0; i < 1; i+=timeInterval){
            newLaser.transform.position = new Vector3(
                (Mathf.Lerp(startingPosition.x, endPosition.x, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(startingPosition.y, endPosition.y, Mathf.SmoothStep(0f, 1f, i))),
                0
            );
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(newLaser);
    }

    // private IEnumerator MultiAttack(float time, List<int> multiAttack, string cardType) {
    //     for (int i = 0; i < multiAttack[1]; i++) {
    //         bool isLast = i == (multiAttack[1] - 1);
    //         playerStats.transform.parent.GetComponent<PlayerAnimator>().AttackAnimation();
    //         // weapon animations here
    //         switch(cardType) {
    //             case "Blaster":
    //                 StartCoroutine(LaserAttack(0.1f));
    //                 break;
    //             default:
    //                 break;
    //         }
    //         STM.GetTarget().TakeDamage(multiAttack[0]);
    //         if (!isLast) {
    //             yield return new WaitForSeconds(time);
    //         }
    //     }
    //     // unlock the target
    //     STM.SetTarget(null);
    //     STM.targetLocked = false;
    // }

    // private IEnumerator DelayCardDeletion(CardDisplay cardDisplay) {
    //     yield return new WaitForSeconds(1.5f);
    //     handManager.PlayCard(cardDisplay);
    // }

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

    public void RemoveEnemyActions(BattleEnemyContainer battleEnemy) {
        for (int i = 0; i < enemyActions.Count; i++) {
        // foreach(Tuple<BattleEnemyContainer,Card> tuple in enemyActions) {
            if(enemyActions[i].Item1 == battleEnemy) {
                enemyActions.RemoveAt(i);
                i--;
            }
        }
    }

    private IEnumerator ProcessEnemyAction(List<BattleEnemyContainer> battleEnemies) {
        foreach (Tuple<BattleEnemyContainer,Card> enemyActionPair in enemyActions) {
            if (!isGameOver) {

                yield return new WaitForSecondsRealtime(0.75f);

                Card randomAction = enemyActionPair.Item2;
                BattleEnemyContainer battleEnemy = enemyActionPair.Item1;

                if (battleEnemy.stunnedTurns > 0) {
                    // play stun animation
                    battleEnemy.ShockAnimation();
                    battleEnemy.stunnedTurns -= 1; 
                }
                else {
                    foreach(var item in randomAction.actions) {
                        switch(item.Key) {
                            case "ATK_RND":
                                List<int> randAttack = randomAction.actions["ATK_RND"].Split(',').Select(int.Parse).ToList();

                                if (randAttack.Count != 2) {
                                    throw new Exception("Invalid ATK_RND attributes! Must be 2 ints comma separated.");
                                }
                                else {
                                    int atkDmg = UnityEngine.Random.Range(randAttack[0] - randAttack[1], randAttack[0] + randAttack[1] + 1);
                                    atkDmg += battleEnemy.getAtkMod();
                                    Debug.Log("attack action: " + atkDmg);
                                    battleEnemy.transform.parent.GetComponent<EnemyAnimator>().AttackAnimation();
                                    if(playerStats.hasBlock()) {
                                        if(playerStats.getBlock() <= atkDmg) {
                                            atkDmg = atkDmg - playerStats.getBlock();
                                            playerStats.setBlock(0);
                                            playerStats.shieldAnimator.StopForceField();
                                        }
                                        else {
                                            playerStats.setBlock(playerStats.getBlock() - atkDmg);
                                            atkDmg = 0;
                                        }
                                    }
                                    playerStats.takeDamage(atkDmg);
                                    playerStats.transform.parent.GetComponent<PlayerAnimator>().DamageAnimation();
                                }
                                break;
                            case "DEF_RND":
                                List<int> randBlock = randomAction.actions["DEF_RND"].Split(',').Select(int.Parse).ToList();

                                if (randBlock.Count != 2) {
                                    throw new Exception("Invalid DEF_RND attributes! Must be 2 ints comma separated.");
                                }
                                else {
                                    int block = UnityEngine.Random.Range(randBlock[0] - randBlock[1], randBlock[0] + randBlock[1] + 1);
                                    // to-do, defMod bonus
                                    // block += etc
                                    Debug.Log("block action: " + block);
                                    battleEnemy.shieldAnimator.StartForceField();
                                    battleEnemy.addBlock(block);
                                }
                                break;
                            case "ATK_MOD":
                                int atkMod = Int32.Parse(randomAction.actions["ATK_MOD"]);
                                Debug.Log("attack mod: " + atkMod.ToString());
                                battleEnemy.modifyAtk(atkMod);
                                battleEnemy.transform.parent.GetComponent<EnemyAnimator>().CastAnimation();
                                break;
                            default:
                                break;
                        }
                    }
                }

                yield return new WaitForSecondsRealtime(0.75f);
            }
        }

        EndEnemyTurn();
    }

    public void EndPlayerTurn() {
        isPlayerTurn = false;
        ResetEnemyShields();
    }

    public void EndEnemyTurn() {
        enemyActions = new List<Tuple<BattleEnemyContainer, Card>>();
        areEnemyActionsDecided = false;
        handManager.DiscardHand();
        handManager.DrawCards(5);
        playerStats.resetMana();
        playerStats.resetBlock();
        playerStats.weak -= 1;
    }

    public void ResetEnemyShields() {
        foreach(BattleEnemyContainer battleEnemy in BEM.GetBattleEnemies()) {
            battleEnemy.resetBlock();
        }
    }
}
