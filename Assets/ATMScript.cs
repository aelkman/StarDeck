using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ATMScript : MonoBehaviour
{
    public Button option1;
    public Button option2;
    public Button leave;
    private MainManager mainManager;
    private Deck deck;
    public Animator crossfade;
    // Start is called before the first frame update
    void Start()
    {
        deck = GameObject.Find("Deck").GetComponent<Deck>();
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Option1Click() {
        mainManager.coinCount += 150;
        var virus = Resources.Load<Card>("Negative Cards/Virus");
        deck.AddCard(virus);
        deck.AddCard(virus);
        option1.interactable = false;
        option2.interactable = false;
    }

    public void Option2Click() {
        mainManager.coinCount += 50;
        option1.interactable = false;
        option1.interactable = false;
    }

    public void Leave() {
        StartCoroutine(LeaveCoroutine());
    }

    IEnumerator LeaveCoroutine() {
        crossfade.SetTrigger("Start");

        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("Map");
    }
}
