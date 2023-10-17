using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleWon : MonoBehaviour
{
    public PointsEarned pointsEarned;
    // public GameObject cardsButtons;
    // Start is called before the first frame update
    void Start()
    {
        // cardsButtons.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetLayerRecursively(GameObject obj, int layer) {
        obj.layer = layer;

        foreach (Transform child in obj.transform) {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public void Initiate() {
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        GameObject go = GameObject.Find("Card Canvas");
        SetLayerRecursively(go, LayerIgnoreRaycast);
        gameObject.SetActive(true);
        pointsEarned.gameObject.SetActive(true);
        pointsEarned.SetData();
    }
}
