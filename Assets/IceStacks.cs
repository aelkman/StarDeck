using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStacks : MonoBehaviour
{
    // public GameObject stack_1;
    // public GameObject stack_2;
    // public GameObject stack_3;
    public List<GameObject> stacksList;
    public BaseCharacterInfo baseCharacterInfo;

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnEnable() {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetStacks(int stacks) {
        for(int i = 0; i < stacks; i++) {
            stacksList[i].SetActive(true);
        }
    }

    public void RemoveStacks() {
        foreach(var go in stacksList) {
            go.SetActive(false);
        }
    }
}
