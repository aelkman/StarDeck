using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockPlayer : MonoBehaviour
{
    private ParticleSystem shockSystem;
    // Start is called before the first frame update
    void Start()
    {
        shockSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartShock() {
        shockSystem.Play();
    }
}
