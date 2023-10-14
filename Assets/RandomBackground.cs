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
        Object[] prefabs;
        if(MainManager.Instance.isBossBattle) {
            prefabs = Resources.LoadAll<Object>("Battle Backgrounds/Boss Backgrounds/Level " + MainManager.Instance.level);
        }
        else {
            prefabs = Resources.LoadAll<Object>("Battle Backgrounds/Level " + MainManager.Instance.level);
        }
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
