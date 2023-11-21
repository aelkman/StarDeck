using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public GameObject mapArrow;
    public CanvasGroup destinationsCG;
    private MapNode currentNode;
    public MainManager mainManager;
    public MapNode movementSelection;
    public MapAudio mapAudio;
    public Animator transition;
    public GameObject diceContainer;
    public DiceRoller diceRoller;
    public ZoomScript zoomScript;
    public bool sceneTest;
    public string sceneNameOverride;
    public bool destinationsClickable = true;
    public List<string> eventScenes = new List<string>() { "Alleyway", "ATM", "NightMarket", "Dojo", "VendingMachines" }; 
    private static MapManager _instance;

    public static MapManager Instance 
    { 
        get { return _instance; } 
    } 

    void Awake()
    {
        if (_instance != null && _instance != this) 
        { 
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        eventScenes = new List<string>() { "Alleyway", "ATM", "NightMarket", "Dojo", "VendingMachines" };
    }

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

    public void SetMovementSelection(MapNode newNode) {
        currentNode = mapArrow.GetComponent<MapArrow>().currentNode;
        if(currentNode.childrenNodes.Contains(newNode)) {
            Debug.Log("nextNode match found!");
            mapAudio.PlayHitDestination();
            var go = Resources.Load<GameObject>("Map Destinations/Checkmark");
            Instantiate(go, mapArrow.GetComponent<MapArrow>().currentNode.transform);
            mapArrow.GetComponent<MapArrow>().currentNode = newNode;
            mainManager.currentNode = newNode;

            // move the scroll rect on click (if it's zoomed)
            // revisit later
            // var pos1 = zoomScript.GetComponent<RectTransform>().transform.InverseTransformPoint(zoomScript.GetComponent<RectTransform>().position);
            // var pos2 = zoomScript.GetComponent<RectTransform>().transform.InverseTransformPoint(newNode.GetComponent<RectTransform>().position);
            // var pos3 = pos1 - pos2;
            // var newPos = zoomScript.GetComponent<RectTransform>().transform.InverseTransformPoint(newNode.GetComponent<RectTransform>().localPosition);
            // zoomScript.LerpToPos(pos3);
           
            newNode.destination.gameObject.SetActive(false);
            LoadNextLevel(newNode.destinationName);
            // now, turn off and on proper animations
            foreach(var node in currentNode.childrenNodes) {
                var childNodeAnimator = node.destinationAnimator;
                childNodeAnimator.Rebind();
                childNodeAnimator.Update(0f);
                childNodeAnimator.enabled = false;
            }
            // now animate the new children
            foreach(var node in newNode.childrenNodes) {
                var childNodeAnimator = node.destinationAnimator;
                childNodeAnimator.enabled = true;
            }
        }
        else {
            Debug.Log("not a next node!");
        }
    }

    public void LoadNextLevel(string destinationName) {
        // for testing dice rolls
        // if(destinationName != "Chest" && destinationName != "Chest") {
        //     destinationName = "Unknown";
        // }
        if(sceneTest) {
            destinationName = sceneNameOverride;
        }

        if(destinationName == "Boss") {
            MainManager.Instance.isBossBattle = true;
            MainManager.Instance.HealPlayerPercent(0.3333);
        }
        if(destinationName == "Enemy" || destinationName == "Mini-Boss" || destinationName == "Boss") {
            destinationName = "Battle";
            StartCoroutine(LoadLevel(destinationName));
        }
        else if(destinationName == "Event") {
            var randIndex = Random.Range(0, eventScenes.Count);
            var randEvent = eventScenes[randIndex];
            eventScenes.Remove(randEvent);
            StartCoroutine(LoadLevel(randEvent));
        }
        else if(destinationName == "Unknown") {
            diceRoller.eventName = "Unknown";
            diceContainer.SetActive(true);
            destinationsClickable = false;
        }
        else if(destinationName == "ChestRoll") {
            diceRoller.eventName = "Chest";
            diceContainer.SetActive(true);
            destinationsClickable = false;
        }
        else {         
            StartCoroutine(LoadLevel(destinationName));
        }
    }

    IEnumerator LoadLevel(string sceneName) {
        transition = GameObject.Find("Crossfade").GetComponent<Animator>();
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1.0f);

        GameManager.Instance.LoadScene(sceneName);
    }

    private void OnSceneUnloaded(Scene current)
    {
        Debug.Log("OnSceneUnloaded: " + current);
    }
}
