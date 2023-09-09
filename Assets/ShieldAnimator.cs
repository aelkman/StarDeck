using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAnimator : MonoBehaviour
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

    public void ShieldOn() {
        animator.SetTrigger("ShieldOn");
    }

    public void ShieldOff() {
        animator.SetTrigger("ShieldOff");
    }
}
