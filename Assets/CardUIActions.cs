using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CardUIActions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardUISelector cardUISelector;
    // private HandManager handManager;
    private BattleManager battleManager;
    private CardDisplay cardDisplay;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    public Vector3 originalPosition;
    private Vector3 expandedScale;
    public CardAnimator cardAnimator;
    private int siblingIndexOriginal;
    private bool isSelected = false;
    private bool isTarget;
    private bool followerCreated = false;
    private bool isHardReset = false;
    private bool isCancelled = false;
    private bool isCardPlayed = false;
    private bool isFirstEnter = true;
    private bool expandAllowed = true;
    public float scaleMultiplier = 1.6f;
    public bool isShop = false;

    Coroutine start;
    Coroutine stop;

    // Start is called before the first frame update
    void Start()
    {   
        // load all cards and pick one at random
        cardDisplay = GetComponent<CardDisplay>();
        if(gameObject.name == "ShopCard(Clone)") {
            isShop = true;
        }
        if(isShop) {
            cardUISelector = GameObject.Find("ShopCardUISelector").GetComponent<CardUISelector>();
        }
        else {
            cardUISelector = GameObject.Find("CardUISelector").GetComponent<CardUISelector>();
        }
        // handManager = GameObject.Find("HandManager").GetComponent<HandManager>();
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        siblingIndexOriginal = transform.GetSiblingIndex();
    }

    void Update () {


    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(1)) {
            isCancelled = true;
            if (isSelected) {
                // Debug.Log("cancelling action (right click)");
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
                // Debug.Log("drag exit");

                        isSelected = false;
                        isCardPlayed = true;

                        // add card play animation here
                        if (cardUISelector.AddToDeck(cardDisplay.card)){
                            StartCoroutine(CardPlayAnimation(0.05f));
                        }
                        // perform card selection & add to deck
            }
            else if (isHardReset) {
                isSelected = false;
                isHardReset = false;
                isCardPlayed = false;
            }
            isCancelled = false;
        }
    }

    private IEnumerator CardPlayAnimation(float timeInterval) {
        Vector3 startingPosition = transform.position;
        // Debug.Log("startingPos: " + startingPosition);
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

    private void ExitResetSequence() {
        if (!isCardPlayed) {
            transform.SetSiblingIndex(siblingIndexOriginal);
            Debug.Log("sibling index: " + transform.GetSiblingIndex());
            if (start != null) {
                StopCoroutine(start);
                start = null;
            }
            // Debug.Log("exit routine starting");
            stop = StartCoroutine(ExitShrink());
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        isSelected = true;
        if (!isCardPlayed) {

            if (stop != null) {
                StopCoroutine(stop);
                stop = null;
            }
            if (isFirstEnter) {
                originalPosition = transform.localPosition;
                originalRotation = transform.rotation;
                originalScale = transform.localScale;
                expandedScale = new Vector3(originalScale.x * scaleMultiplier, originalScale.y * scaleMultiplier, 0.00f);
                isFirstEnter = false;
            }

            // Debug.Log("hover routine starting");
            if (expandAllowed) {
                start = StartCoroutine(HoverPulse());
            }
        }
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        isSelected = false;
        ExitResetSequence();
    }

    // private void OnMouseDrag() {
    //     isSelected = true;

    //     Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    //     Vector3 translatedWorldPosition = new Vector3(screenToWorld.x, screenToWorld.y, Camera.main.nearClipPlane);
    //     transform.position = translatedWorldPosition;
    // }

    private static float WrapAngle(float angle)
    {
        angle%=360;
        if(angle >180)
            return angle - 360;

        return angle;
    }

    private IEnumerator HoverPulse() {
        expandAllowed = false;
        // Debug.Log("child: " + siblingIndexOriginal + ", enter coroutine");
        transform.SetSiblingIndex(20);
        // Vector3 newScale = originalScale;
        // transform.localScale = originalScale;
        // transform.rotation = Quaternion.identity;

        // for each other child, shift it over
        // children to the left, shift left
        // for (int j = 0; j < siblingIndexOriginal; j++) {
        //     StartCoroutine(ShiftCardLeft(j));
        // }
        // // children to the right, shift right
        // for (int j = siblingIndexOriginal + 1; j < handManager.handCards.Count; j++) {
        //     StartCoroutine(ShiftCardRight(j));
        // }

        Vector3 currentPosition = transform.localPosition;
        Vector3 currentScale = transform.localScale;
        // Debug.Log("originalScale: " + originalScale);
        for (float i = 0f; i <= 1f; i+= 0.1f) {
            transform.localPosition = new Vector3(currentPosition.x, Mathf.Lerp(currentPosition.y, 75, Mathf.SmoothStep(0, 1, i)), currentPosition.z);

            transform.localScale = new Vector3(
                (Mathf.Lerp(currentScale.x, expandedScale.x, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(currentScale.y, expandedScale.y, Mathf.SmoothStep(0f, 1f, i))),
                0
            );

            Vector3 currentAngle = new Vector3(0f, 0f, Mathf.Lerp(WrapAngle(originalRotation.eulerAngles.z), 0f, Mathf.SmoothStep(0, 1, i)));
            transform.eulerAngles = currentAngle;
            // Debug.Log(transform.localScale);
            yield return new WaitForSeconds(0.015f);
        }
    }

    // private IEnumerator ShiftCardLeft(int index) {
    //     Vector3 cardOriginalPos = handManager.handCards[index].transform.localPosition;
    //     for (float i = 0f; i <= 1f; i+= 0.1f) {
    //         handManager.handCards[index].transform.localPosition = new Vector3(
    //             Mathf.Lerp(cardOriginalPos.x, cardOriginalPos.x - 155, Mathf.SmoothStep(0, 1, i)),
    //             cardOriginalPos.y,
    //             cardOriginalPos.z
    //         );
    //         yield return new WaitForSeconds(0.01f);
    //     }
    // }

    // private IEnumerator ShiftCardRight(int index) {
    //     Vector3 cardOriginalPos = handManager.handCards[index].transform.localPosition;
    //     for (float i = 0f; i <= 1f; i+= 0.1f) {
    //         handManager.handCards[index].transform.localPosition = new Vector3(
    //             Mathf.Lerp(cardOriginalPos.x, cardOriginalPos.x + 155, Mathf.SmoothStep(0, 1, i)),
    //             cardOriginalPos.y,
    //             cardOriginalPos.z
    //         );
    //         yield return new WaitForSeconds(0.01f);
    //     }
    // }

    private IEnumerator ExitShrink() {
        // transform.localScale = expandedScale;
        transform.rotation = originalRotation;
        // Debug.Log("rotation reset: " + originalRotation);
        // Debug.Log("original position: " + originalPosition);
        // transform.localPosition = new Vector3(originalPosition.x, originalPosition.y, 0);
        // Debug.Log("expandedScaleFirst: " +  expandedScale);

        // for (int j = 0; j < siblingIndexOriginal; j++) {
        //     StartCoroutine(ShiftCardRight(j));
        // }
        // // children to the right, shift right
        // for (int j = siblingIndexOriginal + 1; j < handManager.handCards.Count; j++) {
        //     StartCoroutine(ShiftCardLeft(j));
        // }

        Vector3 currentPosition = transform.localPosition;
        for (float i = 0f; i <= 1f; i+= 0.1f) {

            transform.localPosition = new Vector3(
                currentPosition.x,
                Mathf.Lerp(currentPosition.y, originalPosition.y, Mathf.SmoothStep(0, 1, i)),
                currentPosition.z
            );

            transform.localScale = new Vector3(
                (Mathf.Lerp(expandedScale.x, originalScale.x, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(expandedScale.y, originalScale.y, Mathf.SmoothStep(0f, 1f, i))),
                0
            );

            // Debug.Log(transform.localScale);
            yield return new WaitForSeconds(0.015f);
        }

        expandAllowed = true;
    }
}
