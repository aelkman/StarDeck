using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAudioController : MonoBehaviour
{
    public AudioSource negativeFeedback;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayNegativeFeedback() {
        negativeFeedback.Play();
    }
}
