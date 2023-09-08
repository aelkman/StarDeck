using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardMouseActions : MonoBehaviour
{
    private BattleManager battleManager;
    public GameObject cursorFollowerPrefab;
    private GameObject cursorFollowerInstance;
    private SingleTargetManager singleTargetManager;
    private CardDisplay cardDisplay;
    public float expandSize;
    private Quaternion originalRotation;
    private Vector3 originalScale = new Vector3(2.39f, 3.46f, 0.00f);
    private Vector3 originalPosition;
    private Vector3 expandedScale;
    public CardAnimator cardAnimator;
    private int siblingIndexOriginal;
    private bool allowStart = true;
    private bool allowEnd = false;
    private bool isSelected = false;
    private bool isFollowerPlaced = false;
    private bool isTarget;
    private bool followerCreated = false;
    private bool isHardReset = false;
    private bool isCancelled = false;
    private bool isCardPlayed = false;


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
            isCancelled = true;
            if (isSelected) {
                Debug.Log("cancelling action (right click)");
                if(cursorFollowerInstance != null) {
                    cursorFollowerInstance.SetActive(false);
                }
                if(!isTarget) {
                    // for cards that are not target cards, move it back
                    transform.localPosition = originalPosition;
                }
                isSelected = false;
                isHardReset = true;
                isCardPlayed = false;
                // now shrink card back to where it was
                ExitResetSequence();
            }
        }
        if (Input.GetMouseButtonUp (0)) {
            if (isSelected && !isHardReset) {
                Debug.Log("drag exit");

                if(!battleManager.CheckCanAct(cardDisplay.card)) {
                    isSelected = false;
                    isFollowerPlaced = false;
                    if(cursorFollowerInstance != null) {
                        cursorFollowerInstance.SetActive(false);
                    }
                    ExitResetSequence();
                }
                else{

                    isSelected = false;
                    isCardPlayed = true;
                    if (cursorFollowerInstance != null) {
                        cursorFollowerInstance.SetActive(false);
                    }
                    // add card play animation here
                    StartCoroutine(CardPlayAnimation(0.05f));

                    Debug.Log("isFollowerPlaced: " + isFollowerPlaced);
                    StartCoroutine(CardPlayDelaySequence(1.5f));


                }
            }
            else if (isHardReset) {
                isSelected = false;
                isHardReset = false;
                isFollowerPlaced = false;
                isCardPlayed = false;
            }
            isCancelled = false;
        }

    }

    private IEnumerator CardPlayDelaySequence(float time) {
        yield return new WaitForSeconds(time);
        // perform actions for Target cards exit sequence
        if (isFollowerPlaced) {
                singleTargetManager = GameObject.Find("SingleTargetManager").GetComponent<SingleTargetManager>();
            // if there is an existing target, perform the action
            if (singleTargetManager.GetTarget() != null) {
                Debug.Log("CardMouseActions: performing card action!");
                battleManager.TargetCardAction(cardDisplay);
            }
            else {
                // do nothing here
            }
            isFollowerPlaced = false;
        }
        else if (!isCancelled) {
            // play non target cards here
            battleManager.CardAction(cardDisplay);
        }
        isCardPlayed = false;
    }

    private IEnumerator CardPlayAnimation(float timeInterval) {
        Vector3 startingPosition = transform.position;
        Debug.Log("startingPos: " + startingPosition);
        transform.rotation = Quaternion.identity;
        Vector3 startingScale = transform.localScale;
        for (float i = 0f; i <= 1f; i+= timeInterval) {

            // if it's not a target, shrink the scale back to original as well
            if (!isTarget) {
                transform.localScale = new Vector3(
                    (Mathf.Lerp(startingScale.x, originalScale.x, Mathf.SmoothStep(0f, 1f, i))),
                    (Mathf.Lerp(startingScale.y, originalScale.y, Mathf.SmoothStep(0f, 1f, i))),
                    0
                );
            }

            transform.position = new Vector3(
                (Mathf.Lerp(startingPosition.x, 0f, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(startingPosition.y, 0f, Mathf.SmoothStep(0f, 1f, i))),
                0
            );
            yield return new WaitForSeconds(0.01f);
        }

        // first half flip
        startingScale = transform.localScale;
        float flipSpeed = 0.1f;
        for (float i = 0f; i <= 1f; i+= flipSpeed) {
            transform.eulerAngles = new Vector3(
                0f,
                (Mathf.Lerp(0f, 90f, Mathf.SmoothStep(0f, 1f, i))),
                0f
            );
            transform.localScale = new Vector3(
                (Mathf.Lerp(startingScale.x, startingScale.x * 0.6f, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(startingScale.y, startingScale.y * 0.6f, Mathf.SmoothStep(0f, 1f, i))),
                0
            );
            yield return new WaitForSeconds(0.01f);
        }
        // change card sprite
        cardDisplay.BaseToBack();
        // second half flip
        startingScale = transform.localScale;
        for (float i = 0f; i <= 1f; i+= flipSpeed) {
            transform.eulerAngles = new Vector3(
                0f,
                (Mathf.Lerp(90f, 180f, Mathf.SmoothStep(0f, 1f, i))),
                0f
            );
            transform.localScale = new Vector3(
                (Mathf.Lerp(startingScale.x, startingScale.x * 0.6f, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(startingScale.y, startingScale.y * 0.6f, Mathf.SmoothStep(0f, 1f, i))),
                0
            );
            yield return new WaitForSeconds(0.01f);
        }
        // move card to discard pile
        Vector3 currentPosition = transform.position;
        Vector3 discardPosition = battleManager.discardDeck.transform.position;
        for (float i = 0f; i <= 1f; i+= timeInterval) {
            transform.eulerAngles = new Vector3(
                0f,
                0f,
                (Mathf.Lerp(0f, 45f, Mathf.SmoothStep(0f, 1f, i)))
            );
            transform.position = new Vector3(
                (Mathf.Lerp(currentPosition.x, discardPosition.x, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(currentPosition.y, discardPosition.y, Mathf.SmoothStep(0f, 1f, i))),
                0
            );
            yield return new WaitForSeconds(0.01f);
        }
        // fade card
        for (float i = 0f; i <= 1f; i+= 0.1f) {
            cardDisplay.cardBase.color = new Color32(
                255,
                255,
                255,
                (byte) (Mathf.Lerp(1, 0, Mathf.SmoothStep(0f, 1f, i)) * 255)
            );
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator FlipAnimationSequence() {
        cardAnimator.FlipAnimation1();
        yield return new WaitForSeconds(cardAnimator.flipTime1);
        cardDisplay.BaseToBack();
        cardAnimator.FlipAnimation2();
    }

    public GameObject GetCursorFollowerInstance() {
        return cursorFollowerInstance;
    }


    private void ExitResetSequence() {
        if (!isCardPlayed) {
            transform.SetSiblingIndex(siblingIndexOriginal);
            if (start != null) {
                StopCoroutine(start);
                start = null;
            }
            Debug.Log("exit routine starting");
            stop = StartCoroutine(ExitShrink());
        }
    }

    private void OnMouseExit() {
        ExitResetSequence();
    }

    private void OnMouseEnter() {

            if (!isCardPlayed) {
                // check if it's a Target card first
                cardDisplay = GetComponent<CardDisplay>();
                isTarget = cardDisplay.card.isTarget;
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
        else if(!isTarget && !isCancelled) {
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
