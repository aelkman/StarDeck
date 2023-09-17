using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArrow : MonoBehaviour
{
    public MapNode currentNode;
    // Start is called before the first frame update

    void Start()
    {
        transform.parent.position = currentNode.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.position = currentNode.transform.position;
    }
}
