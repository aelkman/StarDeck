using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleEnemyContainer : MonoBehaviour
{
    public BattleEnemy battleEnemy;
    public NextActionText nextActionText;
    public GameObject damagePrefab;
    private BattleEnemyManager battleEnemyManager;
    public ParticleSystem particleSystem;
    private GameObject singleTargetManagerGO;
    public SingleTargetManager singleTargetManager;
    public EnemySprite enemySprite;
    private Material material;
    public HealthBar healthBar;
    private Object[] actions;
    private int atkMod;
    private bool isTargeted = false;
    private int maxHealth;
    private int health;
    public int block = 0;
    public bool isDead = false;
    // Start is called before the first frame update

    void Start()
    {
        battleEnemyManager = transform.parent.parent.GetComponent<BattleEnemyManager>();
        actions = Resources.LoadAll("BattleEnemies/" + battleEnemy.name + "/Actions");
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
    //     Debug.Log("hovered enemy!");
    // }

    // void OnCollisionEnter2D(Collision2D other) {
    //     Debug.Log("hovered enemy!");
    // }

    public void TakeDamage(int damage) {
        if (block >= damage) {
            block -= damage;
            damage = 0;
        }
        else {
            damage -= block;
            block = 0;
        }
        health -= damage;
        StartCoroutine(particleDelay(0.2f));
        healthBar.SetHealth(health);
        GameObject damageTextInstance = Instantiate(damagePrefab, transform);
        damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();
        if (health <= 0) {
            // death animation here, disable the NextAction as well
            nextActionText.SetText("");
            transform.parent.GetComponent<EnemyAnimator>().DeathAnimation();
            battleEnemyManager.EnemyDeath(this);
            isDead = true;
        }
    }

    public IEnumerator particleDelay(float time) {
        yield return new WaitForSeconds(time);
        particleSystem.Play();
    }

    public int getMaxHealth() {
        return maxHealth;
    }

    public int getHealth() {
        return health;
    }

    public Card RandomAction() {
        // Random.seed = System.DateTime.Now.Millisecond;
        // Random.Range with ints is (inclusive, exclusive)
        return (Card)actions[Random.Range(0, actions.Length)];
    }

    public void SetNextActionText(string text) {
        nextActionText.SetText(text);
    }

    public void modifyAtk(int mod) {
        atkMod += mod;
        Debug.Log("atkMod modified: " + atkMod);
    }

    public int getAtkMod() {
        return atkMod;
    }
}
