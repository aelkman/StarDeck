using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
 
public class ZoomScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    // [SerializeField] float startSize = 1;
    [SerializeField] float minSize = 0.5f;
    [SerializeField] float maxSize = 1;
    public float initialZoom = 1.5f;
    public Animator mapIntroText;
    public GameObject introText;
 
    [SerializeField] private float zoomRate = 5;
    private float currentScale = 0f;
    // public float maxScale = 1.0f;
    // public float minScale = 0.63f;
 
    private bool onObj = false;
    private RectTransform rt;
 

    void Start() {
        rt = GetComponent<RectTransform>();
        StartCoroutine(ZoomLerp());
    }

    private IEnumerator ZoomLerp() {
        yield return new WaitForSeconds(1f);
        float time = 0.01667f;
        float totTime = 1.5f;
        for(float i = 0; i < totTime; i+= time) {
            var zoomVal = Mathf.Lerp(1, initialZoom, Mathf.SmoothStep(0,1, i/totTime));
            SetZoom(zoomVal);
            //960, 510 localPos of rectTransform
            var newPos = new Vector3(Mathf.Lerp(rt.localPosition.x, 960, Mathf.SmoothStep(0,1,i/totTime)),
                        Mathf.Lerp(rt.localPosition.y, 510, Mathf.SmoothStep(0,1,i/totTime)),
                        rt.localPosition.z);
            rt.localPosition = newPos;
            yield return new WaitForSeconds(time);
        }

        mapIntroText.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        introText.SetActive(true);
    }

    public void LerpToPos(Vector3 pos) {
        StartCoroutine(LerpToPosTime(pos));
    }

    private IEnumerator LerpToPosTime(Vector3 pos) {
        // 30 frames/second
        float time = 0.01667f;
        float totTime = 1.5f;
        for(float i = 0; i < totTime; i+= time) {
            // var zoomVal = Mathf.Lerp(1, 2, Mathf.SmoothStep(0,1, i/totTime));
            // SetZoom(zoomVal);
            var rtPos = rt.localPosition;
            var newPos = new Vector3(Mathf.Lerp(rt.localPosition.x, pos.x, Mathf.SmoothStep(0,1,i/totTime)),
                        Mathf.Lerp(rt.localPosition.y, pos.y, Mathf.SmoothStep(0,1,i/totTime)),
                        rt.localPosition.z);
            rt.localPosition = newPos;
            yield return new WaitForSeconds(time);
        }
    }

    private void Update() {
        float scrollWheel = -Input.mouseScrollDelta.y;
        // if (currentScale >= maxScale) {
        //     scrollWheel *= -1;
        // } else if (currentScale <= minScale) {
        //     scrollWheel *= -1;
        // }
 
        if (onObj && scrollWheel != 0) {
            ChangeZoom(scrollWheel);
        }
    }
 
    public void OnPointerEnter(PointerEventData eventData) {
        onObj = true;
    }
 
    public void OnPointerExit(PointerEventData eventData) {
        onObj = false;
    }
 
    public void OnDisable() {
        onObj = false;
    }
 
    private void ChangeZoom(float scrollWheel) {
        float rate = 1 + zoomRate * Time.unscaledDeltaTime;
        if (scrollWheel > 0) {
            SetZoom(Mathf.Clamp(transform.localScale.y / rate, minSize, maxSize));
        } else {
            SetZoom(Mathf.Clamp(transform.localScale.y * rate, minSize, maxSize));
        }
        currentScale = transform.localScale.x;
    }
 
    private void SetZoom(float targetSize) {
        transform.localScale = new Vector3(targetSize, targetSize, 1);
    }
}
