using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RandomBackground : MonoBehaviour
{
    private BattleBackground randomPrefab;
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        // Load random background
        var prefabs = Resources.LoadAll<Object>("Battle Backgrounds");
        randomPrefab = (BattleBackground)prefabs[Random.Range(0, prefabs.Length)];
        foreach(Light2D light in randomPrefab.lights) {
            Instantiate(light);
        }
        spriteRenderer.sprite = randomPrefab.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
