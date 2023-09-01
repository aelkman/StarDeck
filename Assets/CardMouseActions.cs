using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardMouseActions : MonoBehaviour
{
    private BattleManager battleManager;
    public GameObject cursorFollowerPrefab;
    private GameObject cursorFollowerInstance;
    private SingleTargetManager singleTargetManager;
    private Card card;
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
    private bool followerCreated = false;
    private bool isHardReset = false;

    Coroutine start;
    Coroutine stop;

    // Start is called before the first frame update
    void Start()
    {   
        singleTargetManager = GameObject.Find("SingleTargetManager").GetComponent<SingleTargetManager>();
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        originalPosition = transform.localPosition;
        originalRotation = transform.rotation;
        // originalScale = transform.localScale;
        expandedScale = new Vector3(4.14f, 5.21f, 0.00f);

        siblingIndexOriginal = transform.GetSiblingIndex();
    }

    void Update () {
        if (Input.GetMouseButtonDown(1)) {
            if (isSelected) {
                Debug.Log("cancelling action (right click)");
                cursorFollowerInstance.SetActive(false);
                isSelected = false;
                isHardReset = true;
                // now shrink card back to where it was
                ExitResetSequence();
            }
        }
        if (Input.GetMouseButtonUp (0)) {
            if (isSelected && !isHardReset) {
                Debug.Log("drag exit");

                isSelected = false;

                Debug.Log("isFollowerPlaced: " + isFollowerPlaced);
                // perform actions for Target cards exit sequence
                if (isFollowerPlaced) {
                        singleTargetManager = GameObject.Find("SingleTargetManager").GetComponent<SingleTargetManager>();
                    // if there is an existing target, perform the action
                    if (singleTargetManager.GetTarget() != null) {
                        Debug.Log("CardMouseActions: performing card action!");
                        battleManager.TargetCardAction(card);
                    }
                    cursorFollowerInstance.SetActive(false);
                    isFollowerPlaced = false;
                }
            }
            else if (isHardReset) {
                isSelected = false;
                isHardReset = false;
                isFollowerPlaced = false;
            }
        }

}

    private void ExitResetSequence() {
        transform.SetSiblingIndex(siblingIndexOriginal);
        if (start != null) {
            StopCoroutine(start);
            start = null;
        }
        Debug.Log("exit routine starting");
        stop = StartCoroutine(ExitShrink());
    }

    private void OnMouseExit() {
        ExitResetSequence();
    }

    private void OnMouseEnter() {
            // check if it's a Target card first
            card = GetComponent<CardDisplay>().card;
            isTarget = card.isTarget;
            Debug.Log("isTarget: " + isTarget);

            // if it's a Target, instantiate the CursorFollower prefab
            if(isTarget) {
                if(followerCreated) {
                    Destroy(cursorFollowerInstance);
                    followerCreated = false;
                }
                if(!followerCreated) {
                    cursorFollowerInstance = Instantiate(cursorFollowerPrefab);
                    cursorFollowerInstance.SetActive(false);
                    cursorFollowerInstance.transform.parent = transform.parent;
                    cursorFollowerInstance.transform.localScale = new Vector3(1182.52f, 1182.52f, 1182.52f);
                    followerCreated = true;
                }
            }

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

        // Debug.Log("transform postiion: " +  transform.localPosition);
        // Debug.Log(Input.mousePosition);

        if(isTarget) {
            if(!isFollowerPlaced) {
                cursorFollowerInstance.SetActive(true);
                cursorFollowerInstance.transform.localPosition = transform.localPosition;
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
