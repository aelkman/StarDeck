using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackAnimation() {
        Debug.Log("attack animation trigger!");
        animator.SetTrigger("Attack");
    }

    public void CastAnimation() {
        Debug.Log("enemy cast animation trigger!");
        animator.SetTrigger("Cast");
    }

    public IEnumerator TakeDamageAnimation(float delayTime) {
        yield return new WaitForSeconds(delayTime);
        animator.SetTrigger("TakeDamage");
    }

    public void DeathAnimation() {
        animator.SetTrigger("Death");
    }

    public void ShockAnimation() {
        animator.SetTrigger("Shock");
    }

    public void BlockAnimation() {
        animator.SetTrigger("Block");
    }

    public void GoldBot_Melee_1() {
        animator.SetTrigger("GoldBot_Melee1");
    }
}
