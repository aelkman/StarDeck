using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleBackground : MonoBehaviour
{
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        var images = Resources.LoadAll<Sprite>("Battle Backgrounds");
        image.sprite = images[Random.Range(0, images.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
