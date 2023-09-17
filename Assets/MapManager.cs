using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public GameObject mapArrow;
    private MapNode currentNode;
    public MapNode movementSelection;
    public Animator transition;
    // Start is called before the first frame update
    void Start()
    {
        if (transition == null) {
            transition = GameObject.Find("Crossfade").GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMovementSelection(int instanceId, MapNode newNode) {
        currentNode = mapArrow.GetComponent<MapArrow>().currentNode;
        if(currentNode.childrenNodes.Contains(instanceId)) {
            Debug.Log("nextNode match found!");
            mapArrow.GetComponent<MapArrow>().currentNode = newNode;
            LoadNextLevel();
        }
        else {
            Debug.Log("not a next node!");
        }
    }

    public void LoadNextLevel() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int sceneIndex) {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(sceneIndex);
    }

    private void OnSceneUnloaded(Scene current)
    {
        Debug.Log("OnSceneUnloaded: " + current);
    }
}
