using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExpandMouseHover : MonoBehaviour
{
    public float expandSize;
    private bool coroutineAllowed;
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Vector3 raisedPosition;
    private Vector3 expandedScale;
    private int siblingIndexOriginal;

    Coroutine start;
    Coroutine stop;

    // Start is called before the first frame update
    void Start()
    {
        // hard coded solution for optimization, if the expandSize changes
        // this will need to be recalculated
        expandedScale = new Vector3(3.74f, 4.81f, 0.00f);
        coroutineAllowed = true;
        originalScale = transform.localScale;
        Debug.Log("originalScale: " + originalScale);
        siblingIndexOriginal = transform.GetSiblingIndex();
    }

    private void OnMouseExit() {
        transform.SetSiblingIndex(siblingIndexOriginal);
        if (start != null) StopCoroutine(start);
        Debug.Log("exit routine starting");
        stop = StartCoroutine(ExitShrink());
    }

    private void OnMouseEnter() {

        if (stop != null) StopCoroutine(stop);
        if (coroutineAllowed) {
            originalPosition = transform.localPosition;
            // float up a bit for hover
            raisedPosition = new Vector3(transform.localPosition.x, 75, 0);
            // Debug.Log(transform.GetSiblingIndex());
            transform.SetSiblingIndex(10);
            transform.localPosition = raisedPosition;
            Debug.Log("hover routine starting");
            start = StartCoroutine(HoverPulse());
        }
    }

    private void OnMouseDrag() {
        Debug.Log("transform postiion: " +  transform.localPosition);
        Debug.Log(Input.mousePosition);
        Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 translatedWorldPosition = new Vector3(screenToWorld.x, screenToWorld.y, Camera.main.nearClipPlane);
        // if (translatedWorldPosition.y > 320) {
        //     translatedWorldPosition.y = 320;
        // }
        transform.position = translatedWorldPosition;
        if (transform.localPosition.y > 200) {
            transform.localPosition = new Vector2(0, 200);
        }
        // else if (transform.localPosition.y > 320) {
        //     transform.localPosition = new Vector2(transform.localPosition.x, 320);
        // }
        // transform.localPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
    }

    private IEnumerator HoverPulse() {
        coroutineAllowed = false;
        Vector3 newScale = originalScale;

        // Debug.Log("originalScale: " + originalScale);
        for (float i = 0f; i <= 1f; i+= 0.1f) {
            transform.localScale = new Vector3(
                (Mathf.Lerp(newScale.x, newScale.x + expandSize, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(newScale.y, newScale.y + expandSize, Mathf.SmoothStep(0f, 1f, i))),
                0
            );
            // Debug.Log(transform.localScale);
            newScale = transform.localScale;
            yield return new WaitForSeconds(0.015f);
        }
        Debug.Log("finalScale: " + transform.localScale);
    }

    private IEnumerator ExitShrink() {
        transform.localScale = expandedScale;
        transform.localPosition = new Vector3(originalPosition.x, originalPosition.y, 0);
        // Debug.Log("expandedScale: " +  expandedScale);
        for (float i = 0f; i <= 1f; i+= 0.1f) {
            transform.localScale = new Vector3(
                (Mathf.Lerp(transform.localScale.x, transform.localScale.x - expandSize, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(transform.localScale.y, transform.localScale.y - expandSize, Mathf.SmoothStep(0f, 1f, i))),
                0
            );
            // Debug.Log(transform.localScale);
            yield return new WaitForSeconds(0.015f);
        }
        coroutineAllowed = true;
    }
}
