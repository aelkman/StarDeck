using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AlleywayCity : MonoBehaviour
{
    public Button option1;
    public Button option2;
    public Button option3;
    private Deck deck;
    public Animator crossfade;
    // Start is called before the first frame update
    void Start()
    {
        deck = GameObject.Find("Deck").GetComponent<Deck>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Option1Click() {
        if(MainManager.Instance.coinCount >= 100) {
            AudioManager.Instance.PlayCoins();
            MainManager.Instance.coinCount -= 100;
            option1.interactable = false;
            option2.interactable = false;
            option3.interactable = false;
            Leave();
        }
    }

    public void Option2Click() {
        AudioManager.Instance.PlayBlood();
        MainManager.Instance.playerMaxHealth -= 5;
        if(MainManager.Instance.playerHealth > MainManager.Instance.playerMaxHealth) {
            MainManager.Instance.playerHealth = MainManager.Instance.playerMaxHealth;
        }
        option1.interactable = false;
        option2.interactable = false;
        option3.interactable = false;
        Leave();
    }

    public void Option3Click() {
        AudioManager.Instance.PlayBlood();
        MainManager.Instance.playerHealth -= 15;
        option1.interactable = false;
        option2.interactable = false;
        option3.interactable = false;
        Leave();
    }

    public void Leave() {
        StartCoroutine(LeaveCoroutine());
    }

    IEnumerator LeaveCoroutine() {
        yield return new WaitForSeconds(0.5f);
        crossfade.SetTrigger("Start");

        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("Map");
    }
}
