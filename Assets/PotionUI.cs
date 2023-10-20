using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PotionUI : MonoBehaviour
{
    // public PotionUIButton potion1;
    // public PotionUIButton potion2;
    // public PotionUIButton potion3;
    public GameObject potionUIButtonPrefab;
    public List<GameObject> potionSlots;
    public List<GameObject> potionsInstances;
    private static PotionUI _instance;

    public static PotionUI Instance 
    { 
        get { return _instance; } 
    } 


    void Awake()
    {
        if (_instance != null && _instance != this) 
        { 
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        potionsInstances = new List<GameObject>();
        // UsePotion(0);
        // UsePotion(1);
        // UsePotion(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipPotion(Potion potion) {
        // if(slot == 0) {
        GameObject newPotion = Instantiate(potionUIButtonPrefab, transform);
        // Vector3 potPos = newPotion.transform.localPosition;
        newPotion.GetComponent<PotionUIButton>().SetPotion(potion);
        potionsInstances.Add(newPotion);
        newPotion.GetComponent<PotionUIButton>().slot = potionsInstances.Count - 1;

        for(int i = 0; i < potionSlots.Count; i++) {
            if(potionsInstances.Count - 1 <= i) {
                newPotion.transform.localPosition = new Vector3(i * 105, 0, 0);
                potionSlots[i].SetActive(false);
                break;
            }
        }
            // potion1.gameObject.SetActive(true);
            // potion1.SetPotion(potion);
        // potion1slot.SetActive(false);
        // }
        // else if(slot == 1) {
        //     potion2.gameObject.SetActive(true);
        //     potion2.SetPotion(potion);
        //     potion2slot.SetActive(false);
        // }
        // else if(slot == 2){
        //     potion3.gameObject.SetActive(true);
        //     potion3.SetPotion(potion);
        //     potion3slot.SetActive(false);
        // }
    }

    public void UsePotion(int slot) {
        GameObject destroy = potionsInstances[slot];
        potionsInstances.RemoveAt(slot);
        Destroy(destroy);
        // potionsInstances.RemoveAll(s => s == null);
        for(int i = 0; i < potionSlots.Count; i++) {
            if(potionsInstances.Count > i){
                potionsInstances[i].transform.localPosition = new Vector3(i * 105, 0, 0);
                potionsInstances[i].GetComponent<PotionUIButton>().slot = i;
                potionSlots[i].SetActive(false);
            }
            else {
                potionSlots[i].SetActive(true);
            }
        }
    }
}
