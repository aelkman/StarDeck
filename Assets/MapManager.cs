using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public GameObject mapArrow;
    private MapNode currentNode;
    public MainManager mainManager;
    public MapNode movementSelection;
    public Animator transition;
    public GameObject diceContainer;
    // Start is called before the first frame update
    void Start()
    {
        // if (transition == null) {
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMovementSelection(int instanceId, MapNode newNode) {
        currentNode = mapArrow.GetComponent<MapArrow>().currentNode;
        if(currentNode.childrenNodes.Contains(instanceId)) {
            Debug.Log("nextNode match found!");
            var go = Resources.Load<GameObject>("Map Destinations/Checkmark");
            Instantiate(go, mapArrow.GetComponent<MapArrow>().currentNode.transform);
            mapArrow.GetComponent<MapArrow>().currentNode = newNode;
            mainManager.currentNode = newNode;
            newNode.destination.gameObject.SetActive(false);
            if(newNode.destinationName == "Unknown") {
                diceContainer.SetActive(true);
            }
            else {
                LoadNextLevel(newNode.destinationName);
            }
        }
        else {
            Debug.Log("not a next node!");
        }
    }

    public void LoadNextLevel(string destinationName) {
        if(destinationName == "Enemy" || destinationName == "Mini-Boss") {
            destinationName = "Battle";
        }
        StartCoroutine(LoadLevel(destinationName));
    }

    IEnumerator LoadLevel(string sceneName) {
        transition = GameObject.Find("Crossfade").GetComponent<Animator>();
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneUnloaded(Scene current)
    {
        Debug.Log("OnSceneUnloaded: " + current);
    }
}
