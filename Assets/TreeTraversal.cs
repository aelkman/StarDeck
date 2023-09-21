using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTraversal : MonoBehaviour
{
    public GameObject nextLevel;
    public GameObject treeLine;
    public TreeMapManager treeMapManager;
    public bool isFirstLevel = false;
    // Start is called before the first frame update
    void Start()
    {
        treeMapManager.AddChildren(transform.childCount);
        StartCoroutine(WaitForLoad());
    }

    private IEnumerator WaitForLoad() {
        yield return new WaitForSeconds(1.0f);
        if (isFirstLevel) {
            CreateTree();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateTree() {
        if (nextLevel != null && !treeMapManager.isDone) {
            for (int i = 0; i < transform.childCount; i++) {
                MapNode childMapNode = transform.GetChild(i).GetComponent<MapNode>();
                DFS(i);
                for(int j=0; j < nextLevel.transform.childCount; j++) {
                    Transform grandChild = nextLevel.transform.GetChild(j);
                    if (grandChild.GetComponent<MapNode>().parentNodes.Count == 0) {
                        childMapNode.childrenNodes.Add(grandChild.gameObject.GetInstanceID());
                        grandChild.GetComponent<MapNode>().parentNodes.Add(transform.GetChild(i).gameObject.GetInstanceID());

                        GameObject newLine = Instantiate(treeLine, transform.parent.transform);
                        LineRenderer lr = newLine.GetComponent<LineRenderer>();
                        lr.SetPosition(0, transform.GetChild(i).localPosition);
                        lr.SetPosition(1, grandChild.localPosition);
                        nextLevel.GetComponent<TreeTraversal>().DFS(j);
                        // nextLevel.GetComponent<TreeTraversal>().DFS(Random.Range(0, nextLevel.transform.childCount));
                    }
                    // if (childMapNode.childrenNodes.Count == 0) {
                    //     DFS(index);
                    // }

                    CheckIfComplete(childMapNode);
                    CheckIfComplete(grandChild.GetComponent<MapNode>());
                }
                CreateTree();
            }
        }
        // potentially unsafe timing issues
        
    }

    private void CheckIfComplete(MapNode childMapNode) {
        if(childMapNode.parentNodes.Count > 0 && childMapNode.childrenNodes.Count > 0) {
            if (!treeMapManager.treeMap.ContainsKey(childMapNode.instanceId)) {
                treeMapManager.treeMap.Add(childMapNode.instanceId, true);
                Debug.Log("added to the treeMap! count: " + treeMapManager.treeMap.Count);
            }
        }

        // first and last do not have both child & parent
        if (treeMapManager.treeMap.Count == treeMapManager.childCount - 2) {
            treeMapManager.isDone = true;
            Debug.Log("finished!");
        }
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
            }
            else {
                nextChildIndex = Random.Range(index, index + 2);
                if (nextLevel.transform.childCount - 1 < nextChildIndex) {
                    nextChildIndex -= 1;
                }
            }

            // this works, but it is too random, it makes it look very crazy
            // nextChildIndex = Random.Range(0, nextLevel.transform.childCount);
            
            // pseudo-random algo instead, for less complexity, 
            // need next level to be no more than 1 less or 1 more
            // int lowerBound = index - 1;
            // if (lowerBound < 0) { lowerBound = 0;}
            // int upperBound = index + 1;

            // List<int> values = new List<int>
            // {
            //     lowerBound,
            //     index,
            //     upperBound
            // };


            // for (int i = values.Count - 1; i > 0; i--) {
            //     int random = UnityEngine.Random.Range(lowerBound, upperBound + 1);
            //     Debug.Log("nextlevel childcount: " + nextLevel.transform.childCount);
            //     Debug.Log("random index: " + random);
            //     if(nextLevel.transform.childCount >= random + 1) {
            //         nextChildIndex = random;
            //         break;
            //     }
            //     else {
            //         values.Remove(random);
            //         i--;
            //     }
            // }
 
            MapNode childMapNode = nextLevel.transform.GetChild(nextChildIndex).GetComponent<MapNode>();

            if (!childMapNode.GetComponent<MapNode>().parentNodes.Contains(transform.GetChild(index).gameObject.GetInstanceID())) {
                childMapNode.GetComponent<MapNode>().parentNodes.Add(transform.GetChild(index).gameObject.GetInstanceID());
                transform.GetChild(index).GetComponent<MapNode>().childrenNodes.Add(nextLevel.transform.GetChild(nextChildIndex).gameObject.GetInstanceID());
                
                CheckIfComplete(childMapNode);
                CheckIfComplete(transform.GetChild(index).GetComponent<MapNode>());
                
                CreateLine(index, nextChildIndex);
            }





            nextLevel.GetComponent<TreeTraversal>().DFS(nextChildIndex);
        }
    }

    private void CreateLine(int i, int j) {
        Transform grandChild = nextLevel.transform.GetChild(j);
        GameObject newLine = Instantiate(treeLine, transform.parent.transform);
        LineRenderer lr = newLine.GetComponent<LineRenderer>();
        lr.SetPosition(0, transform.GetChild(i).localPosition);
        Transform nextMapNode = grandChild;
        lr.SetPosition(1, nextMapNode.localPosition);
    }
}
