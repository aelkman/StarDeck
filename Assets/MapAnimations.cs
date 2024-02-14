using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapAnimations : MonoBehaviour
{
    private static MapAnimations _instance;
    public GameObject introTextContainer;
    public GameObject introText;
    public Animator introTextAnimator;
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
        MapManager.Instance.destinationsClickable = false;
        yield return new WaitForSeconds(2.8f);
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
        if(Input.GetMouseButtonUp(0) && introTypewriter.doneTyping) {
            clicked = true;
        }
        // if(Input.GetMouseButtonUp(0)) {
        //     clicked = false;
        // }
    }
}
