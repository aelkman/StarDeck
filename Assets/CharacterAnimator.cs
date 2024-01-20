using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public Animator animator;
    public AudioSource shieldAudio;
    public AudioSource shockAudio;
    public BattleManager bem;
    public UIAudio uiAudio;
    // Start is called before the first frame update
    void Start()
    {
        uiAudio = GameObject.Find("UIAudio").GetComponent<UIAudio>();
        animator = GetComponent<Animator>();
        bem = GameObject.Find("BattleManager").GetComponent<BattleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShieldAudioPlay() {
        AudioManager.Instance.PlayShield();
    }

    public void ShockAudioPlay() {
        shockAudio.Play();
    }

    public void DamageAnimation() {
        animator.SetTrigger("TakeDamage");
    }

    public void BlasterAttackAnimation() {
        animator.SetTrigger("Blaster");
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

    public void ShockTauntAnimation() {
        animator.SetTrigger("ShockTaunt");
    }

    public void CastAnimation() {
        // Debug.Log("enemy cast animation trigger!");
        animator.SetTrigger("Cast");
    }
}
