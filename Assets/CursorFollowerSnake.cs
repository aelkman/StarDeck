using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorFollowerSnake : MonoBehaviour
{
    public float expandSize;
    public float timeUnit = 1f;
    public float totTime = 10f;
    public float forceAmount = 2000;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    public Vector3 originalPosition;
    private Vector3 raisedPosition;
    private Vector3 expandedScale;
    private int siblingIndexOriginal;
    private Rigidbody2D rigidBody;

    private Coroutine start;
    private Coroutine stop;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        // Debug.Log("index: " + transform.GetSiblingIndex());
        // hard coded solution for optimization, if the expandSize changes
        // this will need to be recalculated
        expandedScale = new Vector3(4.19f, 5.26f, 0.00f);
        originalScale = transform.localScale;
        originalRotation = transform.rotation;
        // originalPosition = transform.localPosition;
        // Debug.Log("original rotation: " + originalRotation);
        // Debug.Log("originalScale: " + originalScale);
        siblingIndexOriginal = transform.GetSiblingIndex();

    }

    void Update() {
        Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        // originalPosition = transform.TransformPoint(originalPosition);
        originalPosition.z = Camera.main.nearClipPlane;
        // Debug.Log("orig pos: " + originalPosition);
        Vector3 translatedWorldPosition = new Vector3(screenToWorld.x, screenToWorld.y, Camera.main.nearClipPlane);
        // Debug.Log("mouse trans pos: " + translatedWorldPosition);
        transform.position = translatedWorldPosition;
        float calculation = (originalPosition.x - translatedWorldPosition.x)/1.78f;
        // // Debug.Log("calculation: " + calculation);
        // Vector3 currentAngle = new Vector3(0f, 0f, Mathf.Lerp(180f, 0f, (translatedWorldPosition.x+1.778f)/3.556f));
        float angle = Mathf.Rad2Deg * Mathf.Atan((originalPosition.y - translatedWorldPosition.y) / (originalPosition.x - translatedWorldPosition.x));
        if(angle < 0) {
            angle = 180 + angle + 30 * Mathf.Lerp(0, 1, Mathf.Abs( originalPosition.x - translatedWorldPosition.x));
            if(angle > 180) {
                angle = 180;
            }
        }
        else {
            angle -= 30 * Mathf.Lerp(0, 1, Mathf.Abs( originalPosition.x - translatedWorldPosition.x));
            if(angle < 0) {
                angle = 0;
            }
        }

        // math function that correlates between -1 and 1 based on distance from start
        // angle = 1.2f * angle;

        // float distance = Vector3.Distance(originalPosition, translatedWorldPosition);
        // Debug.Log("distance: " + distance);
        // angle = Mathf.Lerp(180, 0, Mathf.Lerp(-1, 1, distance/ 1.0f));

        // Debug.Log(angle);
        // angle += 20;
        Vector3 currentAngle = new Vector3(0f, 0f, angle);
        transform.eulerAngles = currentAngle;
    }
    // private void OnMouseExit() {
    //     // transform.SetSiblingIndex(siblingIndexOriginal);
    //     if (start != null) {
    //         StopCoroutine(start);
    //         start = null;
    //     }
    //     // // Debug.Log("exit routine starting");
    //     stop = StartCoroutine(ExitShrink());
    // }

    // private void OnMouseEnter() {

    //     if (stop != null) {
    //         StopCoroutine(stop);
    //         stop = null;
    //     }
    //     // if (coroutineAllowed) {
    //         originalPosition = transform.localPosition;
    //         originalRotation = transform.rotation;
    //         // float up a bit for hover
    //         // raisedPosition = new Vector3(transform.localPosition.x, 75, 0);
    //         // transform.localPosition = raisedPosition;
    //         // Debug.Log("hover routine starting");
    //         start = StartCoroutine(HoverPulse());
    //     // }
    // }

    // void OnTriggerEnter2D(Collider2D collider) {
    //     // Debug.Log("hovered enemy!");
    // }

    // void OnCollisionEnter2D(Collision2D other) {
    //     // Debug.Log("hovered enemy!");
    // }

    private void OnMouseOver() {

        // Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        // Vector3 translatedWorldPosition = new Vector3(screenToWorld.x, screenToWorld.y, Camera.main.nearClipPlane);

        // transform.position = translatedWorldPosition;
    }

    private static float WrapAngle(float angle)
    {
        angle%=360;
        if(angle >180)
            return angle - 360;

        return angle;
    }

    // private IEnumerator DragCoroutine() {
    //     // transform.SetSiblingIndex(10);
    //     // coroutineAllowed = false;
    //     // Vector3 newScale = originalScale;
    //     // transform.rotation = Quaternion.identity;

    //     // // Debug.Log("originalScale: " + originalScale);
    //     for (float i = 0f; i <= totTime; i+= timeUnit) {
    //         float fractionalPlace = 1 / (transform.GetSiblingIndex() + 1);

    //         Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    //         Vector3 translatedWorldPosition = new Vector3(screenToWorld.x, screenToWorld.y, Camera.main.nearClipPlane);
    //         // if (translatedWorldPosition.y > 320) {
    //         //     translatedWorldPosition.y = 320;
    //         // }
    //         transform.position = new Vector3(
    //             Mathf.Lerp(originalPosition.x, translatedWorldPosition.x, Mathf.SmoothStep(0, fractionalPlace, i)),
    //             Mathf.Lerp(originalPosition.y, translatedWorldPosition.y, Mathf.SmoothStep(0, fractionalPlace, i)),
    //             0f
    //         );

    //         // raisedPosition = new Vector3(originalPosition.x, 75, 0);
    //         // transform.localPosition = new Vector3(originalPosition.x, Mathf.Lerp(originalPosition.y, 75, Mathf.SmoothStep(0, 1, i)), 0);

    //         // // transform.localScale = new Vector3(
    //         // //     (Mathf.Lerp(newScale.x, newScale.x + expandSize, Mathf.SmoothStep(0f, 1f, i))),
    //         // //     (Mathf.Lerp(newScale.y, newScale.y + expandSize, Mathf.SmoothStep(0f, 1f, i))),
    //         // //     0
    //         // // );
    //         // Vector3 currentAngle = new Vector3(0f, 0f, Mathf.Lerp(WrapAngle(originalRotation.eulerAngles.z), 0f, Mathf.SmoothStep(0, 1, i)));
    //         // transform.eulerAngles = currentAngle;
    //         // // // Debug.Log(transform.localScale);
    //         // newScale = transform.localScale;
    //         yield return new WaitForSeconds(0.015f);
    //     }
    //     // expandedScale = transform.localScale;
    //     // Debug.Log("finalScale: " + transform.localScale);
    // }

    // private IEnumerator ExitShrink() {
    //     transform.localScale = expandedScale;
    //     transform.rotation = originalRotation;
    //     // Debug.Log("rotation reset: " + originalRotation);
    //     transform.localPosition = new Vector3(originalPosition.x, originalPosition.y, 0);
    //     // // Debug.Log("expandedScale: " +  expandedScale);
    //     for (float i = 0f; i <= 1f; i+= 0.1f) {
    //         transform.localScale = new Vector3(
    //             (Mathf.Lerp(transform.localScale.x, transform.localScale.x - expandSize, Mathf.SmoothStep(0f, 1f, i))),
    //             (Mathf.Lerp(transform.localScale.y, transform.localScale.y - expandSize, Mathf.SmoothStep(0f, 1f, i))),
    //             0
    //         );
    //         // // Debug.Log(transform.localScale);
    //         yield return new WaitForSeconds(0.015f);
    //     }
    //     coroutineAllowed = true;
    // }
}


