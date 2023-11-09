using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClonerScript : MonoBehaviour
{
    private DeckViewer deckViewer;
    // Start is called before the first frame update
    void Start()
    {
        deckViewer = PersistentHUD.Instance.deckViewer;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ClonerStart() {
        deckViewer.StartCloner();
    }
}
