using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using System.Linq;

public class BattleEnemyContainer : BaseCharacterInfo
{
    public BattleEnemy battleEnemy;
    public NextActionText nextActionText;
    public GameObject characterHUD;
    public GameObject nextAction;
    public GameObject damagePrefab;
    public ParticleSystem effectSystem;
    private GameObject singleTargetManagerGO;
    public SingleTargetManager singleTargetManager;
    public GameObject enemyPrefabInstance;
    public GameObject kingbotWinDialogue;
    public List<Card> actions;
    private EnemyAnimator enemyAnimator;
    public Animator selectorAnimator;
    public CameraShake cameraShake;
    public AttackPrefabsController APC;
    // Start is called before the first frame update
    void Start()
    {
        var pos = selectorAnimator.transform.localPosition;
        selectorAnimator.transform.localPosition = new Vector3(pos.x + 70, pos.y + battleEnemy.nextMoveYOffset + 150, pos.z);
        selectorAnimator.gameObject.SetActive(false);
        counterQueue = new QueueList<KeyValuePair<string, string>>();
        counterTypes = new QueueList<string>();
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        BEM = transform.parent.parent.GetComponent<BattleEnemyManager>();
            // iceSystem.transform.localPosition = new Vector3(iceSystem.transform.localPosition.x, iceSystem.transform.localPosition.y - battleEnemy.yOffset, iceSystem.transform.localPosition.z);
        cameraShake = GameObject.Find("ShakeHolder").GetComponent<CameraShake>();
        nextMoveYOffset = battleEnemy.nextMoveYOffset;
        enemyAnimator = transform.parent.GetComponent<EnemyAnimator>();
        shieldSystem.Stop();
        actions = Resources.LoadAll<Card>("BattleEnemies/BattleEnemyActions/" + battleEnemy.name).ToList();
        // actions = battleEnemy.actions;
        singleTargetManagerGO = GameObject.Find("SingleTargetManager");
        singleTargetManager = singleTargetManagerGO.GetComponent<SingleTargetManager>();
        // healthBarGO = Instantiate(healthBarPrefab);
        // healthBarGO.transform.SetParent(this.transform, false);
        // healthBarGO.transform.localPosition = new Vector3(0,-3.57f,0);
        // healthBar = healthBarGO.GetComponent<HealthBar>();


        maxHealth = battleEnemy.maxHealth;
        health = battleEnemy.health;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update () {
    }
    
    // void OnTriggerEnter2D(Collider2D other) {
    //     // Debug.Log("hovered enemy!");
    // }

    // void OnCollisionEnter2D(Collision2D other) {
    //     // Debug.Log("hovered enemy!");
    // }

    

    public IEnumerator TakeDamage(int damage, float timeDelay,  string type, System.Action<bool> isDeadCallback) {
        yield return new WaitForSeconds(timeDelay);
        damage = CalculateDamage(damage);
        if(type == "Blaster_All") {
            type = "Blaster";
        }
        GameManager.Instance.weaponDamage[type] += damage;
        // add steamworks stats
        // Debug.Log(type + " damage: " + GameManager.Instance.weaponDamage[type]);
        cameraShake.StartShake();
        if (block >= damage) {
            block -= damage;
            if (block == 0) {
                resetBlock();
            }
            damage = 0;
        }
        else {
            damage -= block;
            resetBlock();
        }
        health -= damage;
        StartCoroutine(particleDelay(0f));
        // healthBar.SetHealth(health);
        GameObject damageTextInstance = Instantiate(damagePrefab, transform);
        damageTextInstance.transform.localPosition = new Vector3(damageTextInstance.transform.localPosition.x, damageTextInstance.transform.localPosition.y + battleEnemy.nextMoveYOffset, damageTextInstance.transform.localPosition.z);
        damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();
        if (health <= 0) {
            // death animation here, disable the NextAction as well
            nextActionText.SetText(null);
            // if(frozenTurn) {
            //     characterHUD.GetComponent<CharacterHUD>().iceHUD.SetActive(false);
            //     UnfreezeAnimation();
            // }
            // new ice approach for bug
            characterHUD.GetComponent<CharacterHUD>().iceHUD.SetActive(false);
            iceSystem.Clear();

            if(battleEnemy.name == "Chest") {
                Facepunch.Instance.TriggerAchievement("ACH_MIMIC");
            }
            enemyAnimator.DeathAnimation();
            enemyPrefabInstance.GetComponent<BoxCollider2D>().enabled = false;
            if(enemyPrefabInstance.GetComponentInChildren<ShadowCaster2D>() != null) {
                enemyPrefabInstance.GetComponentInChildren<ShadowCaster2D>().enabled = false;
            }
            var sprites = enemyPrefabInstance.GetComponentsInChildren<SpriteRenderer>();
            if(!battleEnemy.isBoss) {
                BEM.EnemyDeath(this);
                foreach(var sprite in sprites) {
                    StartCoroutine(FadeEnemyDeath(sprite));
                }
            }
            else {
                battleManager.isBattleWon = true;
                Facepunch.Instance.TriggerAchievement("ACH_BEAT_DEMO");
                battleManager.RemoveEnemyActions(this);
                this.nextAction.SetActive(false);
                // this.characterHUD.SetActive(false);
                StartCoroutine(battleManager.StartBossWinDialogue(this));
            }

            isDead = true;
            isDeadCallback(true);
        }
        else {
            if(isTaunter) {
                enemyAnimator.TakeDamageTaunting();
            }
            else {
                StartCoroutine(enemyAnimator.TakeDamageAnimation(0f));
            }
            isDeadCallback(false);
        }

        if(type == "Blaster") {
            Steamworks.SteamUserStats.AddStat( "BLASTER_DAMAGE", damage );
            Steamworks.SteamUserStats.StoreStats();

            // var blasterDmg = Steamworks.SteamUserStats.GetStatInt( "BLASTER_DAMAGE" );
            // if(blasterDmg >= 200 && blasterDmg < 500) {
            //     Facepunch.Instance.TriggerAchievement("ACH_BLASTER_NOVICE");
            // }
            // else if(blasterDmg >= 500 && blasterDmg < 1000) {
            //     Facepunch.Instance.TriggerAchievement("ACH_BLASTER_TRAINED");
            // }
            // else if(blasterDmg >= 1000) {
            //     Facepunch.Instance.TriggerAchievement("ACH_BLASTER_EXPERT");
            // }

        }
        else if(type == "Hammer") {
            Steamworks.SteamUserStats.AddStat( "HAMMER_DAMAGE", damage );
            Steamworks.SteamUserStats.StoreStats();
            AddFrost(1, 0.0f, false);

            // var hammerDmg = Steamworks.SteamUserStats.GetStatInt( "HAMMER_DAMAGE" );
            // if(hammerDmg >= 200 && hammerDmg < 500) {
            //     Facepunch.Instance.TriggerAchievement("ACH_HAMMER_NOVICE");
            // }
            // else if(hammerDmg >= 500 && hammerDmg < 1000) {
            //     Facepunch.Instance.TriggerAchievement("ACH_HAMMER_TRAINED");
            // }
            // else if(hammerDmg >= 1000) {
            //     Facepunch.Instance.TriggerAchievement("ACH_HAMMER_EXPERT");
            // }
        }
    }

    public IEnumerator FadeEnemyDeath(SpriteRenderer sr) {
        float timeInterval = 0.0165f;
        for(float i = 0; i < 2f; i+= timeInterval) {
            var color = new Color(1f, 1f, 1f, Mathf.Lerp(1, 0, i/2f));
            sr.color = color;
            yield return new WaitForSeconds(timeInterval);
        }
    }

    public IEnumerator particleDelay(float time) {
        yield return new WaitForSeconds(time);
        effectSystem.Play();
    }

    public int getMaxHealth() {
        return maxHealth;
    }

    public int getHealth() {
        return health;
    }

    public List<BattleEnemyContainer> GetDamagedEnemies() {
        List<BattleEnemyContainer> damagedEnemies = new List<BattleEnemyContainer>();
        foreach(var be in BEM.GetBattleEnemies()) {
            if(be.health > 0 && be.health < be.maxHealth) {
                damagedEnemies.Add(be);
            }
        }
        return damagedEnemies;
    }

    public Card RandomAction() {
        // Random.seed = System.DateTime.Now.Millisecond;
        // Random.Range with ints is (inclusive, exclusive)

        // remove the nextAction (which is last action right now)
        var possibleActions = new List<Card>(actions);

        // if ArmorBot is alone, he can attack
        if(battleEnemy.name == "ArmorBot") {
            if(BEM.GetBattleEnemies().Count == 1) {
                possibleActions.RemoveAll(x => x.actions.ContainsKey("TAUNT"));
                var attackCard = new Card();
                attackCard.actions = new Dictionary<string, string>();
                attackCard.actions.Add("ATK_RND", "8, 20");
                possibleActions.Add(attackCard);
            }
        }
        possibleActions.Remove(nextActionText.card);

        Card nextCard = (Card)possibleActions[Random.Range(0, possibleActions.Count)];
        if(nextCard.actions.ContainsKey("HEAL")) {
            if(GetDamagedEnemies().Count < 1) {
                possibleActions.Remove(nextCard);
                nextCard = (Card)possibleActions[Random.Range(0, possibleActions.Count)];
            }
        }
        return nextCard;
    }

    public void SetNextActionText(Card card) {
        nextActionText.SetText(card);
    }

    public void BlockSequence(int block) {
        enemyAnimator.BlockAnimation();
        shieldAnimator.StartForceField();
        addBlock(block);
    }

    public void HealRandomTarget(int heal) {
        // pick random damaged enemy to heal
        var damagedEnemies = GetDamagedEnemies();
        if(damagedEnemies.Count > 0) {
            var randIndex = Random.Range(0, damagedEnemies.Count);
            var target = damagedEnemies[randIndex];
            GameObject healTextInstance = Instantiate(healPrefab, target.transform.position, Quaternion.identity, target.transform);
            healTextInstance.transform.localPosition = new Vector3(healTextInstance.transform.localPosition.x, healTextInstance.transform.localPosition.y + target.battleEnemy.nextMoveYOffset, healTextInstance.transform.localPosition.z);
            healTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().text = heal.ToString();  
            target.Heal(heal);
        }
        else {
            // shrug animation
        }
    }
}
