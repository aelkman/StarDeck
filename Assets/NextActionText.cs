using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NextActionText : MonoBehaviour
{
    public TextMeshPro nextActionText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetText(string text) {
        nextActionText.text = text;
    }
}
