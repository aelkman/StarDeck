using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DojoCity : MonoBehaviour
{
    public Button option1;
    public Button option2;
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
        AudioManager.Instance.PlayCardRustling();
        var calculatedTraining = Resources.Load<Card>("CardsEvents/Calculated Training");
        var ct1 = Instantiate(calculatedTraining);
        var ct2 = Instantiate(calculatedTraining);
        var ct3 = Instantiate(calculatedTraining);
        deck.AddCard(ct1);
        deck.AddCard(ct2);
        deck.AddCard(ct3);
        option1.interactable = false;
        option2.interactable = false;
        Leave();
    }

    public void Option2Click() {
        AudioManager.Instance.PlayButtonPress();
        option1.interactable = false;
        option2.interactable = false;
        Leave();
    }

    public void Leave() {
        StartCoroutine(LeaveCoroutine());
    }

    IEnumerator LeaveCoroutine() {
        yield return new WaitForSeconds(0.5f);
        crossfade.SetTrigger("Start");

        yield return new WaitForSeconds(1.0f);

        GameManager.Instance.LoadScene("Map");
    }
}
