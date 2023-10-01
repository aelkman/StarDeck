using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageAnimation() {
        animator.SetTrigger("TakeDamage");
    }

    public void AttackAnimation() {
        animator.SetTrigger("Attack");
    }

    public void DeathAnimation() {
        animator.SetTrigger("Death");
    }

    public void BlockAnimation() {
        animator.SetTrigger("Blocking");
    }

    public void ShockAnimation() {
        animator.SetTrigger("Shock");
    }

    public void CastAnimation() {
        Debug.Log("enemy cast animation trigger!");
        animator.SetTrigger("Cast");
    }
}
