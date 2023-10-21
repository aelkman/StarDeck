using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterInfo : MonoBehaviour
{
    public ParticleSystem shieldSystem;
    public ParticleSystem iceSystem;
    public GameObject damageText;
    public ShieldAnimator shieldAnimator;
    public HealthBar healthBar;
    public GameObject swordPrefab;
    public GameObject vulnPrefab;
    public GameObject tauntPrefab;
    public ShockPlayer shockPlayer;
    public CharacterAnimator characterAnimator;
    public int block = 0;
    public int vuln = 0;
    public int blind = 0;
    public int maxHealth;
    public int health;
    public int stunnedTurns = 0;
    public bool isDead = false;
    public int atkMod;
    public int frostStacks;
    public float nextMoveYOffset;
    public bool isTaunter = false;
    public bool frozenTurn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Heal(int heal) {
        if(heal + health > maxHealth) {
            health = maxHealth;
        }
        else {
            health += heal;
        }
        healthBar.SetHealth(health);
    }

    public void addBlock(int block) {
        this.block += block;
        Debug.Log("block is: " + this.block);
    }

    public void DoubleBlock() {
        this.block *= 2;
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
        shieldAnimator.StopForceField();
    }

    public bool hasBlock() {
        if(block > 0) {
            return true;
        }
        else return false;
    }

    public int GetVuln() {
        return vuln;
    }

    public void AddVuln(int vuln) {
        this.vuln += vuln;
    }

    public void modifyAtk(int mod) {
        atkMod += mod;
        Debug.Log("atkMod modified: " + atkMod);
    }
    
    public int getAtkMod() {
        return atkMod;
    }

    public void RemoveSingleBlind() {
        if (blind > 0) {
            blind -= 1;
        }
    }

    public void SwordAnimation() {
        GameObject swordAnimationInsance = Instantiate(swordPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
        swordAnimationInsance.transform.localPosition = new Vector3(swordAnimationInsance.transform.localPosition.x, swordAnimationInsance.transform.localPosition.y + nextMoveYOffset, swordAnimationInsance.transform.localPosition.z);
    }

    public void VulnerableAnimation() {
        GameObject vulnAnimationInsance = Instantiate(vulnPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
        vulnAnimationInsance.transform.localPosition = new Vector3(vulnAnimationInsance.transform.localPosition.x, vulnAnimationInsance.transform.localPosition.y + nextMoveYOffset, vulnAnimationInsance.transform.localPosition.z);
    }

    public void TauntAnimation() {
        GameObject tauntAnimationInsance = Instantiate(tauntPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
        tauntAnimationInsance.transform.localPosition = new Vector3(tauntAnimationInsance.transform.localPosition.x, tauntAnimationInsance.transform.localPosition.y + nextMoveYOffset, tauntAnimationInsance.transform.localPosition.z);
    }

    public void RemoveSingleVuln() {
        if (vuln > 0) {
            vuln -= 1;
        }
    }

    public bool isBlind() {
        if(blind > 0) {
            return true;
        }
        else return false;
    }

    public int CalculateDamage(int damage) {
        if (vuln > 0) {
            float extraDamage = (float)damage * MainManager.Instance.vulnerableModifier;
            damage = (int)Math.Round(extraDamage, 0);
            Debug.Log("vulnerable, new damage: " + damage);
        }
        return damage;
    }

    public void ShockAnimation() {
        shockPlayer.StartShock();
        characterAnimator.ShockAnimation();
    }

    public void FreezeAnimation() {
        characterAnimator.GetComponent<Animator>().speed = 0;
    }

    public void UnfreezeAnimation() {
        characterAnimator.GetComponent<Animator>().speed = 1;
        iceSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void AddFrost(int frost) {
        for(int i = 0; i < frost; i++) {
            frostStacks += 1;
            if(frostStacks >= 3) {
                // delay reset on frost counter
                StartCoroutine(delayedFrostReset());
                // trigger freeze on enemy

            }
        }
    }

    private IEnumerator delayedFrostReset() {
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlayFreeze();
        FreezeAnimation();
        iceSystem.Clear();
        iceSystem.Play();
        stunnedTurns += 1;
        frozenTurn = true;
        frostStacks = 0;
    }
}
