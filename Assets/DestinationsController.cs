using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DestinationsController : MonoBehaviour
{
    public TreeMapManager treeMapManager;
    public List<string> destinations;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForLoad());
    }

    private IEnumerator WaitForLoad() {
        yield return new WaitForSeconds(0.25f);
        // weights: 
        // regular enemy spawn: 0 - 4
        // super enemy spawn:   5
        // unknown location :   6 - 8
        // shop:                9
        // event:               10
        // don't include first or last node, those are fixed enemy & boss

        // shops should be 10%
        // events should be 10%
        // enemies should be 45%
        // miniboss should be 15%
        // unkmown should be 10%
        // chest should be 10%
        int totalVisitableNodes = treeMapManager.childCount - 2;
        Debug.Log("total visitable nodes (excluding first and last): " + totalVisitableNodes);
        int tenPercent = (int)System.Math.Round((double)totalVisitableNodes/(double)10);
        for(int i = 0; i < 4; i++) {
            // get random count +/- 1
            int randomCount = Random.Range(tenPercent - 1, tenPercent + 2);
            
            for(int j = 0; j < randomCount; j++) {
                if(i == 0) {
                    destinations.Add("Shop");
                }
                if(i == 1) {
                    destinations.Add("Event");
                }
                if(i == 2) {
                    destinations.Add("Unknown");
                }
                if(i == 3) {
                    destinations.Add("Chest");
                }
            }
        }

        // check how many we have left now
        totalVisitableNodes -= destinations.Count;
        int fifteenPercent = (int)System.Math.Round(totalVisitableNodes * 0.25);
        for(int i = 0; i < fifteenPercent; i++) {
            destinations.Add("Mini-Boss");
        }

        totalVisitableNodes -= fifteenPercent;
        for(int i = 0; i < totalVisitableNodes; i++) {
            destinations.Add("Enemy");
        }

        // for (int i = 0; i < treeMapManager.childCount - 2; i++) {
        //     int randomIndex = Random.Range(0, 11);
        //     // regular enemy
        //     if (randomIndex >= 0 && randomIndex <= 3) {
        //         destinations.Add("Enemy");
        //     }
        //     else if (randomIndex == 4) {
        //         destinations.Add("Chest");
        //     }
        //     else if (randomIndex >= 5 && randomIndex <= 6) {
        //         destinations.Add("Mini-Boss");
        //     }
        //     else if (randomIndex >= 7 && randomIndex <= 8) {
        //         destinations.Add("Unknown");
        //     }
        //     else if (randomIndex == 9) {
        //         destinations.Add("Shop");
        //     }
        //     else if (randomIndex == 10) {
        //         destinations.Add("Event");
        //     }
        // }

        // now, assign the destinations to the GOs
    }

    public string AssignPopDestination(int level, MapNode node) {
        Debug.Log(node.name);
        var index = Random.Range(0, destinations.Count);
        string destination = destinations[index];
        if(level < 3) {
            while(destination == "Mini-Boss") {
                index = Random.Range(0, destinations.Count);
                destination = destinations[index];
            }
        }

        // sadly this doesnt work due to the undefined instantiation of all the nodes
        // will need to post-process the data later, this is something worth investigation another time
        
        // int tries = 0;
        // if(node.parentNodes.Where((node) => node.destinationName == "Mini-Boss").Count() > 0) {
        //     Debug.Log("parent was Mini-Boss! cannot be mini-boss");
        //     while(destination == "Mini-Boss") {
        //         index = Random.Range(0, destinations.Count);
        //         destination = destinations[index];
        //         tries += 1;
        //         if(tries > 100) {
        //             Debug.Log("edge case for destinations mini-boss reached!");
        //             destination = "Enemy";
        //         }
        //     }
        // }
        destinations.Remove(destination);
        return destination;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
