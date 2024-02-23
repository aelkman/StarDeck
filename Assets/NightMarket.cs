using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NightMarket : MonoBehaviour
{
    public Button option1;
    public Button option2;
    public Animator crossfade;
    public DeckViewer deckViewer;
    public GameObject tradeCardViewer;
    public TextMeshProUGUI leaveButton;
    public CardUISelector cardSelector;
    // Start is called before the first frame update
    void Start()
    {
        deckViewer = GameObject.Find("PersistentHUD").GetComponent<PersistentHUD>().deckViewer;
    }

    // Update is called once per frame
    void Update()
    {
        if(cardSelector.addedCount > 0) {
            leaveButton.text = "Leave";
        }
    }

    public void Option1Click() {
        AudioManager.Instance.PlayButtonPress();
        deckViewer.StartRemoval(0);
        deckViewer.isRemovalEvent = true;
    }

    public void Option2Click() {
        AudioManager.Instance.PlayButtonPress();
        deckViewer.isRemovalEvent = false;
        Leave();
    }

    public void Leave() {
        StartCoroutine(LeaveCoroutine());
    }

    IEnumerator LeaveCoroutine() {
        deckViewer.isRemovalEvent = false;
        yield return new WaitForSeconds(0.5f);
        crossfade.SetTrigger("Start");

        yield return new WaitForSeconds(1.0f);

        GameManager.Instance.LoadScene("Map");
    }
}
