using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapAnimations : MonoBehaviour
{
    private static MapAnimations _instance;
    public GameObject introTextContainer;
    public GameObject introText;
    public Image blackBG;
    public Animator introTextAnimator;
    public Animator crossfadeAnimator;
    public Animator slidingAnimator;
    public GameObject mapTitle;
    public TypewriterText introTypewriter;
    private bool clicked = false;

    void Awake()
    {
        if (_instance != null && _instance != this) 
        { 
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowIntroText());
    }

    private IEnumerator ShowIntroText() {
        blackBG.gameObject.SetActive(true);
        MapManager.Instance.destinationsClickable = false;
        yield return new WaitForSeconds(0.5f);
        crossfadeAnimator.SetTrigger("FadeIn");
        blackBG.gameObject.SetActive(false);
        slidingAnimator.SetTrigger("StartSlide");
        yield return new WaitForSeconds(1.8f);
        introTextAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(1.0f);
        if(introText != null) {
            introText.SetActive(true);
            // yield return new WaitUntil(() => introTypewriter.doneTyping);
            yield return new WaitUntil(() => clicked);
            MapManager.Instance.destinationsClickable = true;
            if(introTextAnimator != null) {
                introTextAnimator.SetTrigger("End");
                MapManager.Instance.destinationsClickable = true;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if(SceneManager.GetActiveScene().name != "Map") {
            Destroy(introTextContainer);
            if(mapTitle != null) {
                Destroy(mapTitle);
            }
        }
        if(introTypewriter != null) {
            if(Input.GetMouseButtonUp(0) && introTypewriter.doneTyping) {
                clicked = true;
            }
        }
        else if(Input.GetMouseButtonUp(0)) {
            clicked = true;
        }

        // if(Input.GetMouseButtonUp(0)) {
        //     clicked = false;
        // }
    }
}
