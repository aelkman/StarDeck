using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoComplete : MonoBehaviour
{
    public TypewriterText signature;
    public TypewriterText mainText;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Sequence());
    }

    private IEnumerator Sequence() {
        CreateUserNameWithText();
        yield return new WaitUntil(() => mainText.doneTyping);
        yield return new WaitForSeconds(1.5f);
        signature.gameObject.SetActive(true);
        yield return new WaitUntil(() => signature.doneTyping);
        yield return new WaitForSeconds(1.0f);
        // links.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateUserNameWithText() {
        string nameString = "Congrats <u>" + Steamworks.SteamClient.Name + "</u>";
        nameString += mainText.text.text;
        mainText.text.text = nameString;
    }

    public void ExitClick() {
        AudioManager.Instance.PlayButtonPress();
        gameObject.SetActive(false);
    }
}
