using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public new string name;
    public Sprite artwork;
    public string typeString;
    public int manaCost;
    public int attack;
    public int health;
    public int defense;
    public string description;
    // private CardType type;
    // Start is called before the first frame update
    // public Card() {
        
    // }

    // Update is called once per frame
    // private void generateRandomCard() {
    //     type = new CardType("attack");
    //     // type.getRandomType();
    // }

    // private void Start() {
    //     this.type = new CardType();
    //     // generateRandomCard();
    // }
}

[System.Serializable]
public class CardType {

    public Dictionary<string, bool> types = new Dictionary<string, bool>();
    // public bool isAttack;
    // public bool isCast;
    // public bool isConsumeable;
    // public bool isDefend;

    private string attack = "attack";
    private string cast = "cast";
    private string consumeable = "consumeable";
    private string defend = "defend";

    // private List<string> typesList = ["attack", "cast", "consumeable", "defend"];

    // default constructor
    public CardType() {
        types[attack] = false;
        types[cast] = false;
        types[consumeable] = false;
        types[defend] = false;
    }

    public CardType(string type) {
        types[type] = true;
    }

    // public void getRandomType() {
    //     // List<string> typeList = types.ToList();
    //     List<string> keyList = new List<string>(types.Keys);
    //     types[keyList[Random.Range(0,keyList.Count)]] = true;

    //     // types[types.ElementAt(rand.Next(0, types.Count)).Value] = true;
    // }

}
