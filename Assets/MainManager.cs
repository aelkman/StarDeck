using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MainManager : MonoBehaviour, IDisposable
{
    // Start() and Update() methods deleted - we don't need them right now

    public static MainManager Instance;

    public int playerHealth = 50;
    public int playerMaxHealth = 50;
    public int coinCount = 50;
    public int level = 1;
    public int maxCharges = 3;
    public MapNode currentNode;
    public float vulnerableModifier = 1.5f;

    public List<string> artifacts = new List<string>();
    public List<Potion> potions = new List<Potion>();
    public List<string> possibleArtifacts;
    public Texture2D mouseTexture;
    public Texture2D mouseClickTex;
    private CursorMode cursorMode = CursorMode.ForceSoftware;
    private Vector2 hotSpot;
    public bool isBossBattle = false;

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

        var arr = Resources.LoadAll<Artifact>("Artifacts/").ToList();
        foreach(var artifact in arr) {
            possibleArtifacts.Add(artifact.codeName);
        }

        hotSpot = new Vector2(mouseTexture.width/2, mouseTexture.height/2);
        Cursor.SetCursor(mouseTexture, hotSpot, cursorMode);
    }

    public void AddArtifact(string codeName){
        if(codeName == "MARKS_MED") {
            vulnerableModifier = 1.75f;
        }
        if(codeName == "EXTRA_CHARGE") {
            maxCharges += 1;
        }
        artifacts.Add(codeName);
    }

    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            Cursor.SetCursor(mouseClickTex, hotSpot, cursorMode);
        }
        if(Input.GetMouseButtonUp(0)) {
            Cursor.SetCursor(mouseTexture, hotSpot, cursorMode);
        }
    }

    void OnDestroy() {
        Debug.Log("MainManager destroyed!");
    }

    public void Dispose() {
        Instance = null;
    }

    public void HealPlayer(double percent) {
        playerHealth += (int)(playerMaxHealth * percent);
        if(playerHealth > playerMaxHealth) {
            playerHealth = playerMaxHealth;
        }
    }
}
