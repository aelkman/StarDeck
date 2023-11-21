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
        AudioManager.Instance.PlayButtonPress();
        mainManager.coinCount += 250;
        var virus = Resources.Load<Card>("Negative Cards/Virus");
        var virusInstance = Instantiate(virus);
        var virusInstance2 = Instantiate(virus);
        deck.AddCard(virusInstance);
        deck.AddCard(virusInstance2);
        option1.interactable = false;
        option2.interactable = false;
        leave.interactable = false;
        StartCoroutine(LeaveCoroutine());
    }

    public void Option2Click() {
        AudioManager.Instance.PlayButtonPress();
        mainManager.coinCount += 50;
        option1.interactable = false;
        option2.interactable = false;
        leave.interactable = false;
        StartCoroutine(LeaveCoroutine());
    }

    public void Leave() {
        option1.interactable = false;
        option2.interactable = false;
        leave.interactable = false;
        StartCoroutine(LeaveCoroutine());
    }

    IEnumerator LeaveCoroutine() {
        AudioManager.Instance.PlayButtonPress();
        yield return new WaitForSeconds(0.5f);
        crossfade.SetTrigger("Start");

        yield return new WaitForSeconds(1.0f);

        GameManager.Instance.LoadScene("Map");
    }
}
