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
    public ManaBar manaBar;
    public ParticleSystem damageParticles;
    public ParticleSystem counterParticles;
    public ParticleSystem iceAura;
    public GameObject forceField;
    public List<string> weapons;
    public GameObject blasterHeld;
    public GameObject blasterHip;
    public GameObject hammerHeld;
    public GameObject hammerHip;
    public BattleEnemyContainer tauntingEnemy;
    public int tauntTurns = 0;
    public CameraShake cameraShake;

    // Start is called before the first frame update
    void Start()
    {
        if(MainManager.Instance.artifacts.Contains("PENNY_PINCH")) {
            if(MainManager.Instance.coinCount < 100) {
                atkMod += 2;
            }
            else {
                atkMod -= 1;
            }
        }

        counterQueue = new QueueList<KeyValuePair<string, string>>();
        counterTypes = new QueueList<string>();
        cameraShake = GameObject.Find("ShakeHolder").GetComponent<CameraShake>();
        RemoveWeapon("Blaster");
        RemoveWeapon("Hammer");
        nextMoveYOffset = 0;
        if (MainManager.Instance != null) {
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
        if(MainManager.Instance.artifacts.Contains("MANA_VULN")) {
            stats.mana += 1;
            vuln += 2;
        }
        healthBar.SetMaxHealth(stats.maxHealth);
        healthBar.SetHealth(health);
        manaBar.SetMana(stats.mana, stats.maxMana);
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

    public void takeDamage(int damage, BattleEnemy be) {
        cameraShake.StartShake();
        damage = CalculateDamage(damage);
        health -= damage;
        if (damage > 0) {
            battleManager.noDamageTaken = false;
        }
        MainManager.Instance.playerHealth -= damage;
        StartCoroutine(damageAnimation(.2f, be));
        // healthBar.SetHealth(health);
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

    public IEnumerator damageAnimation(float time, BattleEnemy be) {
        yield return new WaitForSeconds(time);
        if(be != null) {
            var effect = Instantiate(be.attackEffect.prefab, transform);
            effect.transform.localScale = be.attackEffect.scale;
            foreach(Transform child in effect.transform) {
                child.localScale = be.attackEffect.scale;
            }
        }
        damageParticles.Play();
    }

    public void HoldWeapon(string weaponType) {
        if(weaponType == "Blaster") {
            blasterHip.SetActive(false);
            blasterHeld.SetActive(true);
        }
        else if(weaponType == "Hammer") {
            hammerHip.SetActive(false);
            hammerHeld.SetActive(true);
        }
    }

    public void RemoveWeapon(string weapon) {
        if(weapon == "Blaster") {
            blasterHip.SetActive(true);
            blasterHeld.SetActive(false);
        }
        else if(weapon == "Hammer") {
            hammerHip.SetActive(true);
            hammerHeld.SetActive(false);
        }

    }

    public void RemoveSingleTaunt() {
        if(tauntTurns == 1) {
            tauntingEnemy.isTaunter = false;
            ((EnemyAnimator)tauntingEnemy.characterAnimator).EndTauntAnimation();
        }
        if(tauntTurns >= 1) {
            tauntTurns -= 1;
        }
    }

    public void HealSelf(int heal) {
        GameObject healTextInstance = Instantiate(healPrefab, transform.position, Quaternion.identity, transform);
        healTextInstance.transform.localPosition = new Vector3(healTextInstance.transform.localPosition.x, healTextInstance.transform.localPosition.y, healTextInstance.transform.localPosition.z);
        healTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().text = heal.ToString();  
        MainManager.Instance.HealPlayer(heal);
    }

}
