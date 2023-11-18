using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealNode : MonoBehaviour
{
    public MapNode parentNode;
    private Animator healAnimator;
    // Start is called before the first frame update
    void Start()
    {
        healAnimator = GetComponent<Animator>();
        healAnimator.Rebind();
        healAnimator.Update(0f);
        healAnimator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(MainManager.Instance.currentNode == parentNode) {
            healAnimator.enabled = true;
        }
    }
}
