using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockText : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshPro blockText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void setText(string text) {
        blockText.text = text;
    }
}
