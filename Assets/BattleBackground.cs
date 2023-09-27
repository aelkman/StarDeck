using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "New Battle Background", menuName = "Battle Background")]
public class BattleBackground : ScriptableObject
{
    public Sprite sprite;
    // private BattleBackground randomPrefab;
    public List<Light2D> lights;
    // Start is called before the first frame update
    void Start()
    {


        // var lights = Resources.LoadAll<Light2D>("Battle Backgrounds");
        // spriteRenderer.sprite = images[Random.Range(0, images.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
