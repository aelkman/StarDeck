using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using System.Linq;

public class CardViewScroller : MonoBehaviour
{
    List<Card> cards;
    public CardDisplay cardDisplay;
    public TextMeshProUGUI countText;
    // Start is called before the first frame update
    void Start()
    {
        cards = Resources.LoadAll<Card>("Cards").ToList();
        foreach(Card card in Resources.LoadAll<Card>("Cards_Speial")) {
            cards.Add(card);
        }
        foreach(Card card in Resources.LoadAll<Card>("CardsEvents")) {
            cards.Add(card);
        }
        StartCoroutine(CardScrolls());
    }

    private IEnumerator CardScrolls() {
        
        var count = 1;
        Shuffle();

        yield return new WaitForSeconds(1.0f);

        foreach(Card card in cards) {
            cardDisplay.card = card;
            cardDisplay.UpdateCard();
            countText.text = count.ToString();
            count++;
            yield return new WaitForSeconds(0.9f);
        }   
    }

    public void Shuffle() {
        // List<Card> cardList = cardStack;

        for (var i = cards.Count() - 1; i > 0; i--)
        {
            var temp = cards[i];
            var index = UnityEngine.Random.Range(0, i + 1);
            cards[i] = cards[index];
            cards[index] = temp;
        }
        // cardStack = new Stack<Card>(cardList);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
