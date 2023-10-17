using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAudioController : MonoBehaviour
{
    public AudioSource negativeFeedback;
    public AudioSource buttonPress;
    public AudioSource coins;
    public AudioSource takeItem;
    public AudioSource heal;

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
