using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiscardDisplay : MonoBehaviour
{
    public TextMeshPro discardText;
    public HandManager handManager;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        discardText.text = "Discard Pile:<br>" + handManager.GetDiscards().Count;
    }
}
