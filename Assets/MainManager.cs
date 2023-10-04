using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now

    public static MainManager Instance;

    public int playerHealth = 50;
    public int playerMaxHealth = 50;
    public int coinCount = 50;
    public MapNode currentNode;
    public float vulnerableModifier = 1.5f;

    public List<string> artifacts = new List<string>();
    public List<string> possibleArtifacts;

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddArtifact(string codeName){
        if(codeName == "MARKS_MED") {
            vulnerableModifier = 1.75f;
        }
        artifacts.Add(codeName);
    }
}
