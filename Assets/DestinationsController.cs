using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        for (int i = 0; i < treeMapManager.childCount - 2; i++) {
            int randomIndex = Random.Range(0, 11);
            // regular enemy
            if (randomIndex >= 0 && randomIndex <= 3) {
                destinations.Add("Enemy");
            }
            else if (randomIndex == 4) {
                destinations.Add("Chest");
            }
            else if (randomIndex >= 5 && randomIndex <= 6) {
                destinations.Add("MiniBoss");
            }
            else if (randomIndex >= 7 && randomIndex <= 8) {
                destinations.Add("Unknown");
            }
            else if (randomIndex == 9) {
                destinations.Add("Shop");
            }
            else if (randomIndex == 10) {
                destinations.Add("Event");
            }
        }

        // now, assign the destinations to the GOs
    }

    public string AssignPopDestination() {
        var index = Random.Range(0, destinations.Count);
        string destination = destinations[index];
        destinations.Remove(destination);
        return destination;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
