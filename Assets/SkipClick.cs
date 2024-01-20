using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkipClick : MonoBehaviour
{
    private float timeClicked = 0f;
    public float timeToHold = 3f;
    public Image donut;
    float fillValue = 0f;
    public GameObject skipText;
    public bool skip = false;
    // Start is called before the first frame update
    void Start()
    {
        skipText.SetActive(false);
        donut.fillAmount = 0f;
    }

    void Update() {

        if(Input.GetMouseButton(0)) {
            skipText.SetActive(true);
            // Debug.Log(timeClicked);
            timeClicked += Time.deltaTime;
            fillValue = timeClicked/timeToHold;
            donut.fillAmount = fillValue;
            if(fillValue >= 1) {
                skip = true;
            }
        }
        else if(Input.GetMouseButtonUp(0)) {
            skipText.SetActive(false);
            timeClicked = 0;
            fillValue = 0;
            donut.fillAmount = 0;
        }
    }
}
