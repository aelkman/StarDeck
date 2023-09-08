using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FontChanger : MonoBehaviour
{
    private FontManager fontManager;
    private TMP_FontAsset fontAsset;
    // Start is called before the first frame update
    void Start()
    {
        fontManager = GameObject.Find("FontManager").GetComponent<FontManager>();
        fontAsset = fontManager.GetAsset();

        foreach(TextMeshPro textMeshPro3D in GameObject.FindObjectsOfType<TextMeshPro>())
        {
            textMeshPro3D.font = fontAsset;
        }
        foreach(TextMeshProUGUI textMeshProUi in GameObject.FindObjectsOfType<TextMeshProUGUI>())
        {
            textMeshProUi.font = fontAsset;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
