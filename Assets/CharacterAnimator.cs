using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public Animator animator;
    public AudioSource shieldAudio;
    public AudioSource shockAudio;
    public BattleManager bem;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        bem = GameObject.Find("BattleManager").GetComponent<BattleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShieldAudioPlay() {
        shieldAudio.Play();
    }

    public void ShockAudioPlay() {
        shockAudio.Play();
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
        animator.SetTrigger("Block");
    }

    public void ShockAnimation() {
        animator.SetTrigger("Shock");
    }

    public void CastAnimation() {
        Debug.Log("enemy cast animation trigger!");
        animator.SetTrigger("Cast");
    }
}
