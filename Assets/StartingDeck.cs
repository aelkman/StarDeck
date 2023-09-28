using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "New Starting Deck", menuName = "Starting Deck")]
public class StartingDeck : ScriptableObject
{
    public List<Card> cardList;
}
