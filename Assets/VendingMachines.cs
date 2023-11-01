using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VendingMachines : MonoBehaviour
{
    public Button option1;
    public GameObject jammedText;
    private bool isJammed = false;
    public ShopCardUISelector shopCardUISelector;
    public Animator crossfade;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(shopCardUISelector.limitReached && !isJammed) {
            jammedText.SetActive(true);
            isJammed = true;
        } 
    }

    public void Option1Click() {
        AudioManager.Instance.PlayButtonPress();
        option1.interactable = false;
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
