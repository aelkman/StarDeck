using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class OpeningScene : MonoBehaviour
{
    public TypewriterText typewriterText;
    public Animator textFadeAnimator;
    public Animator textFadeAnimator2;
    public OpeningAudio openingAudio;
    public GameObject convo1;
    public GameObject convo2;
    public GameObject convo3;
    public Animator transition;
    public TypewriterText convoText1;
    public TypewriterText convoText2;
    public TypewriterText convoText3;
    public GameObject final;
    private TypewriterText finalText;
    public Button continueButton;
    public TextMeshProUGUI buttonText;
    public SkipClick skipClick;
    private bool continueClicked = false;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        // part 1
        ButtonEnabled(false);
        yield return new WaitUntil(() => typewriterText.doneTyping);
        ButtonEnabled(true);
        yield return new WaitUntil(() => continueClicked);

        // part 2
        ButtonEnabled(false);
        continueClicked = false;
        textFadeAnimator.SetTrigger("Start");
        openingAudio.PlaySong2WithFade(1.0f);
        yield return new WaitForSeconds(1.0f);
        convo1.SetActive(true);
        convoText1 = convo1.GetComponent<TypewriterText>();
        yield return new WaitUntil(() => convoText1.doneTyping);
        yield return new WaitForSeconds(0.5f);
        
        convo2.SetActive(true);
        convoText2 = convo2.GetComponent<TypewriterText>();
        yield return new WaitUntil(() => convoText2.doneTyping);
        yield return new WaitForSeconds(1.0f);
        
        convo3.SetActive(true);
        convoText3 = convo3.GetComponent<TypewriterText>();
        openingAudio.PlayHits();
        yield return new WaitUntil(() => convoText3.doneTyping);
        yield return new WaitForSeconds(2.0f);
        ButtonEnabled(true);
        yield return new WaitUntil(() => continueClicked);

        // part 3
        ButtonEnabled(false);
        continueClicked = false;
        yield return StartCoroutine(FadeAudioSource.StartFade(openingAudio.song2, 0.5f, 0));
        textFadeAnimator2.SetTrigger("Start");
        yield return new WaitForSeconds(1.0f);
        final.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        openingAudio.song3.Play();
        finalText = final.GetComponent<TypewriterText>();
        yield return new WaitUntil(() => finalText.doneTyping);
        ButtonEnabled(true);
        yield return new WaitUntil(() => continueClicked);

        StartCoroutine(LoadLevel("Map"));
    }

    private void ButtonEnabled(bool enabled) {
        continueButton.interactable = enabled;
        buttonText.enabled = enabled;
    }

    IEnumerator LoadLevel(string sceneName) {
        transition = GameObject.Find("Crossfade").GetComponent<Animator>();
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene(sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        if(skipClick.skip) {
            StartCoroutine(LoadLevel("Map"));
        }
    }

    public void ClickContinue() {
        continueClicked = true;
    }
}
