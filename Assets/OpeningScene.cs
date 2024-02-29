using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Video;

public class OpeningScene : MonoBehaviour
{
    public TypewriterText typewriterText;
    public Animator textFadeAnimator;
    public Animator textFadeAnimator2;
    public Animator textFadeAnimator3;
    public Animator videoAnimator;
    public CanvasGroup videoCanvasGroup;
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
    public VideoPlayer cutscene1;
    private bool video1complete = false;
    private bool scene1started = false;
    public VideoPlayer cutscene2;
    private bool video2complete = false;
    private bool scene2started = false;
    public VideoPlayer cutscene3;
    private bool video3complete = false;
    private bool scene3started = false;
    public GameObject textHolder;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        ButtonEnabled(false);

        // cutscene 1 - ship
        videoCanvasGroup.alpha = 1;
        AudioManager.Instance.PlayVaporwaveSong1();
        cutscene1.Play();
        yield return new WaitUntil(() => cutscene1.isPlaying);
        videoCanvasGroup.alpha = 0;
        scene1started = true;
        yield return new WaitUntil(() => video1complete);
        videoAnimator.SetTrigger("StartFade");
        yield return new WaitForSeconds(1.0f);
        textHolder.SetActive(true);
        videoCanvasGroup.alpha = 0f;      
        cutscene1.gameObject.SetActive(false);

        // part 1
        yield return new WaitUntil(() => typewriterText.doneTyping);
        ButtonEnabled(true);
        yield return new WaitUntil(() => continueClicked);
        ButtonEnabled(false);
        continueClicked = false;
        textFadeAnimator.SetTrigger("Start");
        // openingAudio.PlaySong2WithFade(1.0f);
        yield return new WaitForSeconds(1.0f);

        // cutscene 2 - boarded
        cutscene2.Play();
        yield return new WaitUntil(() => cutscene2.isPlaying);
        textHolder.SetActive(false);
        scene2started = true;
        yield return new WaitUntil(() => video2complete);
        videoAnimator.SetTrigger("StartFade");
        yield return new WaitForSeconds(1.0f);
        textHolder.SetActive(true);
        videoCanvasGroup.alpha = 0f;      
        cutscene2.gameObject.SetActive(false);
        textHolder.SetActive(true);

        // part 2
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
        
        // fade & pause after click
        ButtonEnabled(false);
        continueClicked = false;
        // yield return StartCoroutine(FadeAudioSource.StartFade(openingAudio.song2, 0.5f, 0));
        textFadeAnimator2.SetTrigger("Start");
        yield return new WaitForSeconds(1.0f);
        // remove this until there's another cutscene to put here
        //textHolder.SetActive(false);

        // part 3
        final.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        // openingAudio.song3.Play();
        finalText = final.GetComponent<TypewriterText>();
        yield return new WaitUntil(() => finalText.doneTyping);
        ButtonEnabled(true);
        yield return new WaitUntil(() => continueClicked);
        ButtonEnabled(false);
        continueClicked = false;
        textFadeAnimator3.SetTrigger("Start");
        yield return new WaitForSeconds(1.0f);

        // custcene 3 - enter robotica
        cutscene3.Play();
        yield return new WaitUntil(() => cutscene3.isPlaying);
        textHolder.SetActive(false);
        scene3started = true;
        yield return new WaitUntil(() => video3complete);
        // cutscene3.gameObject.SetActive(false);
        // textHolder.SetActive(true);

        StartCoroutine(LoadLevel("Weapons"));
    }

    private void ButtonEnabled(bool enabled) {
        continueButton.interactable = enabled;
        buttonText.enabled = enabled;
    }

    IEnumerator LoadLevel(string sceneName) {
        transition = GameObject.Find("Crossfade").GetComponent<Animator>();
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1.0f);

        GameManager.Instance.LoadScene(sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        if(!cutscene1.isPlaying && scene1started) {
            video1complete = true;
        }
        if(!cutscene2.isPlaying && scene2started) {
            video2complete = true;
        }
        if(!cutscene3.isPlaying && scene3started) {
            video3complete = true;
        }
        if(skipClick.skip) {
            StartCoroutine(LoadLevel("Weapons"));
        }
    }

    public void ClickContinue() {
        AudioManager.Instance.PlayButtonPress();
        continueClicked = true;
    }
}
