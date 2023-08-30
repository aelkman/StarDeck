using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattle : MonoBehaviour
{
    private Material material;
    private float fade = 0;
    private bool isGlowUp = true;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
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
        material.SetFloat("_Transparency", fade);
    }

    private void OnMouseExit() {
        fade = 0f;
        material.SetFloat("_Transparency", fade);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
