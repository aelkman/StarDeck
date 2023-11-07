using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : CharacterAnimator
{
    public AudioSource attack1Audio;
    public AudioSource missAudio;
    public AudioSource deathAudio;
    public AudioSource castAudio;
    public AudioSource laughAudio;
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
    public void PlayAttack1Audio() {
        if(bem.isCharacterMissing) {
            missAudio.Play();
        }
        else {
            attack1Audio.Play();
        }
    }

    public void DeathAudioPlay() {
        deathAudio.Play();
    }

    public void CastAudioPlay() {
        castAudio.Play();
    }

    public void LaughAudioPlay() {
        laughAudio.Play();
    }

    public IEnumerator TakeDamageAnimation(float delayTime) {
        yield return new WaitForSeconds(delayTime);
        animator.SetTrigger("TakeDamage");
    }

    public void TakeDamageTaunting() {
        animator.SetTrigger("TakeDamageTaunt");
    }

    public void GoldBot_Melee_1() {
        animator.SetTrigger("GoldBot_Melee1");
    }

    public void TauntAnimation() {
        animator.SetTrigger("Taunt");
    }

    public void EndTauntAnimation() {
        animator.SetTrigger("EndTaunt");
    }

    public void LaughAnimation() {
        animator.SetTrigger("Laugh");
    }

    public void PointAnimation() {
        animator.SetTrigger("Point");
    }
}
