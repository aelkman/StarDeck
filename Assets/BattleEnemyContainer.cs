using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnemyContainer : MonoBehaviour
{
    public BattleEnemy battleEnemy;
    private Material material;
    private float fade = 0;
    private bool isGlowUp = true;
    private float maxHealth;
    private float health;
    private Sprite sprite;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update

    void Start()
    {
    }
    
    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("hovered enemy!");
    }

    void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("hovered enemy!");
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
        fade = 0f;
        spriteRenderer.material.SetFloat("_Transparency", fade);
    }

    public void Setup() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = battleEnemy.sprite;
        spriteRenderer.material = battleEnemy.material;
        maxHealth = battleEnemy.maxHealth;
        health = battleEnemy.health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
