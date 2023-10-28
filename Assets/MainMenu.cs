using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button option1;
    public Button option2;
    public Button option3;
    public Button option4;
    public Animator crossfade;
    public Animator shipAnimator;
    public ParticleSystem rocket1;
    public ParticleSystem rocket2;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Option1Click() {
        AudioManager.Instance.PlayButtonPress();
        shipAnimator.SetTrigger("Start");
        rocket1.Play();
        rocket2.Play();
        Leave();
    }

    public void Option2Click() {
        AudioManager.Instance.PlayButtonPress();

    }

    public void Option3Click() {
        AudioManager.Instance.PlayButtonPress();

    }

    public void QuitClick() {
        StartCoroutine(QuitClickTimed());
    }

    public IEnumerator QuitClickTimed() {
        AudioManager.Instance.PlayButtonPress();
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }

    public void Leave() {
        StartCoroutine(LeaveCoroutine());
    }

    IEnumerator LeaveCoroutine() {
        yield return new WaitForSeconds(2.5f);
        crossfade.SetTrigger("Start");

        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("OpeningScene1");
    }
}
