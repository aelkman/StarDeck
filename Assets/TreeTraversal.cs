using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTraversal : MonoBehaviour
{
    public GameObject nextLevel;
    public GameObject treeLine;
    public bool isFirstLevel = false;
    // Start is called before the first frame update
    void Start()
    {
        if (isFirstLevel) {
            CreateTree(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateTree(int index) {
        Node childNode = transform.GetChild(index).GetComponent<Node>();
        for(int j=0; j < nextLevel.transform.childCount; j++) {
            Transform grandChild = nextLevel.transform.GetChild(j);
            if (grandChild.GetComponent<Node>().parentNodes.Count == 0) {
                childNode.childrenNodes.Add(j);
                grandChild.GetComponent<Node>().parentNodes.Add(index);
                nextLevel.GetComponent<TreeTraversal>().DFS(j);

                GameObject newLine = Instantiate(treeLine, transform.parent.parent.transform);
                LineRenderer lr = newLine.GetComponent<LineRenderer>();
                lr.SetPosition(0, transform.GetChild(0).position);
                lr.SetPosition(1, grandChild.position);
                nextLevel.GetComponent<TreeTraversal>().DFS(Random.Range(0, nextLevel.transform.childCount));
                nextLevel.GetComponent<TreeTraversal>().CreateTree(Random.Range(0, nextLevel.transform.childCount));
            }
        }
        // potentially unsafe timing issues
        
    }

    private void SetChildParentCreateLine() {
        
    }

    private void DFS(int index) {
        if (nextLevel != null) {
            int nextChildIndex = 0;
            // if next level has more children, then first & last can choose any of 2 options
            if (nextLevel.transform.childCount <= transform.childCount) {
                nextChildIndex = Random.Range(index, index + 2);
                if (index == 0 || nextLevel.transform.childCount == 1) {
                    nextChildIndex = 0;
                }
                else if (index >= nextLevel.transform.childCount - 1) {
                    nextChildIndex = nextLevel.transform.childCount - 1;
                }
                else {
                    if (nextChildIndex >= nextLevel.transform.childCount) {
                        nextChildIndex = index;
                    }
                }
                Node childNode = nextLevel.transform.GetChild(nextChildIndex).GetComponent<Node>();
                childNode.GetComponent<Node>().parentNodes.Add(nextChildIndex);
                transform.GetChild(nextChildIndex).GetComponent<Node>().childrenNodes.Add(nextChildIndex);
            }
            else {
                nextChildIndex = Random.Range(index, index + 2);
                if (nextLevel.transform.childCount - 1 < nextChildIndex) {
                    nextChildIndex -= 1;
                }
                Node childNode = nextLevel.transform.GetChild(nextChildIndex).GetComponent<Node>();
                childNode.GetComponent<Node>().parentNodes.Add(index);
                transform.GetChild(index).GetComponent<Node>().childrenNodes.Add(nextChildIndex);
            }

            CreateLine(index, nextChildIndex);

            nextLevel.GetComponent<TreeTraversal>().DFS(nextChildIndex);
        }
    }

    private void CreateLine(int i, int j) {
        Transform grandChild = nextLevel.transform.GetChild(j);
        GameObject newLine = Instantiate(treeLine, transform.parent.parent.transform);
        LineRenderer lr = newLine.GetComponent<LineRenderer>();
        lr.SetPosition(0, transform.GetChild(i).position);
        Transform nextNode = grandChild;
        lr.SetPosition(1, nextNode.position);
    }
}
