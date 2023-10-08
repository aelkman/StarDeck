using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : CharacterAnimator
{
    private bool reloadFinished = false;
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
        ps.HoldWeapon();
        animator.SetTrigger("Reload");
        yield return new WaitUntil(() => reloadFinished);
        ps.RemoveWeapon();
        reloadFinished = false;
    }
}
