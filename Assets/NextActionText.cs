using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NextActionText : MonoBehaviour
{
    public TextMeshPro nextActionText;
    public Dictionary<string, string> actions;
    public Card card;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetText(string text, Dictionary<string, string> actions, Card card) {
        this.actions = actions;
        this.card = card;
        nextActionText.text = text;
    }
}
