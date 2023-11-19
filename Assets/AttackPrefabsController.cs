using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPrefabsController : MonoBehaviour
{
    public List<AttackEffect> hammerEffects;
    public List<AttackEffect> blasterEffects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateRandomHammer() {
        int rand = Random.Range(0, hammerEffects.Count);
        var attackEffect = hammerEffects[rand];
        var newEffect = Instantiate(attackEffect.prefab, transform);
        newEffect.transform.localScale = attackEffect.scale;
        foreach(Transform child in newEffect.transform) {
            child.localScale = attackEffect.scale;
        }
    }

    public void InstantiateRandomBlaster() {
        int rand = Random.Range(0, blasterEffects.Count);
        var attackEffect = blasterEffects[rand];
        var newEffect = Instantiate(attackEffect.prefab, transform);
        newEffect.transform.localScale = attackEffect.scale;
        foreach(Transform child in newEffect.transform) {
            child.localScale = attackEffect.scale;
        }
    }
}
