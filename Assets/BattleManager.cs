using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public GameObject targetManager;
    public GameObject hand;
    public GameObject battleEnemyManager;
    public GameObject discardDeck;
    public GameObject laserAttack;
    public GameObject characterSpace;
    public GameObject noAmmoPrefab;
    public GameObject noEnergyPrefab;
    public GameOver gameOver;
    public BattleWon battleWon;
    public BattleAudioController bac;
    public GameObject mainCanvas;
    public GameObject kingBotStartingDialogue;
    public GameObject kingBotWinDialogue;
    public PlayerStats playerStats;
    public GameObject ammoControllerInstance;
    public GameObject shakeHolder;
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
    private bool isPowerSurge = false;
    private bool isPulseAmplifier = false;
    private bool isFrostWard = false;
    private bool isFyornsResolve = false;
    public bool isIceBarricade = false;
    private bool hasReloaded = false;
    private bool isFirstEnemyAttack = true;
    public bool isCharacterMissing = false;
    public bool noDamageTaken = true;
    public ScryUISelector scryUISelector;
    public bool isScryComplete = false;
    public Button endTurnButton;

    private float hammerAttackTime = 0.5f;
    private float blasterAttackTime = 0.3f;
    public FlashImage flashImage;
    // Start is called before the first frame update
    void Start()
    {
        if(MainManager.Instance.isBossBattle) {
            Instantiate(kingBotStartingDialogue, mainCanvas.transform);
        }
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

    public IEnumerator StartBossWinDialogue(BattleEnemyContainer thisEnemy) {
        GameObject go;
        if(thisEnemy.battleEnemy.name == "KingBot") {
            go = Instantiate(kingBotWinDialogue, mainCanvas.transform);
            var winDialogue = go.GetComponent<KingBotBossWinDialogue>();
            yield return new WaitUntil(() => winDialogue.isFinished);
        }
        else {
            go = new GameObject();
        }
        // after dialogue is finished
        BEM.EnemyDeath(thisEnemy);
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
            bac.PlayNegativeFeedback();
            Instantiate(noEnergyPrefab, shakeHolder.transform);
            return false;
        }
    }

    public bool CheckBlasterCanAct(CardDisplay cardDisplay) {

        String cardType = cardDisplay.card.type;
        // for all non attack Blaster cards, they will not use a charge
        int requiredCharges = 0;

        if((cardDisplay.card.actions.ContainsKey("ATK") && cardType == "Blaster")
            || (cardDisplay.card.actions.ContainsKey("ATK_ALL") && cardType == "Blaster")
            || (cardDisplay.card.actions.ContainsKey("FINAL_BLOW") && cardType == "Blaster")) {
            if(isPocketGenerator && cardDisplay.card.subType == "Shot") {
                requiredCharges = 0;
            }
            else {
                // int attack;
                int multi;
                List<string> strings;
                if(cardDisplay.card.actions.ContainsKey("ATK")){
                    strings = cardDisplay.card.actions["ATK"].Split(',').ToList();
                } 
                else if(cardDisplay.card.actions.ContainsKey("ATK_ALL")){
                    strings = cardDisplay.card.actions["ATK_ALL"].Split(',').ToList();
                }
                else if(cardDisplay.card.actions.ContainsKey("FINAL_BLOW")){
                    strings = cardDisplay.card.actions["FINAL_BLOW"].Split(',').ToList();
                }
                else {
                    throw new Exception("Invalid actions type for Blaster CheckCanAct! " + string.Join(Environment.NewLine, cardDisplay.card.actions));
                }

                if(strings[1] == "X"){
                    multi = playerStats.stats.mana;
                }
                else if(strings[1] == "A") {
                    multi = (int)ammoController.charge;
                }
                else {
                    multi = Int32.Parse(strings[1]);
                }
                requiredCharges = multi;
            }

        }

        if(!ammoController.userHasBlaster) {
            return true;
        }
        else if(ammoController.charge >= requiredCharges) {
            // add some ui element to tell the user not enough ammo
            return true;
        }
        else {
            // play noammo UI element
            bac.PlayNegativeFeedback();
            Instantiate(noAmmoPrefab, shakeHolder.transform);   
            return false;
        }
    }

    private void SetConfusionTarget() {
        int target = UnityEngine.Random.Range(0, battleEnemies.Count + 1);
        if(target == 0) {
            STM.SetTarget(playerStats);
        }
        else {
            STM.SetTarget(battleEnemies[target]);
        }
    }

    private bool DidTargetMiss(BaseCharacterInfo character) {
        bool miss = false;
        if(character.isBlind()) {
            // see if they miss
            miss = UnityEngine.Random.Range(0, 2) == 1;
        }
        return miss;
    }

    public IEnumerator CardAction(Card card) {
        string cardType = card.type;

        // first, consume card mana
        playerStats.useMana(card.manaCost);

        for(int x = 0; x < card.actions.Count; x++ ) {
        // foreach(var item in card.actions) {
            var item = card.actions.ToList()[x];
            switch(item.Key) {
                case "ATK":
                    int attack;
                    int multi;
                    // List<int> multiAttack = new List<int>();
                    if(item.Value == "DEF") {
                        attack = playerStats.block;
                        multi = 1;
                        // multiAttack.Add(playerStats.block);
                        // multiAttack.Add(1);
                    }
                    else {
                        var strings = item.Value.Split(',').ToList();
                        attack = Int32.Parse(strings[0]);

                        if(strings[1] == "X"){
                            multi = playerStats.stats.mana;
                        }
                        else if(strings[1] == "A") {
                            multi = (int)ammoController.charge;
                        }
                        else {
                            multi = Int32.Parse(strings[1]);
                        }
                        // multiAttack = card.actions["ATK"].Split(',').Select(int.Parse).ToList();
                    }

                    // if (multiAttack.Count != 2) {
                    //     throw new Exception("Invalid ATK attributes! Must be 2 ints comma separated.");
                    // }

                    float atkMod = 1.0f;

                    for (int i = 0; i < multi; i++) {
                        Vector3 STMPos;
                        isCharacterMissing = DidTargetMiss(playerStats);
                        if(isCharacterMissing) {
                            Vector3 targPos = STM.GetTarget().transform.position;
                            STMPos = new Vector3(targPos.x, targPos.y + 100f, targPos.z);
                        }
                        else {
                            if(playerStats.tauntTurns > 0) {
                                STM.SetTarget(playerStats.tauntingEnemy);
                                atkMod += MainManager.Instance.tauntBonus;
                            }
                            STMPos = STM.GetTarget().transform.position;
                        }

                        if(isFyornsResolve) {
                            playerStats.CharacterShield(3);
                        }

                        bool isLast = i == (multi - 1);
                        // weapon animations here
                        switch(cardType) {
                            case "Blaster":
                                StartCoroutine(BlasterAttack(STMPos, 0.1f, card, true, isLast));
                                break;
                            case "Blaster_All":
                                StartCoroutine(BlasterAttack(STMPos, 0.1f, card, false, isLast));
                                break;
                            case "Hammer":
                                HammerAttack(isLast);
                                break;
                            default:
                                break;
                        }

                        if(!isCharacterMissing) {
                            int damage = (int)Math.Round(((float)attack + playerStats.getAtkMod()) * atkMod);
                            if(damage < 0) {
                                damage = 0;
                            }
                            damage = WeakenedDamage(damage, playerStats);
                            // check target type
                            if(STM.GetTarget() is BattleEnemyContainer) {
                                // if player using hammer, then add a stack
                                if(cardType == "Hammer") {
                                    if(isIceBarricade && STM.GetTarget().frostStacks + 1 >= 3) {
                                        playerStats.CharacterShield(5);
                                    }
                                    STM.GetTarget().AddFrost(1, hammerAttackTime, false);
                                }
                                StartCoroutine(((BattleEnemyContainer)STM.GetTarget()).TakeDamage(damage, attackDelay, isDeadReturnValue => {
                                    if(isDeadReturnValue) {
                                        // if the enemy was killed, perform the next action
                                        isLast = true;
                                        i = multi;
                                    }
                                }));
                            }
                            // else {
                            //     // player is blinded, attack self
                            //     ((PlayerStats)STM.GetTarget()).takeDamage(damage);
                            // }
                        }
                        if (!isLast) {
                            yield return new WaitForSeconds(0.5f);
                        }
                    }
                    break;
                case "ATK_ALL":
                    if(card.type == "Blaster") {
                        ammoController.UseCharge(1);
                    }
                    foreach(var enemy in battleEnemies) {
                        Card newCard = ScriptableObject.CreateInstance<Card>();
                        if(card.type == "Blaster") {
                            newCard.type = "Blaster_All";
                            // check if its the first, then otherwise remove Blaster type
                            // so that it doesnt consume more ammo
                        }
                        else {
                            newCard.type = card.type;
                        }
                        newCard.manaCost = 0;
                        newCard.actions = new Dictionary<string, string>();
                        newCard.actions.Add("ATK", item.Value);
                        // hopefully this works
                        STM.SetTarget(enemy);
                        // recursion
                        StartCoroutine(CardAction(newCard));
                    }
                    break;
                case "ATK_MOD":
                    playerStats.atkMod += Int32.Parse(item.Value);
                    if(card.name == "Lil Schnukles") {
                        playerStats.playerAnimator.DrinkPotionAnimation();
                        AudioManager.Instance.PlayMunchin();
                    }
                    else {
                        playerStats.playerAnimator.CastAnimation();
                        AudioManager.Instance.PlayArcanePower();
                    }
                    playerStats.SwordAnimation();
                    break;
                case "ATK_MOD_TEMP":
                    List<int> atkTemp = item.Value.Split(',').Select(int.Parse).ToList();
                    playerStats.playerAnimator.CastAnimation();

                    if(card.isTarget) {
                        STM.GetTarget().atkModTemp += atkTemp[0];
                        STM.GetTarget().atkModTempTurns = atkTemp[1];
                        STM.GetTarget().atkMod += atkTemp[0];
                        AudioManager.Instance.PlayGlassBreak();
                        STM.GetTarget().WeakenAnimation();
                    }
                    else {
                        playerStats.atkModTemp += atkTemp[0];
                        playerStats.atkModTempTurns = atkTemp[1];
                        playerStats.atkMod += atkTemp[0];
                        playerStats.SwordAnimation();
                    }

                    break;
                case "BLIND_ALL":
                    foreach(var battleEnemy in battleEnemies) {
                        battleEnemy.blind += 1;
                    }
                    break;
                case "BLIND":
                    STM.GetTarget().blind += 1;
                    break;
                case "CLEAR_DEBUFF":
                    if(card.name == "Gorga Snax") {
                        AudioManager.Instance.PlayMunchin();
                        playerStats.playerAnimator.DrinkPotionAnimation();
                    }
                    playerStats.tauntTurns = 0;
                    playerStats.blind = 0;
                    playerStats.vuln = 0;
                    playerStats.weak = 0;
                    if(playerStats.atkMod < 0) {
                        playerStats.atkMod = 0;
                    }
                    break;
                case "DEF":
                    if(item.Value == "2X") {
                        playerStats.characterAnimator.CastAnimation();
                        playerStats.DoubleBlock();
                    }
                    else{
                        playerStats.CharacterShield(Int32.Parse(item.Value));
                    }
                    // StartCoroutine(DelayCardDeletion(cardDisplay));
                    break;
                case "DEF_RELOAD":
                    var blocks = item.Value.Split(',').Select(int.Parse).ToList();
                    if(hasReloaded) {
                        playerStats.addBlock(blocks[1]);
                    }
                    else {
                        playerStats.addBlock(blocks[0]);
                    }

                    playerStats.CharacterShield(0);
                    break;
                case "DRAW":
                    handManager.DrawCards(Int32.Parse(item.Value));
                    break;
                case "DRAW_PULSE":
                    DrawPulse();
                    break;
                case "ICE_STACK":
                    playerStats.playerAnimator.CastAnimation();
                    STM.GetTarget().AddFrost(Int32.Parse(item.Value), 0.5f, true);
                    break;
                case "HACK":
                    // perform hacking animation here
                    if(item.Value == "SHIELD") {
                        STM.GetTarget().resetBlock();
                    }
                    break;
                case "HEAL":
                    playerStats.playerAnimator.DrinkPotionAnimation();
                    if(card.name == "Jingus Jerky") {
                        AudioManager.Instance.PlayMunchin();
                    }
                    playerStats.HealSelf(Int32.Parse(item.Value));
                    break;
                case "QUICKDRAW":
                    StartCoroutine(handManager.DrawCardsTimed(3, cardsReturnValue => {
                        foreach(Card card in cardsReturnValue) {
                            if (card.isAttack) {
                                playerStats.addMana(1);
                            }
                        }
                    }));
                    break;
                case "TOBOGGAN":
                    StartCoroutine(handManager.DrawCardsTimed(Int32.Parse(item.Value), cardsReturnValue => {
                        foreach(Card card in cardsReturnValue) {
                            if (card.isSkill) {
                                playerStats.addMana(1);
                            }
                        }
                    }));
                    break;
                case "STN":
                    // STM.GetTarget().stunnedTurns += Int32.Parse(item.Value);
                    if(STM.GetTarget().antiStunTurns == 0) {
                        STM.GetTarget().stunnedTurns = 1;
                        Debug.Log("target stunned: " + STM.GetTarget().stunnedTurns);
                        STM.GetTarget().ShockAnimation();
                    }
                    // StartCoroutine(DelayCardDeletion(cardDisplay));
                    break;
                case "STN_ALL":
                    foreach(var be in battleEnemies) {
                        if(be.antiStunTurns == 0) {
                            be.stunnedTurns = 1;
                            be.ShockAnimation();
                        }
                        // be.stunnedTurns += Int32.Parse(item.Value);
                    }
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
                    if(item.Value == "EXPAND" && ammoController.IsAmmoFull()) {
                        ammoController.ExpandSlots(1, true);
                    }
                    else {
                        hasReloaded = true;
                        ammoController.FullCharge();
                        if(isPowerSurge) {
                            handManager.DrawCards(1);
                            playerStats.addMana(1);
                        }
                        if(isPulseAmplifier)
                        {
                            DrawPulse();
                        }
                    }
                    playerStats.playerAnimator.ReloadAnimation(playerStats);
                    break;
                case "FINAL_BLOW":
                    List<string> finalBlow = card.actions["FINAL_BLOW"].Split(',').ToList();

                    // if (finalBlow.Count != 4) {
                    //     throw new Exception("Invalid FINAL_BLOW attributes! Must be 4 strings comma separated.");
                    // }

                    int dmg = Int32.Parse(finalBlow[0]) + playerStats.getAtkMod();
                    if(dmg < 0) {
                        dmg = 0;
                    }
                    dmg = WeakenedDamage(dmg, playerStats);
                    int multiplier = Int32.Parse(finalBlow[1]);

                    // unfortunate code copying, but refacting is too much work
                    for (int i = 0; i < multiplier; i++) {

                        Vector3 STMPos2;
                        isCharacterMissing = DidTargetMiss(playerStats);
                        if(isCharacterMissing) {
                            Vector3 targPos = STM.GetTarget().transform.position;
                            STMPos2 = new Vector3(targPos.x, targPos.y + 100f, targPos.z);
                        }
                        else {
                            STMPos2 = STM.GetTarget().transform.position;
                        }

                        bool isLast = i == (multiplier - 1);
                        // playerStats.transform.parent.GetComponent<PlayerAnimator>().AttackAnimation();
                        
                        // weapon animations here
                        switch(cardType) {
                            case "Blaster":
                                StartCoroutine(BlasterAttack(STMPos2, 0.1f, card, true, isLast));
                                break;
                            default:
                                break;
                        }

                        if(!isCharacterMissing) {
                            if(STM.GetTarget() is BattleEnemyContainer) {
                                StartCoroutine(((BattleEnemyContainer)STM.GetTarget()).TakeDamage(dmg, attackDelay, isDeadReturnValue => {
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
                            }
                        }
                        // else {
                        //     // player is blind, attack self
                        //     ((PlayerStats)STM.GetTarget()).takeDamage(dmg);
                        // }

                        if (!isLast) {
                            yield return new WaitForSeconds(0.5f);
                        }
                    }
                    break;
                case "POWER":
                    playerStats.characterAnimator.CastAnimation();
                    if (item.Value == "POCK_GEN") {
                        AudioManager.Instance.PlayArcanePower();
                        ammoController.FullCharge();
                        isPocketGenerator = true;
                    }
                    else if(item.Value == "POWER_SURGE"){
                        AudioManager.Instance.PlayArcanePower();
                        isPowerSurge = true;
                    }
                    else if(item.Value == "PULSE_AMPLIFIER") {
                        AudioManager.Instance.PlayArcanePower();
                        isPulseAmplifier = true;
                    }
                    else if(item.Value == "FROST_WARD") {
                        playerStats.iceAura.Play();
                        AudioManager.Instance.PlayFreeze();
                        isFrostWard = true;
                    }
                    else if(item.Value == "FYORNS_RESOLVE") {
                        playerStats.iceAura.Play();
                        AudioManager.Instance.PlayFreeze();
                        isFyornsResolve = true;
                    }
                    else if(item.Value == "ICE_BARRICADE") {
                        playerStats.iceAura.Play();
                        AudioManager.Instance.PlayFreeze();
                        isIceBarricade = true;
                    }
                    break;
                case "CAST":
                    playerStats.playerAnimator.CastAnimation();
                    break;
                case "SCRY":
                    scryUISelector.isComplete = false;
                    handManager.Scry(Int32.Parse(item.Value));
                    yield return new WaitUntil(() => scryUISelector.isComplete);
                    yield return new WaitForSeconds(1f);
                    // isScryComplete = true;
                    Debug.Log("ending scry");
                    break;
                case "COUNTER": 
                    var nextAction = card.actions.ToList()[x+1];
                    playerStats.counterQueue.Push(nextAction);
                    playerStats.counterTypes.Push(cardType);
                    // now, skip the next action in the loop
                    x+=1;
                    break;
                case "WEAKEN":
                    playerStats.playerAnimator.CastAnimation();
                    STM.GetTarget().WeakenAnimation();
                    yield return new WaitForSeconds(0.3f);
                    AudioManager.Instance.PlayGlassBreak();
                    STM.GetTarget().weak += Int32.Parse(item.Value);
                    break;
                case "WEAKEN_ALL":
                    foreach(var be in battleEnemies) {
                        be.weak += Int32.Parse(item.Value);
                        // hail vfx here?
                    }
                    break;
                default:
                    break;
            }
        }
        // unlock the target
        STM.SetTarget(null);
        STM.targetLocked = false;
    }

    private void DrawPulse()
    {
        var pulse = Resources.Load<Card>("Cards_Special/Blaster/Pulse");
        var pulseInstance = Instantiate(pulse);
        handManager.deckCopy.AddCard(pulseInstance);
        handManager.DrawCards(1);
    }

    public void CreateActionFromPair(KeyValuePair<string, string> keyValuePair, string type, BattleEnemyContainer be) {
        Card newCard = ScriptableObject.CreateInstance<Card>();
        newCard.manaCost = 0;
        newCard.type = type;
        newCard.actions = new Dictionary<string, string>();
        newCard.actions[keyValuePair.Key] = keyValuePair.Value;
        STM.SetTarget(be);
        AudioManager.Instance.PlayCounter();
        playerStats.CounterAnimation();
        flashImage.Flash(0.2f, 0f, 0.3f, Color.black);
        StartCoroutine(CardAction(newCard));
    }

    // deprecated, moved to BaseCharacterInfo as CharacterShield()
    // private IEnumerator PlayerShieldSequence() {
    //     playerStats.playerAnimator.BlockAnimation();
    //     yield return new WaitForSeconds(0.5f);
    //     playerStats.shieldAnimator.StartForceField();
    // }

    private void HammerAttack(bool isLast) {
        StartCoroutine(HammerAttackTimed(isLast));
    }

    private IEnumerator HammerAttackTimed(bool isLast) {
        playerStats.HoldWeapon("Hammer");
        playerStats.transform.parent.GetComponent<PlayerAnimator>().HammerAttackAnimation();
        yield return new WaitForSeconds(hammerAttackTime);
        if(isLast) {
            playerStats.RemoveWeapon("Hammer");
        }
    }

    private IEnumerator BlasterAttack(Vector3 STMPos, float timeInterval, Card card, bool useCharge, bool isLast) {
        playerStats.HoldWeapon("Blaster");
        playerStats.transform.parent.GetComponent<PlayerAnimator>().BlasterAttackAnimation();
        yield return new WaitForSeconds(attackDelay);

        // reduce the charges in the ammo container
        if (isPocketGenerator && card.subType == "Shot") {
            Debug.Log("pocket generator strikes again!");
        }
        else if (!useCharge) {
            Debug.Log("don't use charge!");
        }
        else {
            Debug.Log("using charge for card type: " + card.type + ", name: " + card.name);
            ammoController.UseCharge(1);
        }

        Debug.Log(playerStats.transform.position.x);
        Vector3 startingPosition = new Vector3(playerStats.blasterHeld.transform.position.x + 0.1f, playerStats.blasterHeld.transform.position.y, playerStats.blasterHeld.transform.position.z);
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
        if(isLast) {
            playerStats.RemoveWeapon("Blaster");
        }
    }

    private void GenerateEnemyActions(List<BattleEnemyContainer> battleEnemies) {
        foreach (BattleEnemyContainer battleEnemy in battleEnemies) {

            // while(battleEnemy.actions.Length == 0) {
            //     // wait until it loads
            // }
            if(battleEnemy.actions.Count > 0){
                // if(battleEnemy.name == "ArmorBot") {
                //     if(battleEnemy.nextActionText.actions.ContainsKey("TAUNT")) {
                //         Card blockAction = Resources.Load<Card>("BLOCK_MEDIUM");
                //     }
                // }

                // next action should not be the same as last
                Card randomAction;
                // if(battleEnemy.battleEnemy.name == "KingBot" && isFirstEnemyAttack) {
                //     var laughCard = new Card();
                //     laughCard.actions = new Dictionary<string, string>();
                //     laughCard.actions.Add("LAUGH", "");
                //     randomAction = laughCard;
                //     isFirstEnemyAttack = false;
                // }
                // else {
                randomAction = battleEnemy.RandomAction();
                // }
                // pass the action back to the enemy to display
                string actionText = randomAction.name;
                // Dictionary<string, string> actions = new Dictionary<string, string>();
                battleEnemy.SetNextActionText(randomAction);
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
        for(int x = 0; x < enemyActions.Count; x++) {
        // foreach (Tuple<BattleEnemyContainer,Card> enemyActionPair in enemyActions) {
            if (!isGameOver) {

                var enemyActionPair = enemyActions[x];
                var id = enemyActions[x].Item1.gameObject.GetInstanceID().ToString();

                yield return new WaitForSecondsRealtime(0.75f);

                Card randomAction = enemyActionPair.Item2;
                BattleEnemyContainer battleEnemy = enemyActionPair.Item1;

                if (battleEnemy.stunnedTurns > 0) {
                    // play stun animation
                    if(battleEnemy.frozenTurn) {
                        AudioManager.Instance.PlayFrozen();
                    }
                    else {
                        battleEnemy.ShockAnimation();
                    }
                }
                else {
                    foreach(var item in randomAction.actions) {
                        switch(item.Key) {
                            case "ATK":
                                List<int> multiAttack = new List<int>();

                                int attack;
                                int multi;

                                multiAttack = item.Value.Split(',').Select(int.Parse).ToList();

                                // if (multiAttack.Count != 2) {
                                //     throw new Exception("Invalid ATK attributes! Must be 2 ints comma separated.");
                                // }

                                attack = multiAttack[0];
                                multi = multiAttack[1];

                                // float atkMod = 1.0f;

                                if(isFrostWard) {
                                    AudioManager.Instance.PlayFreeze();
                                    playerStats.addBlock(2);
                                    playerStats.shieldAnimator.StartForceField();
                                    yield return new WaitForSeconds(1.0f);
                                }

                                if(playerStats.counterQueue.Count() > 0) {
                                    while(playerStats.counterQueue.Count() > 0) {
                                        var counter = playerStats.counterQueue.Pop();
                                        var type = playerStats.counterTypes.Pop();
                                        CreateActionFromPair(counter, type, battleEnemy);
                                        // if(type == "Hammer") {
                                        //     yield return new WaitForSeconds(hammerAttackTime);
                                        // }
                                        // else if(type == "Blaster") {
                                        //     yield return new WaitForSeconds(blasterAttackTime);
                                        // }
                                        // else {
                                        //     yield return new WaitForSeconds(0.5f);
                                        // }
                                        yield return new WaitForSeconds(1.0f);

                                        // if enemy died or is last, then break
                                        if(enemyActions.Count <= x) {
                                            break;
                                        }
                                        var newId3 = enemyActions[x].Item1.gameObject.GetInstanceID().ToString();
                                        if(id != newId3) {
                                            // if enemy died, then break out and stopp attacking
                                            break;
                                        }
                                    }
                                }

                                // check if the enemy died, if it did go to the next
                                if(enemyActions.Count <= x) {
                                    break;
                                }
                                var newId = enemyActions[x].Item1.gameObject.GetInstanceID().ToString();
                                if(id != newId) {
                                    x-=1;
                                    continue;
                                }

                                if(battleEnemy.stunnedTurns == 0) {
                                    for (int i = 0; i < multi; i++) {
                                        // Vector3 STMPos;
                                        isCharacterMissing = DidTargetMiss(battleEnemy);
                                        // if(isCharacterMissing) {
                                        //     Vector3 targPos = playerStats.transform.position;
                                        //     STMPos = new Vector3(targPos.x, targPos.y + 100f, targPos.z);
                                        // }

                                        battleEnemy.characterAnimator.AttackAnimation();

                                        bool isLast = i == (multi - 1);

                                        if(!isCharacterMissing) {

                                            var atkDmg = attack + battleEnemy.getAtkMod();
                                            if(atkDmg < 0) {
                                                atkDmg = 0;
                                            }
                                            atkDmg = WeakenedDamage(atkDmg, battleEnemy);

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
                                        if(playerStats.isDead) {
                                            // should work since its not a coroutine to take damage
                                            isLast = true;
                                            i = multi;
                                        }

                                        if (!isLast) {
                                            yield return new WaitForSeconds(0.2f);
                                        }
                                    }
                                }


                                break;
                            case "ATK_RND":

                                List<int> randAttack = randomAction.actions["ATK_RND"].Split(',').Select(int.Parse).ToList();

                                if (randAttack.Count != 2) {
                                    throw new Exception("Invalid ATK_RND attributes! Must be 2 ints comma separated.");
                                }
                                else {

                                    if(isFrostWard) {
                                        AudioManager.Instance.PlayFreeze();
                                        playerStats.addBlock(2);
                                        playerStats.shieldAnimator.StartForceField();
                                        yield return new WaitForSeconds(1.0f);
                                    }

                                    if(playerStats.counterQueue.Count() > 0) {
                                        while(playerStats.counterQueue.Count() > 0) {
                                            var counter = playerStats.counterQueue.Pop();
                                            var type = playerStats.counterTypes.Pop();
                                            CreateActionFromPair(counter, type, battleEnemy);
                                            // if(type == "Hammer") {
                                            //     yield return new WaitForSeconds(hammerAttackTime);
                                            // }
                                            // else if(type == "Blaster") {
                                            //     yield return new WaitForSeconds(blasterAttackTime);
                                            // }
                                            // else {
                                            //     yield return new WaitForSeconds(0.5f);
                                            // }
                                            yield return new WaitForSeconds(1.0f);

                                            // if enemy died or is last, then break
                                            if(enemyActions.Count <= x) {
                                                break;
                                            }
                                            var newId3 = enemyActions[x].Item1.gameObject.GetInstanceID().ToString();
                                            if(id != newId3) {
                                                // if enemy died, then break out and stopp attacking
                                                break;
                                            }
                                        }
                                    }

                                    // check if the enemy died, if it did go to the next
                                    if(enemyActions.Count <= x) {
                                        break;
                                    }
                                    var newId2 = enemyActions[x].Item1.gameObject.GetInstanceID().ToString();
                                    if(id != newId2) {
                                        x-=1;
                                        continue;
                                    }

                                    if(battleEnemy.stunnedTurns == 0) {


                                        int atkDmg = UnityEngine.Random.Range(randAttack[0], randAttack[1] + 1);
                                        atkDmg += battleEnemy.getAtkMod();
                                        if(atkDmg < 0) {
                                            atkDmg = 0;
                                        }
                                        atkDmg = WeakenedDamage(atkDmg, battleEnemy);
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
                                        isCharacterMissing = DidTargetMiss(battleEnemy);
                                        if(!isCharacterMissing) {
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
                                    }
                                }
                                break;
                            case "DEF_RND":
                                List<int> randBlock = randomAction.actions["DEF_RND"].Split(',').Select(int.Parse).ToList();

                                if (randBlock.Count != 2) {
                                    throw new Exception("Invalid DEF_RND attributes! Must be 2 ints comma separated.");
                                }
                                else {
                                    int block = UnityEngine.Random.Range(randBlock[0], randBlock[1] + 1);
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
                            case "TAUNT":
                                ((EnemyAnimator)battleEnemy.characterAnimator).TauntAnimation();
                                battleEnemy.isTaunter = true;
                                playerStats.TauntAnimation();
                                playerStats.tauntingEnemy = battleEnemy;
                                playerStats.tauntTurns += Int32.Parse(item.Value);
                                break;
                            case "HEAL":
                                battleEnemy.characterAnimator.CastAnimation();
                                battleEnemy.HealRandomTarget(Int32.Parse(item.Value));
                                break;
                            case "BLIND":
                                if(battleEnemy.battleEnemy.name == "KingBot") {
                                    ((EnemyAnimator)battleEnemy.characterAnimator).PointAnimation();
                                }
                                else {
                                    battleEnemy.characterAnimator.AttackAnimation();
                                }
                                playerStats.blind += 1;
                                break;
                            case "LAUGH":
                                ((EnemyAnimator)battleEnemy.characterAnimator).LaughAnimation();
                                break;
                            case "REMOVE_DEBUFF":
                                battleEnemy.characterAnimator.CastAnimation();
                                AudioManager.Instance.PlayArcanePower();
                                battleEnemy.frostStacks = 0;
                                battleEnemy.vuln = 0;
                                battleEnemy.weak = 0;
                                battleEnemy.blind = 0;
                                break;
                            case "ANTI_STUN":
                                battleEnemy.antiStunTurns += 2;
                                break;
                            case "VULN":
                                playerStats.VulnerableAnimation();
                                playerStats.AddVuln(1);
                                battleEnemy.characterAnimator.CastAnimation();
                                AudioManager.Instance.PlayGlassBreak();
                                break;
                            case "WEAKEN":
                                battleEnemy.characterAnimator.CastAnimation();
                                AudioManager.Instance.PlayGlassBreak();
                                playerStats.weak += Int32.Parse(item.Value);
                                playerStats.WeakenAnimation();
                                break;
                            default:
                                break;
                        }
                    }
                }

                yield return new WaitForSecondsRealtime(0.75f);
            }
        }

        StartCoroutine(EndEnemyTurn());
    }

    public int WeakenedDamage(int damage, BaseCharacterInfo attacker) {
        if(attacker.weak > 0) {
            return (int)Mathf.Round(damage * MainManager.Instance.weakenedModifier);
        }
        else return damage;
    }

    public void EndPlayerTurn() {
        AudioManager.Instance.PlayButtonPress();
        endTurnButton.interactable = false;
        isPlayerTurn = false;
        ResetEnemyShields();
        playerStats.RemoveSingleBlind();
        playerStats.RemoveSingleVuln();
        playerStats.RemoveSingleTaunt();
        playerStats.RemoveSingleWeak();
        if(playerStats.atkModTempTurns > 0) {
            playerStats.atkModTempTurns -= 1;
            if(playerStats.atkModTempTurns == 0) {
                playerStats.atkMod -= playerStats.atkModTemp;
                playerStats.atkModTemp = 0;
            }
        }
    }

    public IEnumerator EndEnemyTurn() {
        hasReloaded = false;
        enemyActions = new List<Tuple<BattleEnemyContainer, Card>>();
        areEnemyActionsDecided = false;
        handManager.DiscardHand();
        playerStats.resetMana();
        if(!MainManager.Instance.artifacts.Contains("DEF_PERSIST")) {
            playerStats.resetBlock();
        }
        else {
            playerStats.block -= playerStats.block/2;
        }
        // ok solution for now, need to wait for block off animation to finish
        // for situation with DEF_DRAW artifact
        yield return new WaitForSeconds(playerStats.shieldAnimator.stopTime);
        handManager.DrawCards(5);
        foreach(BattleEnemyContainer be in battleEnemies) {
            if(be.frozenTurn) {
                be.UnfreezeAnimation();
                be.frozenTurn = false;
            }
            be.RemoveSingleVuln();
            be.RemoveSingleBlind();
            be.RemoveSingleWeak();
            be.RemoveSingleAntiStun();
            if(be.atkModTempTurns > 0) {
                be.atkModTempTurns -= 1;
                if(be.atkModTempTurns == 0) {
                    be.atkMod -= be.atkModTemp;
                    be.atkModTemp = 0;
                }
            }
            be.stunnedTurns = 0;
        }
        endTurnButton.interactable = true;
    }

    public void ResetEnemyShields() {
        foreach(BattleEnemyContainer battleEnemy in BEM.GetBattleEnemies()) {
            battleEnemy.resetBlock();
        }
    }
}
