using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WeaponsManager : MonoBehaviour
{
    public WeaponWindow weaponWindow1;
    public WeaponWindow weaponWindow2;
    public List<string> weapons = new List<string>() { "", ""};
    public Button embarkButton;

    // Start is called before the first frame update
    void Start()
    {
        weaponWindow1.gameObject.SetActive(false);
        weaponWindow2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(weapons.Contains("")) {
            embarkButton.interactable = false;
        }
        else {
            embarkButton.interactable = true;
        }
    }

    public void SetWeaponWindow(string weaponName) {
        int i = 0;
        for(i = 0; i < weapons.Count; i++) {
            if(weapons[i] == "") {
                weapons[i] = weaponName;
                break;
            }
        }
        // if(i == 2) {
        //     weapons[0] = weaponName;
        //     i = 0;
        // }
        // now i is the index to insert
        if(i == 0) {
            weaponWindow1.gameObject.SetActive(true);
            weaponWindow1.SetBody(weaponName);
        }
        else if(i == 1) {
            weaponWindow2.gameObject.SetActive(true);
            weaponWindow2.SetBody(weaponName);
        }
    }

    public void RemoveWeaponWindow(string weaponName) {
        int i = 0;
        for(i = 0; i < weapons.Count; i++) {
            if(weapons[i] == weaponName) {
                weapons[i] = "";
                break;
            }
        }
        if(i == 0) {
            weaponWindow1.gameObject.SetActive(false);
        }
        else if(i == 1) {
            weaponWindow2.gameObject.SetActive(false);
        }
    }
}
