using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScryUISelector : MonoBehaviour
{
    public DeckCopy deckCopy;
    private int addedCount = 0;
    public GameObject prefab;
    public int scryCount = 5;
    // public MainManager mainManager;
    public List<GameObject> selectedCards;
    public List<GameObject> shownCards;
    public HandManager handManager;
    // Start is called before the first frame update

    void Start() {
        handManager = GameObject.Find("HandManager").GetComponent<HandManager>();
    }

    void OnEnable()
    {
        StartCoroutine(WaitForDeck());
    }

    private IEnumerator WaitForDeck() {
        yield return new WaitUntil(() => deckCopy.isInitialized);
        if(scryCount > deckCopy.cardStack.Count()) {
            scryCount = deckCopy.cardStack.Count();
            Debug.Log("scryCount: " + scryCount);
        }
        for (int i = 0; i < scryCount; i++) {
            int index = deckCopy.cardStack.Count() - 1 - i;
            Debug.Log("cardStack size: " + deckCopy.cardStack.Count());
            Debug.Log("attempted index: " + index);
            CreateCard(deckCopy.cardStack.items[index], index, i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void ContinueClick() {
        int indexModifier = 0;
        for(int i = selectedCards.Count - 1; i >= 0; i--) {
            int artificialIndex = deckCopy.cardStack.Count();
        // foreach(GameObject go in selectedCards) {
            GameObject go = selectedCards[i];
            go.GetComponent<ScryUIActions>().CardPlay();
            // Debug.Log("removing card at deck index: " + go.GetComponent<ScryUIActions>().deckIndex);
            // int newIndex = go.GetComponent<ScryUIActions>().deckIndex - indexModifier;
            // Debug.Log("selected card: " + selectedCards[i].GetComponent<CardDisplay>().card.name);
            // Debug.Log("removing deck card " + deckCopy.cardStack.items[newIndex].name);
            deckCopy.cardStack.items.Remove(go.GetComponent<CardDisplay>().card);
            // // deckCopy.cardStack.Remove(newIndex);
            // Debug.Log("removed card at index: " + newIndex);
            // if (go.GetComponent<ScryUIActions>().deckIndex < i) {
            //     indexModifier += 1;
            // }

        }
        foreach(GameObject shownGo in shownCards) {
            if(selectedCards.Contains(shownGo)) {
                handManager.DeleteCardWithDiscard(shownGo.GetComponent<CardDisplay>(), 1.7f);
            }
            else {
                handManager.DeleteCardNoDiscard(shownGo.GetComponent<CardDisplay>(), 1.7f);
            }
        }
        selectedCards = new List<GameObject>();
        shownCards = new List<GameObject>();
        handManager.DisableScryViewer();
    }

    public Card GetRandomCard(List<Card> cardList) {
        // Random.seed = System.DateTime.Now.Millisecond;
        // Random.Range with ints is (inclusive, exclusive)
        return (Card)cardList[UnityEngine.Random.Range(0, cardList.Count)];
    }

    public void CreateCard(Card card, int deckIndex, int i) {
        prefab.GetComponent<CardDisplay>().card = card;
        prefab.GetComponent<ScryUIActions>().deckIndex = deckIndex;
        GameObject cardInstance = Instantiate(prefab, new Vector3(-20f, 30f, 0f), Quaternion.identity, transform.GetChild(0));
        float fract = (float)i/((float)scryCount - 1f);
        Debug.Log("fract: " + fract);
        float totalSpace = scryCount * 225f;
        cardInstance.transform.localPosition = new Vector3(Mathf.Lerp(-1f * totalSpace/2, totalSpace/2, fract), cardInstance.transform.position.y, cardInstance.transform.position.z);
        cardInstance.transform.localScale = new Vector3(3.0f, 3.0f, 0f);
        shownCards.Add(cardInstance);
    }

    // public bool AddToDeck(Card card) {
    //     if (addedCount < 1) {
    //         deck.AddCard(card);
    //         addedCount++;
    //         return true;
    //     }
    //     else {
    //         Debug.Log("you already added a card!");
    //         return false;
    //     }
    // }
}
