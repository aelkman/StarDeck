using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<int> childrenNodes;
    public List<int> parentNodes;
    // Start is called before the first frame update
    void Start()
    {
        childrenNodes = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
