using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource buttonPress;
    public AudioSource cardRustling;
    public AudioSource heal;
    public AudioSource coins;
    public AudioSource blood;
    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonPress() {
        buttonPress.Stop();
        buttonPress.Play();
    }

    public void PlayCardRustling() {
        cardRustling.Play();
    }

    public void PlayHeal() {
        heal.Play();
    }

    public void PlayCoins() {
        coins.Play();
    }

    public void PlayBlood() {
        blood.Play();
    }
}
