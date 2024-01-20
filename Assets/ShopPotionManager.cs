using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPotionManager : MonoBehaviour
{
    public PotionShopButton potion1;
    public PotionShopButton potion2;
    public PotionShopButton potion3;
    public PotionShopButton potion4;
    private List<PotionShopButton> potInstances;
    public List<Potion> gauranteedPots = new List<Potion>();
    public List<Potion> randomPots = new List<Potion>();
    public int maxStorePots = 4;
    // Start is called before the first frame update
    void Start()
    {
        potInstances = new List<PotionShopButton> {potion1, potion2, potion3, potion4};
        int i = 0;
        for(i = 0; i < gauranteedPots.Count; i++){
            potInstances[i].SetPotion(gauranteedPots[i]);
        }
        for(int j = i; j < maxStorePots; j++) {
            // Debug.Log("j is " + j);
            var randomPot = randomPots[Random.Range(0, randomPots.Count)];
            randomPots.Remove(randomPot);
            potInstances[j].SetPotion(randomPot);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
