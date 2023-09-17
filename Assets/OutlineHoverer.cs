using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineHoverer : MonoBehaviour
{
    private float fade = 0;
    private bool isGlowUp = true;
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private void OnMouseEnter() {
    //     Debug.Log("targetLocked: " + STM.targetLocked);
    //     if(!STM.targetLocked) {
    //         // add target to STM
    //         STM.SetTarget(transform.parent.GetComponent<BattleEnemyContainer>());
    //         Debug.Log("set target to SingleTargetManager!");
    //     }
    // }

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
        // STM.ClearTarget();
        // Debug.Log("cleared target to SingleTargetManager!");
        // if(!STM.targetLocked) {
        //     STM.SetTarget(null);
        // }
        fade = 0f;
        spriteRenderer.material.SetFloat("_Transparency", fade);
    }
}
