using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMapManager : MonoBehaviour
{
    public Dictionary<int, bool> treeMap;
    public int childCount;
    public bool isDone = false;
    // Start is called before the first frame update
    void Start()
    {
        treeMap = new Dictionary<int, bool>();
    }

    public void AddChildren(int children) {
        childCount += children;
        // Debug.Log("child count: " + childCount);
    }
}
