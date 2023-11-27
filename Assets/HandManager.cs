using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandManager : MonoBehaviour
{
    public CardDisplay prefab;
    public GameObject drawingDeck;
    public GameObject animatorPrefab;
    public DeckCopy deckCopy;
    public List<CardDisplay> handCards;
    private List<CardDisplay> discardCards;
    public List<CardDisplay> expelCards;
    private float zRot = 1.0f;
    private float yOffset = 15.0f;
    private float xOffset = 200;
    public PlayerStats playerStats;
    public GameObject scryViewer;
    public ScryUISelector scryUISelector;
    public Card lastCard;
    public BattleManager battleManager;
    public BattleEnemyManager BEM;
    public AudioSource drawAudio;
    public AudioSource damageAudio;
    private CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        scryViewer.SetActive(false);
        expelCards = new List<CardDisplay>();
        handCards = new List<CardDisplay>();
        discardCards = new List<CardDisplay>();
    }

    public void DrawCards(int cardCount) {
        StartCoroutine(DrawCardsTimed(cardCount, cardsReturnValue => {}));
    }

    public void Scry(int scryCount) {
        StartCoroutine(ScryTimed(scryCount));
    }

    private IEnumerator ScryTimed(int scryCount) {
        yield return new WaitForSeconds(0.3f);
        scryUISelector.scryCount = scryCount;
        scryViewer.SetActive(true);
    }

    public void DisableScryViewer() {
        StartCoroutine(DisableScryTimed());
    }

    private IEnumerator DisableScryTimed() {
        yield return new WaitForSeconds(0.75f);
        scryViewer.SetActive(false);
    }

    public IEnumerator DrawCardsTimed(int cardCount, System.Action<List<Card>> cardsCallback) {

        canvasGroup.blocksRaycasts = false;

        List<Card> cards = new List<Card>();
        for(int i = 0; i < cardCount; i++) {
            Debug.Log("drawing card: " + i+1);
            if(MainManager.Instance.artifacts.Contains("DEF_DRAW")) {
                if(playerStats.block < 1) {
                    playerStats.characterAnimator.BlockAnimation();
                    playerStats.shieldAnimator.StartForceField();
                }
                playerStats.addBlock(1);
            }
            if(MainManager.Instance.artifacts.Contains("ATK_DRAW")) {
                int randIndex = UnityEngine.Random.Range(0, BEM.GetBattleEnemies().Count);
                var randTarget = BEM.GetBattleEnemies()[randIndex];
                damageAudio.Stop();
                damageAudio.Play();
                StartCoroutine(randTarget.TakeDamage(1, 0.2f, "Artifacts", returnValue => {}));
            }
            else {
                drawAudio.Play();
            }
            if(deckCopy.cardStack.Count() < 1) {
                for (int j = 0; j < discardCards.Count; j = 0) {
                    CardDisplay cardDisplay = discardCards[j];
                    deckCopy.cardStack.Push(cardDisplay.card);
                    discardCards.Remove(cardDisplay);
                }
                deckCopy.Shuffle();
            }

            Card currentCard = deckCopy.cardStack.Pop();
            if(lastCard != null && lastCard.name == "Meditation") {
                currentCard.manaCost = 0;
                currentCard.actions.Add("EXPEL", "");
            }
            cards.Add(currentCard);

            prefab.card = currentCard;
            // GameObject animatorInstance = Instantiate(animatorPrefab, transform);
            // animatorInstance.transform.GetChild(0).GetComponent<CardDisplay>().card = currentCard;
            // CardDisplay cardInstance = animatorInstance.transform.GetChild(0).GetComponent<CardDisplay>();
            CardDisplay cardInstance = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
            SetCardDefaultScalePos(cardInstance);
            handCards.Add(cardInstance);
            SortCards();
            yield return new WaitForSeconds(0.2f);
        }

        canvasGroup.blocksRaycasts = true;
        cardsCallback(cards);
    }

    void Update() {
        // SortCards();
    }

    private void SetCardDefaultScalePos(CardDisplay cardInstance) {
        cardInstance.transform.position = drawingDeck.transform.position;
        // cardInstance.transform.localPosition = new Vector3(0,0,0);
        // cardInstance.transform.SetParent(this.transform);
        cardInstance.transform.localScale = new Vector3(3.0f, 3.0f, 0f);
    }

    public List<CardDisplay> GetDiscards() {
        return discardCards;
    }

    // inverse of y = x²(3-2x)
    float InverseSmoothstep( float x )
    {
        return 0.5f-(float)Math.Sin(Math.Asin(1.0f-2.0f*x)/3.0f);
    }

    public void PlayCard(CardDisplay cardDisplay) {
        // remove from hand, then sort
        // lastCard = cardDisplay.card;
        handCards.Remove(cardDisplay);
        SortCards();
        // defer deletion & removal by 1.7s
        // StartCoroutine(DeferCardDeletion(cardDisplay, 1.7f));
    }

    public void DeleteCardNoDiscard(CardDisplay cardDisplay, float delay) {
        StartCoroutine(DeferCardDeletionNoDiscard(cardDisplay, delay));
    }

    public void DeleteCardWithDiscard(CardDisplay cardDisplay, float delay) {
        StartCoroutine(DeferCardDeletion(cardDisplay, delay));
    }

    private IEnumerator DeferCardDeletionNoDiscard(CardDisplay cardDisplay, float time) {
        yield return new WaitForSeconds(time);
        cardDisplay.gameObject.SetActive(false);
        Destroy(cardDisplay.gameObject);
    }

    private IEnumerator DeferCardDeletion(CardDisplay cardDisplay, float time) {
        yield return new WaitForSeconds(time);
        if(cardDisplay.card.actions.ContainsKey("EXPEL")) {
            // make a copy since it is about to be deleted
            CardDisplay expelCard = Instantiate(cardDisplay);
            expelCards.Add(expelCard);
        }
        else {
            discardCards.Add(cardDisplay);
        }
        // next, need to remove GO from the Hand
        cardDisplay.gameObject.SetActive(false);
        Destroy(cardDisplay.gameObject);
    }

    public void PlayCardBattleManager() {
        StartCoroutine(battleManager.CardAction(lastCard));
        // if(cardDisplay.card.actionKeys.Contains("SCRY")) {
        //     yield return new WaitUntil(() => battleManager.isScryComplete);
        // }
    }

    private void NegativeDiscards(CardDisplay cardDisplay) {
        if(cardDisplay.card.name == "Virus") {
            // duplicate
            discardCards.Add(cardDisplay);
            // deal 5 damage to player
            playerStats.takeDamage(5, null);
        }
    }

    public void DiscardHand() {
        for(int i=0; i< handCards.Count; i=0) {
            CardDisplay cardDisplay = handCards[i];
            // negative cards effects for discard here
            NegativeDiscards(cardDisplay);
            handCards.Remove(cardDisplay);
            discardCards.Add(cardDisplay);
            cardDisplay.gameObject.SetActive(false);
            DestroyImmediate(cardDisplay.GetComponent<CardMouseActions>().GetCursorFollowerInstance());
            DestroyImmediate(cardDisplay.gameObject);
        }
    }

    private void SortCards() {
        for(int i = 0; i < handCards.Count; i++) {
            float alignResult = i / (handCards.Count - 1.0f);
            StartCoroutine(MoveCard(handCards[i], alignResult, 0.1f));
        }
    }

    private IEnumerator MoveCard(CardDisplay cardDisplay, float alignResult, float timeInterval) {

        cardDisplay.pointerBoundary.SetActive(false);
        int handCardsLess1 = handCards.Count - 1;
        if(handCards.Count == 1) {
            alignResult = 1;
            handCardsLess1 = 2;
        }
        for (float i = 0f; i <= 1f; i+= timeInterval) {

            Vector3 originalPosition = cardDisplay.transform.localPosition;
            float newXPos = Mathf.Lerp(-xOffset/2 * (handCardsLess1), xOffset/2 * (handCardsLess1), alignResult);
            float newYPos = -Mathf.Abs(Mathf.Lerp((handCardsLess1) * -yOffset, (handCardsLess1) * yOffset, InverseSmoothstep(alignResult)));

            cardDisplay.transform.localPosition = new Vector3(
                (Mathf.Lerp(originalPosition.x, newXPos, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(originalPosition.y, newYPos, Mathf.SmoothStep(0f, 1f, i))),
                0
            );

            float newRotationZ = Mathf.Lerp((handCardsLess1) * zRot, (handCardsLess1) * -zRot, alignResult);
            Quaternion originalRotation = cardDisplay.transform.rotation;

            // Debug.Log("newZRot: " + newRotationZ);
            // Debug.Log("originalRot: " + originalRotation.eulerAngles.z);

            Vector3 currentAngle = new Vector3(0f, 0f, Mathf.Lerp(WrapAngle(originalRotation.eulerAngles.z), newRotationZ, Mathf.SmoothStep(0, 1, i)));
            cardDisplay.transform.eulerAngles = currentAngle;

            yield return new WaitForSeconds(0.01f);
        }
        cardDisplay.gameObject.GetComponent<CardMouseActions>().originalPosition = cardDisplay.transform.localPosition;
        cardDisplay.pointerBoundary.SetActive(true);
    }

    private static float WrapAngle(float angle)
    {
        angle%=360;
        if(angle >180)
            return angle - 360;

        return angle;
    }
}
