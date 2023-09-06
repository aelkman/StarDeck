using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnemyContainer : MonoBehaviour
{
    public BattleEnemy battleEnemy;
    public NextActionText nextActionText;
    private GameObject singleTargetManagerGO;
    private SingleTargetManager singleTargetManager;
    private Material material;
    public GameObject healthBarPrefab;
    private GameObject healthBarGO;
    private HealthBar healthBar;
    private Object[] actions;
    private int atkMod;
    private float fade = 0;
    private bool isGlowUp = true;
    private bool isTargeted = false;
    private int maxHealth;
    private int health;
    private Sprite sprite;
    private SpriteRenderer spriteRenderer;
    public bool isDead = false;
    // Start is called before the first frame update

    void Start()
    {
        actions = Resources.LoadAll("BattleEnemies/" + battleEnemy.name + "/Actions");
        singleTargetManagerGO = GameObject.Find("SingleTargetManager");
        singleTargetManager = singleTargetManagerGO.GetComponent<SingleTargetManager>();
        healthBarGO = Instantiate(healthBarPrefab);
        healthBarGO.transform.SetParent(this.transform, false);
        // healthBarGO.transform.localPosition = new Vector3(0,-3.57f,0);
        healthBar = healthBarGO.GetComponent<HealthBar>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = battleEnemy.sprite;
        spriteRenderer.material = battleEnemy.material;
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

    private void OnMouseEnter() {
        // add target to STM
        singleTargetManager.SetTarget(this);
        Debug.Log("set target to SingleTargetManager!");
    }

    private void OnMouseOver() {
        
        if (isGlowUp) {
            fade += Time.deltaTime * 2f;
        }
        else {
            fade -= Time.deltaTime * 2f;
        }
        if (fade >= 1f) { 
            isGlowUp = false;
        }
        else if (fade <= 0f) {
            isGlowUp = true;
        }
        spriteRenderer.material.SetFloat("_Transparency", fade);
    }

    private void OnMouseExit() {
        // remove target from STM
        singleTargetManager.ClearTarget();
        Debug.Log("cleared target to SingleTargetManager!");
        
        fade = 0f;
        spriteRenderer.material.SetFloat("_Transparency", fade);
    }

    public void TakeDamage(int damage) {
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0) {
            isDead = true;
        }
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
