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
    public GameObject ammoControllerInstance;
    private AmmoController ammoController;
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
    public float attackDelay = 0.25f;
    private bool isPocketGenerator = false;
    public bool noDamageTaken = true;
    // Start is called before the first frame update
    void Start()
    {
        ammoController = ammoControllerInstance.GetComponent<AmmoController>();
        enemyActions = new List<Tuple<BattleEnemyContainer, Card>>();
        STM = targetManager.GetComponent<SingleTargetManager>();
        BEM = battleEnemyManager.GetComponent<BattleEnemyManager>();

        // wait until the BEM has finished initializing it's Start()
        handManager = hand.GetComponent<HandManager>();
        isPlayerTurn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(BEM.isInitialized){
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
        else {
            // add some ui element to tell user not enough mana
            return false;
        }
    }

    public bool CheckBlasterCanAct(CardDisplay cardDisplay) {

        String cardType = cardDisplay.card.type;
        // for all non attack Blaster cards, they will not use a charge
        int requiredCharges = 0;

        if(cardDisplay.card.actions.ContainsKey("ATK") && cardType == "Blaster") {
            requiredCharges = cardDisplay.card.actions["ATK"].Split(',').Select(int.Parse).ToList()[1];
        }

        if(!ammoController.userHasBlaster) {
            return true;
        }
        else if(ammoController.charge >= requiredCharges) {
            // add some ui element to tell the user not enough ammo
            return true;
        }
        else return false;
    }

    public IEnumerator CardAction(Card card) {
        // Debug.Log("battlemanager TargetCardAcion");
        // bool canAct = CheckCanAct(cardDisplay.card);
        // Debug.Log("canAct: " + canAct);
        string cardType = card.type;
        // bool canBlasterAct = true;

        // // now check here if ammo charges are needed & allowed
        // if(cardDisplay.card.actions.ContainsKey("ATK") && cardType == "Blaster") {
        //     int requiredCharges = cardDisplay.card.actions["ATK"].Split(',').Select(int.Parse).ToList()[1];
        //     if (!CheckBlasterCanAct(requiredCharges)) {
        //         canBlasterAct = false;
        //     }
        // }

        // canBlasterAct will always be true if it's not a Blaster card
        // if(canAct && canBlasterAct) {
            // first, consume card mana
            playerStats.useMana(card.manaCost);

            foreach(var item in card.actions) {
                switch(item.Key) {
                    case "ATK":
                        List<int> multiAttack = card.actions["ATK"].Split(',').Select(int.Parse).ToList();

                        if (multiAttack.Count != 2) {
                            throw new Exception("Invalid ATK attributes! Must be 2 ints comma separated.");
                        }

                        Vector3 STMPos = STM.GetTarget().transform.position;

                        for (int i = 0; i < multiAttack[1]; i++) {
                            bool isLast = i == (multiAttack[1] - 1);
                            playerStats.transform.parent.GetComponent<PlayerAnimator>().AttackAnimation();
                            // weapon animations here
                            switch(cardType) {
                                case "Blaster":
                                    StartCoroutine(BlasterAttack(STMPos, 0.1f, card));
                                    break;
                                default:
                                    break;
                            }
                            StartCoroutine(STM.GetTarget().TakeDamage(multiAttack[0], attackDelay, returnValue => {}));
                            if (!isLast) {
                                yield return new WaitForSeconds(0.5f);
                            }
                        }
                        break;
                    case "DEF":
                        playerStats.addBlock(Int32.Parse(item.Value));
                        StartCoroutine(PlayerShieldSequence());
                        // StartCoroutine(DelayCardDeletion(cardDisplay));
                        break;
                    case "DRAW":
                        handManager.DrawCards(1);
                        break;
                    case "HACK":
                        // perform hacking animation here
                        if(item.Value == "SHIELD") {
                            if(STM.GetTarget().block <= 10) {
                                STM.GetTarget().resetBlock();
                            }
                        }
                        break;
                    case "QUICKDRAW":
                        StartCoroutine(handManager.DrawCardsTimed(2, cardsReturnValue => {
                            foreach(Card card in cardsReturnValue) {
                                if (card.subType == "Shot") {
                                    playerStats.addMana(1);
                                }
                            }
                        }));
                        break;
                    case "STN":
                        STM.GetTarget().stunnedTurns += Int32.Parse(item.Value);
                        Debug.Log("target stunned: " + STM.GetTarget().stunnedTurns);
                        STM.GetTarget().ShockAnimation();
                        // StartCoroutine(DelayCardDeletion(cardDisplay));
                        break;
                    case "VULN":
                        if(card.isTarget) {
                            STM.GetTarget().AddVuln(Int32.Parse(item.Value));
                            STM.GetTarget().VulnerableAnimation();
                        }
                        else {
                            playerStats.AddVuln(Int32.Parse(item.Value));
                            playerStats.VulnerableAnimation();
                        }
                        break;
                    case "RELOAD":
                        ammoController.FullCharge();
                        break;
                    case "FINAL_BLOW":
                        List<string> finalBlow = card.actions["FINAL_BLOW"].Split(',').ToList();

                        // if (finalBlow.Count != 4) {
                        //     throw new Exception("Invalid FINAL_BLOW attributes! Must be 4 strings comma separated.");
                        // }

                        int dmg = Int32.Parse(finalBlow[0]);
                        int multiplier = Int32.Parse(finalBlow[1]);

                        // unfortunate code copying, but refacting is too much work
                        Vector3 STMPos2 = STM.GetTarget().transform.position;

                        for (int i = 0; i < multiplier; i++) {
                            bool isLast = i == (multiplier - 1);
                            playerStats.transform.parent.GetComponent<PlayerAnimator>().AttackAnimation();
                            // weapon animations here
                            switch(cardType) {
                                case "Blaster":
                                    StartCoroutine(BlasterAttack(STMPos2, 0.1f, card));
                                    break;
                                default:
                                    break;
                            }
                            StartCoroutine(STM.GetTarget().TakeDamage(dmg, attackDelay, isDeadReturnValue => {
                                if(isDeadReturnValue) {
                                    // if the enemy was killed, perform the next action
                                    string nextAction = finalBlow[2];
                                    // for now, this only works with RELOAD... will have to fix later
                                    Card newCard = ScriptableObject.CreateInstance<Card>();
                                    newCard.type = "Blaster";
                                    newCard.manaCost = 0;
                                    newCard.actions = new Dictionary<string, string>();
                                    newCard.actions.Add(finalBlow[2], finalBlow[3]);
                                    // recursion
                                    StartCoroutine(CardAction(newCard));
                                }
                            }));
                            if (!isLast) {
                                yield return new WaitForSeconds(0.5f);
                            }
                        }
                        break;
                    case "POWER":
                        if (item.Value == "POCK_GEN") {
                            ammoController.FullCharge();
                            isPocketGenerator = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            // unlock the target
            STM.SetTarget(null);
            STM.targetLocked = false;
        // }
        // else {
        //     Debug.Log("card could not be played, not enough mana!");
        // }
    }

    private IEnumerator PlayerShieldSequence() {
        playerStats.playerAnimator.BlockAnimation();
        yield return new WaitForSeconds(0.5f);
        playerStats.shieldAnimator.StartForceField();
    }

    private IEnumerator BlasterAttack(Vector3 STMPos, float timeInterval, Card card) {
        yield return new WaitForSeconds(attackDelay);

        // reduce the charges in the ammo container
        if (isPocketGenerator && card.subType == "Shot") {
            Debug.Log("pocket generator strikes again!");
        }
        else {
            ammoController.UseCharge(1);
        }

        Debug.Log(playerStats.transform.position.x);
        Vector3 startingPosition = new Vector3(playerStats.transform.position.x + 0.1f, playerStats.transform.position.y + 0.1f, playerStats.transform.position.z);
        Vector3 endPosition = STMPos;
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

    private void GenerateEnemyActions(List<BattleEnemyContainer> battleEnemies) {
        foreach (BattleEnemyContainer battleEnemy in battleEnemies) {
            // while(battleEnemy.actions.Length == 0) {
            //     // wait until it loads
            // }
            if(battleEnemy.actions.Length > 0){
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
                                    switch(battleEnemy.battleEnemy.name) {
                                        case "GoldBot":
                                            battleEnemy.transform.parent.GetComponent<EnemyAnimator>().GoldBot_Melee_1();
                                            yield return new WaitForSeconds(0.75f);
                                            break;
                                        default:
                                            battleEnemy.transform.parent.GetComponent<EnemyAnimator>().AttackAnimation();
                                            break;
                                    }
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
                                    battleEnemy.BlockSequence(block);
                                }
                                break;
                            case "ATK_MOD":
                                int atkMod = Int32.Parse(randomAction.actions["ATK_MOD"]);
                                Debug.Log("attack mod: " + atkMod.ToString());
                                battleEnemy.modifyAtk(atkMod);
                                battleEnemy.transform.parent.GetComponent<EnemyAnimator>().CastAnimation();
                                battleEnemy.SwordAnimation();
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
        playerStats.RemoveSingleVuln();
        foreach(BattleEnemyContainer be in battleEnemies) {
            be.RemoveSingleVuln();
        }
    }

    public void ResetEnemyShields() {
        foreach(BattleEnemyContainer battleEnemy in BEM.GetBattleEnemies()) {
            battleEnemy.resetBlock();
        }
    }
}
