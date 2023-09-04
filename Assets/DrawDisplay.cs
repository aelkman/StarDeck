using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrawDisplay : MonoBehaviour
{
    public HandManager handManager;
    public TextMeshPro drawText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        drawText.text = "Draw Pile: <br>" + handManager.deck.cardStack.Count.ToString();
    }


}
