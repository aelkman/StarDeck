using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExpandMouseHover : MonoBehaviour
{
    public float expandSize;
    private GameObject cursorFollower;
    private Quaternion originalRotation;
    private Vector3 originalScale = new Vector3(2.39f, 3.46f, 0.00f);

    private Vector3 originalPosition;
    private Vector3 expandedScale;
    private int siblingIndexOriginal;
    private bool allowStart = true;
    private bool allowEnd = false;
    private bool isSelected = false;
    private bool isFollowerPlaced = false;
    private bool isTarget;

    Coroutine start;
    Coroutine stop;

    // Start is called before the first frame update
    void Start()
    {   
        originalPosition = transform.localPosition;
        originalRotation = transform.rotation;
        // originalScale = transform.localScale;
        expandedScale = new Vector3(4.14f, 5.21f, 0.00f);

        siblingIndexOriginal = transform.GetSiblingIndex();
    }

    void Update () {
    if (Input.GetMouseButtonUp (0)) {
        if (isSelected) {
            Debug.Log("drag exit");

            isSelected = false;
            isFollowerPlaced = false;
        }
        // This will be executed when the mouse button was released.
    }
}

    private void OnMouseExit() {
            transform.SetSiblingIndex(siblingIndexOriginal);
            if (start != null) {
                StopCoroutine(start);
                start = null;
            }
            Debug.Log("exit routine starting");
            stop = StartCoroutine(ExitShrink());
    }

    private void OnMouseEnter() {

            if (stop != null) {
                StopCoroutine(stop);
                stop = null;
            }
            originalPosition = transform.localPosition;
            originalRotation = transform.rotation;
            // originalScale = transform.localScale;
            expandedScale = new Vector3(4.14f, 5.21f, 0.00f);

            Debug.Log("hover routine starting");
            start = StartCoroutine(HoverPulse());
    }

    private void OnMouseDrag() {
        isSelected = true;

        cursorFollower = GameObject.Find("CursorSelector");

        CardDisplay refScript = GetComponent<CardDisplay>();
        isTarget = refScript.card.isTarget;
        Debug.Log("isTarget: " + isTarget);

        Debug.Log("transform postiion: " +  transform.localPosition);
        Debug.Log(Input.mousePosition);

        if(isTarget) {
            cursorFollower.SetActive(true);
            if(!isFollowerPlaced) {
                cursorFollower.transform.localPosition = transform.localPosition;
                isFollowerPlaced = true;
            }
        }
        else {
            Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3 translatedWorldPosition = new Vector3(screenToWorld.x, screenToWorld.y, Camera.main.nearClipPlane);
            transform.position = translatedWorldPosition;
        }
    }

    private static float WrapAngle(float angle)
    {
        angle%=360;
        if(angle >180)
            return angle - 360;

        return angle;
    }

    private IEnumerator HoverPulse() {
        transform.SetSiblingIndex(10);
        Vector3 newScale = originalScale;
        transform.localScale = originalScale;
        // transform.rotation = Quaternion.identity;

        Debug.Log("originalScale: " + originalScale);
        for (float i = 0f; i <= 1f; i+= 0.1f) {
            transform.localPosition = new Vector3(originalPosition.x, Mathf.Lerp(originalPosition.y, 75, Mathf.SmoothStep(0, 1, i)), originalPosition.z);

            transform.localScale = new Vector3(
                (Mathf.Lerp(originalScale.x, originalScale.x + expandSize, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(originalScale.y, originalScale.y + expandSize, Mathf.SmoothStep(0f, 1f, i))),
                0
            );
            Vector3 currentAngle = new Vector3(0f, 0f, Mathf.Lerp(WrapAngle(originalRotation.eulerAngles.z), 0f, Mathf.SmoothStep(0, 1, i)));
            transform.eulerAngles = currentAngle;
            Debug.Log(transform.localScale);
            yield return new WaitForSeconds(0.015f);
        }
        // expandedScale = transform.localScale;
        Debug.Log("expandedScale: " + expandedScale);
        Debug.Log("expandedPosition: " + transform.localPosition);

        // // enable cursorFollower and move under this card
        // cursorFollower.SetActive(true);
        Debug.Log("finalScale: " + transform.localScale);
    }

    private IEnumerator ExitShrink() {
        transform.localScale = expandedScale;
        transform.rotation = originalRotation;
        Debug.Log("rotation reset: " + originalRotation);
        Debug.Log("original position: " + originalPosition);
        transform.localPosition = new Vector3(originalPosition.x, originalPosition.y, 0);
        Debug.Log("expandedScaleFirst: " +  expandedScale);
        for (float i = 0f; i <= 1f; i+= 0.1f) {
            transform.localScale = new Vector3(
                (Mathf.Lerp(expandedScale.x, expandedScale.x - expandSize, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(expandedScale.y, expandedScale.y - expandSize, Mathf.SmoothStep(0f, 1f, i))),
                0
            );
            // Debug.Log(transform.localScale);
            yield return new WaitForSeconds(0.015f);
        }
    }
}
