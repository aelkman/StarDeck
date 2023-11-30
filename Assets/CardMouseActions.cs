using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CardMouseActions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    private HandManager handManager;
    private BattleManager battleManager;
    public GameObject cursorFollowerPrefab;
    private GameObject cursorFollowerInstance;
    private SingleTargetManager STM;
    private CardDisplay cardDisplay;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    public Vector3 originalPosition;
    private Vector3 expandedScale;
    public CardAnimator cardAnimator;
    private int siblingIndexOriginal;
    private bool isSelected = false;
    private bool isFollowerPlaced = false;
    private bool isTarget;
    private bool followerCreated = false;
    private bool isHardReset = false;
    private bool isCancelled = false;
    private bool isCardPlayed = false;
    private bool isFirstEnter = true;
    private bool expandAllowed = true;
    public float scaleMultiplier = 1.3f;
    public float hoverYOffset = 37f;
    public ScryUISelector scryUISelector;
    private bool mouseOver = false;

    Coroutine start;
    Coroutine stop;

    // Start is called before the first frame update
    void Start()
    {   
        cardDisplay = GetComponent<CardDisplay>();
        handManager = GameObject.Find("HandManager").GetComponent<HandManager>();
        STM = GameObject.Find("SingleTargetManager").GetComponent<SingleTargetManager>();
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        siblingIndexOriginal = transform.GetSiblingIndex();
    }

    void Update () {
        // if(mouseOver) {
            if (Input.GetMouseButtonDown(1)) {
                isCancelled = true;
                if (isSelected) {
                    // Debug.Log("cancelling action (right click)");
                    if(cursorFollowerInstance != null) {
                        cursorFollowerInstance.SetActive(false);
                        Cursor.visible = true;
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
                    // Debug.Log("drag exit");
                    if (cursorFollowerInstance != null) {
                            cursorFollowerInstance.SetActive(false);
                            Cursor.visible = true;
                    }

                    if(!battleManager.CheckCanAct(cardDisplay.card) || 
                        (!battleManager.CheckBlasterCanAct(cardDisplay) && cardDisplay.card.type == "Blaster")) {
                        if(!isTarget) {
                            // for cards that are not target cards, move it back
                            transform.localPosition = originalPosition;
                        }
                        isSelected = false;
                        isFollowerPlaced = false;
                        isCardPlayed = false;
                        // isHardReset = true;

                    }
                    else{
                        // if it's a target but the STM doesn't have a target, that's bad
                        if (isTarget && STM.GetTarget() == null) {
                            isSelected = false;
                            isFollowerPlaced = false;
                            isCardPlayed = false;
                            // ExitResetSequence();
                        }
                        else {
                            // if it's a target card, then lock the STM
                            if (isTarget) {
                                STM.targetLocked = true;
                                // this line of code is hugely important
                                cardDisplay.card.target = STM.GetTarget();
                            }
                            isSelected = false;
                            isCardPlayed = true;

                            // add card play animation here
                            StartCoroutine(CardPlayAnimation(0.05f, true));
                            StartCoroutine(CardPlayDelaySequence(0.2f));
                        }
                    }
                }
                else if (isHardReset) {
                    isSelected = false;
                    isHardReset = false;
                    isFollowerPlaced = false;
                    isCardPlayed = false;
                    transform.localPosition = originalPosition;
                }
                isCancelled = false;
            }
        // }
    }

    private IEnumerator CardPlayDelaySequence(float time) {
        yield return new WaitForSeconds(time);
        // perform actions for Target cards exit sequence
        if (isFollowerPlaced) {
            isFollowerPlaced = false;
        }
        battleManager.isScryComplete = false;
        handManager.PlayCardBattleManager();
        yield return new WaitForSeconds(1.3f);

        isCardPlayed = false;
    }

    public IEnumerator CardPlayAnimation(float timeInterval, bool isHandCard) {
        // play card now, but defer the deletion for 1.7s
        if(isHandCard) {
            handManager.PlayCard(cardDisplay);
            handManager.lastCard = cardDisplay.card;
        }
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

        handManager.DeleteCardWithDiscard(cardDisplay, 0f);
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
        cardDisplay.glowImage.gameObject.SetActive(false);
        if (!isCardPlayed) {
            transform.SetSiblingIndex(siblingIndexOriginal);
            // Debug.Log("sibling index: " + transform.GetSiblingIndex());
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
        mouseOver = true;
        cardDisplay.glowImage.gameObject.SetActive(true);
        if (!isCardPlayed) {
            // check if it's a Target card first
            isTarget = cardDisplay.card.isTarget;
            // Debug.Log("isTarget: " + isTarget);

            // if it's a Target, instantiate the CursorFollower prefab
            if(isTarget) {
                // if(followerCreated) {
                //     Destroy(cursorFollowerInstance);
                //     followerCreated = false;
                // }
                if(!followerCreated) {
                    cursorFollowerInstance = Instantiate(cursorFollowerPrefab);
                    cursorFollowerInstance.SetActive(false);
                    cursorFollowerInstance.transform.parent = transform.parent;
                    cursorFollowerInstance.transform.SetSiblingIndex(9999);
                    cursorFollowerInstance.transform.localScale = new Vector3(1182.52f, 1182.52f, 1182.52f);
                    followerCreated = true;
                }
            }

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
        mouseOver = false;
        ExitResetSequence();
    }

    public void OnDrag(PointerEventData eventData)
    {
        cardDisplay.cardHoverDescription.pointerDown = true;
        isSelected = true;
        if(!isTarget) {
            // Debug.Log("input y pos: " + Input.mousePosition.y);
            if(Input.mousePosition.y > 420) {
                isHardReset = false;
            }
            else {
                isHardReset = true;
            }
        }

        if(isTarget) {
            if(!isFollowerPlaced) {
                Cursor.visible = false;
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

    public void OnEndDrag(PointerEventData pointerEventData) {
        cardDisplay.cardHoverDescription.pointerDown = false;
    }

    private void OnMouseDrag() {
        // isSelected = true;
        // if(!isTarget) {
        //     // Debug.Log("input y pos: " + Input.mousePosition.y);
        //     if(Input.mousePosition.y > 420) {
        //         isHardReset = false;
        //     }
        //     else {
        //         isHardReset = true;
        //     }
        // }

        // if(isTarget) {
        //     if(!isFollowerPlaced) {
        //         Cursor.visible = false;
        //         cursorFollowerInstance.SetActive(true);
        //         cursorFollowerInstance.transform.localPosition = transform.localPosition;
        //         isFollowerPlaced = true;
        //     }
        // }
        // else if(!isTarget && !isCancelled) {
        //     Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //     Vector3 translatedWorldPosition = new Vector3(screenToWorld.x, screenToWorld.y, Camera.main.nearClipPlane);
        //     transform.position = translatedWorldPosition;
        // }
    }



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
        transform.SetSiblingIndex(9998);

        Vector3 currentPosition = transform.localPosition;
        Vector3 currentScale = transform.localScale;
        // Debug.Log("originalScale: " + originalScale);
        for (float i = 0f; i <= 1f; i+= 0.1f) {
            transform.localPosition = new Vector3(currentPosition.x, Mathf.Lerp(currentPosition.y, hoverYOffset, Mathf.SmoothStep(0, 1, i)), currentPosition.z);

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

    private IEnumerator ShiftCardLeft(int index) {
        Vector3 cardOriginalPos = handManager.handCards[index].transform.localPosition;
        for (float i = 0f; i <= 1f; i+= 0.1f) {
            handManager.handCards[index].transform.localPosition = new Vector3(
                Mathf.Lerp(cardOriginalPos.x, cardOriginalPos.x - 155, Mathf.SmoothStep(0, 1, i)),
                cardOriginalPos.y,
                cardOriginalPos.z
            );
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator ShiftCardRight(int index) {
        Vector3 cardOriginalPos = handManager.handCards[index].transform.localPosition;
        for (float i = 0f; i <= 1f; i+= 0.1f) {
            handManager.handCards[index].transform.localPosition = new Vector3(
                Mathf.Lerp(cardOriginalPos.x, cardOriginalPos.x + 155, Mathf.SmoothStep(0, 1, i)),
                cardOriginalPos.y,
                cardOriginalPos.z
            );
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator ExitShrink() {
        // transform.localScale = expandedScale;
        transform.rotation = originalRotation;

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
