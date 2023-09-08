using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public Stats stats;
    public GameDataManager gdm;
    public HealthBar healthBar;
    public BattleManager battleManager;
    public ManaBar manaBar;
    public ParticleSystem particleSystem;
    public GameObject forceField;
    public GameObject damageText;
    private int block = 0;
    private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        // for now, gameData is ONLY the Stats object
        forceField.SetActive(false);
        stats = gdm.gameData;
        healthBar.SetMaxHealth(stats.maxHealth);
        manaBar.SetMana(stats.maxMana, stats.mana);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void useMana(int manaCost) {
        stats.mana -= manaCost;
        manaBar.SetMana(stats.mana, stats.maxMana);
    }

    public void resetMana() {
        stats.mana = stats.maxMana;
        manaBar.SetMana(stats.mana, stats.maxMana);
    }

    public void takeDamage(int damage) {
        stats.health -= damage;
        StartCoroutine(particleDelay(.2f));
        healthBar.SetHealth(stats.health);
        GameObject damageTextInstance = Instantiate(damageText, transform);
        damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();
        if (stats.health <= 0) {
            isDead = true;
            transform.parent.GetComponent<PlayerAnimator>().DeathAnimation();
            StartCoroutine(DelayGameOver(2.0f));
        }
    }

    private IEnumerator DelayGameOver(float time) {
        yield return new WaitForSeconds(time);
        battleManager.GameOver();
    }

    public IEnumerator particleDelay(float time) {
        yield return new WaitForSeconds(time);
        particleSystem.Play();
    }

    public void StartForceField() {
        forceField.SetActive(true);
    }

    public void StopForceField() {
        StartCoroutine(ForceFieldOff());
    }

    public void addBlock(int block) {
        this.block += block;
        Debug.Log("block is: " + this.block);
    }

    public void setBlock(int block) {
        this.block = block;
        Debug.Log("block is: " + this.block);
    }

    public int getBlock() {
        return block;
    }

    public void resetBlock() {
        block = 0;
        StartCoroutine(ForceFieldOff());
    }

    private IEnumerator ForceFieldOff() {
        transform.parent.GetComponent<PlayerAnimator>().ForceFieldOff();
        yield return new WaitForSeconds(0.5f);
        forceField.SetActive(false);
    }

    public bool hasBlock() {
        if(block > 0) {
            return true;
        }
        else return false;
    }
}
