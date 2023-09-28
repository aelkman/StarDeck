using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterInfo : MonoBehaviour
{
    public ParticleSystem shieldSystem;
    public GameObject damageText;
    public ShieldAnimator shieldAnimator;
    public HealthBar healthBar;
    public int block = 0;
    public int vuln = 0;
    public int maxHealth;
    public int health;
    public int stunnedTurns = 0;
    public bool isDead = false;
    public int atkMod;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
