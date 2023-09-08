using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimator : MonoBehaviour
{
    private Animator animator;
    public float flipTime1 = 0f;
    public float flipTime2 = 0f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        UpdateAnimClipTimes();
    }

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            switch(clip.name)
            {
                case "CardRotation1":
                    flipTime1 = clip.length;
                    break;
                case "CardRotation2":
                    flipTime2 = clip.length;
                    break;
            }
        }
    }

    public void FlipAnimation1() {
        animator.SetTrigger("Flip1");
    }

    public void FlipAnimation2() {
        animator.SetTrigger("Flip2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
