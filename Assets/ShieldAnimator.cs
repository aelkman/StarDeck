using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAnimator : MonoBehaviour
{
    private Animator animator;
    private ParticleSystem shieldSystem;
    private float startTime = 0f;
    private float stopTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        shieldSystem = GetComponent<ParticleSystem>();
        RuntimeAnimatorController runtimeController = animator.runtimeAnimatorController;
        RuntimeAnimatorController newController = Instantiate(runtimeController);
        animator.runtimeAnimatorController = newController;
        UpdateAnimClipTimes();
    }

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            switch(clip.name)
            {
                case "ShieldOn":
                    startTime = clip.length;
                    break;
                case "ShieldOff":
                    stopTime = clip.length;
                    // Debug.Log("stopTime: " + stopTime);
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShieldOn() {
        animator.SetTrigger("ShieldOn");
    }

    private void ShieldOff() {
        animator.SetTrigger("ShieldOff");
    }

    private IEnumerator ShieldOffTimed() {
        ShieldOff();
        yield return new WaitForSeconds(stopTime);
        shieldSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void StartForceField() {
        if(shieldSystem.isStopped) {
            shieldSystem.Play();
            ShieldOn();
        }
    }

    public void StopForceField() {
        if (!shieldSystem.isStopped) {
            StartCoroutine(ShieldOffTimed());
        }
    }
}
