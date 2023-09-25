using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCardUISelector : CardUISelector
{
    public new void CreateCard(Card card, int i) {
        prefab.card = card;
        CardDisplay cardInstance = Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity, transform.GetChild(0));
        float fract = (float)i/((float)selectionCount - 1f);
        Debug.Log("fract: " + fract);
        cardInstance.transform.localPosition = new Vector3(Mathf.Lerp(-285f, 285f, fract), cardInstance.transform.position.y, cardInstance.transform.position.z);
        cardInstance.transform.localScale = new Vector3(3.0f, 3.0f, 0f);
    }

    public new bool AddToDeck(Card card) {
            deck.AddCard(card);
            return true;
    }
}
