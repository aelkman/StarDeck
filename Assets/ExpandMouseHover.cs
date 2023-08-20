using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExpandMouseHover : MonoBehaviour
{
    public float expandSize;
    private bool coroutineAllowed;
    private Vector3 originalScale;
    private Vector3 expandedScale;

    Coroutine start;
    Coroutine stop;
    // Start is called before the first frame update
    void Start()
    {
        expandedScale = new Vector3(3.74f, 4.81f, 0.00f);
        coroutineAllowed = true;
        originalScale = transform.localScale;
        // expandedScale = new Vector3(originalScale.x + (expandSize * 10), originalScale.y + (expandSize * 10));
    }

    private void OnMouseExit() {
        if (start != null) StopCoroutine(start);
        // if (coroutineAllowed == false) {
            Debug.Log("exit routine starting");
            stop = StartCoroutine(ExitShrink());
        // }
    }

    private void OnMouseEnter() {
        if (stop != null) StopCoroutine(stop);
        if (coroutineAllowed) {
            Debug.Log("hover routine starting");
            start = StartCoroutine(HoverPulse());
        }
    }

    private IEnumerator HoverPulse() {
        coroutineAllowed = false;
        Vector3 newScale = originalScale;
        Debug.Log("originalScale: " + originalScale);
        for (float i = 0f; i <= 1f; i+= 0.1f) {
            transform.localScale = new Vector3(
                (Mathf.Lerp(newScale.x, newScale.x + expandSize, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(newScale.y, newScale.y + expandSize, Mathf.SmoothStep(0f, 1f, i))),
                0
            );
            Debug.Log(transform.localScale);
            newScale = transform.localScale;
            yield return new WaitForSeconds(0.015f);
        }
        Debug.Log("finalScale: " + transform.localScale);
    }

    private IEnumerator ExitShrink() {
        transform.localScale = expandedScale;
        Debug.Log("expandedScale: " +  expandedScale);
        for (float i = 0f; i <= 1f; i+= 0.1f) {
            transform.localScale = new Vector3(
                (Mathf.Lerp(transform.localScale.x, transform.localScale.x - expandSize, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(transform.localScale.y, transform.localScale.y - expandSize, Mathf.SmoothStep(0f, 1f, i))),
                0
            );
            Debug.Log(transform.localScale);
            yield return new WaitForSeconds(0.015f);
        }
        coroutineAllowed = true;
    }
}
