using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : BaseCharacterInfo
{
    public Stats stats;
    public PlayerAnimator playerAnimator;
    public GameDataManager gdm;
    public BattleManager battleManager;
    public ManaBar manaBar;
    public ParticleSystem damageParticles;
    public GameObject forceField;
    public List<string> weapons;

    // Start is called before the first frame update
    void Start()
    {
        if (MainManager.Instance.playerHealth != null) {
            health = (int)MainManager.Instance.playerHealth;
        }
        else {
            health = stats.health;
        }
        maxHealth = stats.maxHealth;
        // for now, gameData is ONLY the Stats object
        weapons = new List<string>();
        shieldSystem = forceField.GetComponent<ParticleSystem>();
        shieldSystem.Stop();
        stats = gdm.gameData;
        healthBar.SetMaxHealth(stats.maxHealth);
        healthBar.SetHealth(health);
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

    public void addMana(int mana) {
        stats.mana += mana;
        manaBar.SetMana(stats.mana, stats.maxMana);
    }

    public void resetMana() {
        stats.mana = stats.maxMana;
        manaBar.SetMana(stats.mana, stats.maxMana);
    }

    public void takeDamage(int damage) {
        if (vuln > 0) {
            float extraDamage = (float)damage * 1.5f;
            damage = (int)Math.Round(extraDamage, 0);
            Debug.Log("vulnened, new damage: " + damage);
        }
        health -= damage;
        StartCoroutine(damageAnimation(.2f));
        healthBar.SetHealth(health);
        GameObject damageTextInstance = Instantiate(damageText, transform);
        damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();
        if (health <= 0) {
            isDead = true;
            transform.parent.GetComponent<PlayerAnimator>().DeathAnimation();
            StartCoroutine(DelayGameOver(2.0f));
        }
    }

    private IEnumerator DelayGameOver(float time) {
        yield return new WaitForSeconds(time);
        battleManager.GameOver();
    }

    public IEnumerator damageAnimation(float time) {
        yield return new WaitForSeconds(time);
        damageParticles.Play();
    }

}
