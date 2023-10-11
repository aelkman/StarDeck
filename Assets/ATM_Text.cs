using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ATM_Text : MonoBehaviour
{
    public TextMeshProUGUI text;
    private string descriptionString;
    private List<string> words;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "";
        descriptionString = "Walking on the street, you happen upon an ATM machine.<br><br>You think to yourself,<br><br><i>\"I could use some money.\"</i><br><br>What would you like to do?";
        words = new List<string>();
        words.Add("Walking on the street, you happen upon an ATM machine.");
        words.Add("<br><br>");
        words.Add("You think to yourself,");
        words.Add("<br><br><i>");
        words.Add("\"I could use some money.\"");
        words.Add("</i><br><br>");
        words.Add("What would you like to do?");
        StartCoroutine(TypeTimed());
    }

    public IEnumerator TypeTimed() {
        float timeInterval = 0.016667f;
        float time = 10f;

        // var charArr = descriptionString.ToCharArray();
        // int length = charArr.Length;

        foreach(var word in words) {
            var charArr = word.ToCharArray();
            if(charArr[0] == '<') {
                text.text += word;
                yield return new WaitForSeconds(0.5f);
            }
            else {
                for(int i = 0; i < charArr.Length; i++){
                    text.text += charArr[i];
                    yield return new WaitForSeconds(timeInterval);
                }
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
