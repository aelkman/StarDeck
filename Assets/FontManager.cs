using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FontManager : MonoBehaviour
{
    public TMP_FontAsset fontAsset;
 
    void Start() {
    

        foreach(TextMeshPro textMeshPro3D in GameObject.FindObjectsOfType<TextMeshPro>())
        {
            textMeshPro3D.font = fontAsset;
        }
        foreach(TextMeshProUGUI textMeshProUi in GameObject.FindObjectsOfType<TextMeshProUGUI>())
        {
            textMeshProUi.font = fontAsset;
        }
    }

    public TMP_FontAsset GetAsset() {
        return fontAsset;
    }
}
