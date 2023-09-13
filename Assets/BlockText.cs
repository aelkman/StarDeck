using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockText : MonoBehaviour
{
    public TextMeshPro blockText;
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

    public void setText(string text) {
        blockText.text = text;
    }

    public void BlockAnimation() {
        animator.SetTrigger("BlockChange");
    }
}
