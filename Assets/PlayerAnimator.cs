using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : CharacterAnimator
{
    private bool reloadFinished = false;
    public AudioSource reloadAudio;
    public AudioSource deathAudio;
    public AudioSource blasterAudio;
    public AudioSource hammerAudio;
    public PlayerStats playerStats;
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

    public void ReloadFinished()
    {
        reloadFinished = true;
        Debug.Log("reload animation finished!");
    }

    public void ReloadAudioPlay() {
        reloadAudio.Play();
    }

    public void DeathAudioPlay() {
        deathAudio.Play();
    }

    public void BlasterAudioPlay() {
        blasterAudio.Stop();
        blasterAudio.Play();
    }

    public void ClapAnimation() {
        animator.SetTrigger("Clap");
    }

    public void HipThrust() {
        animator.SetTrigger("HipThrust");
    }

    public void ReloadAnimation(PlayerStats ps) {
        StartCoroutine(ReloadTimed(ps));
    }

    private IEnumerator ReloadTimed(PlayerStats ps) {
        ps.HoldWeapon("Blaster");
        animator.SetTrigger("Reload");
        yield return new WaitUntil(() => reloadFinished);
        ps.RemoveWeapon("Blaster");
        reloadFinished = false;
    }

    public void DrinkPotionAnimation() {
        animator.SetTrigger("Drink");
    }

    public void DrinkPotionAudio() {
        uiAudio.PlayPotionAudio();
    }

    public void PlayHammerAudio() {
        hammerAudio.Play();
    }

    public void HammerAttackAnimation() {
        animator.SetTrigger("Hammer");
    }

    public void RemoveHammer() {
        playerStats.RemoveWeapon("Hammer");
    }
}
