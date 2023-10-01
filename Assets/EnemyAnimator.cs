using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : CharacterAnimator
{
    // private Animator animator;
    // Start is called before the first frame update
    // void Start()
    // {
    //     animator = GetComponent<Animator>();
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    public IEnumerator TakeDamageAnimation(float delayTime) {
        yield return new WaitForSeconds(delayTime);
        animator.SetTrigger("TakeDamage");
    }

    public void GoldBot_Melee_1() {
        animator.SetTrigger("GoldBot_Melee1");
    }
}
