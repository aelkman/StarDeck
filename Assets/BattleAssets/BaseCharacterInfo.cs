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
    public int weak = 0;
    public int maxHealth;
    public int health;
    public int stunnedTurns = 0;
    public bool isDead = false;
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

    public int GetWeak() {
        return weak;
    }

    public void AddWeak(int weak) {
        this.weak += weak;
    }
}
