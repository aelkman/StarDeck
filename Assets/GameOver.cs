using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public Animator transition;
    public HandManager handManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initiate() {
        gameObject.SetActive(true);
        handManager.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void RestartToMainMenu() {
        GameManager.Instance.RestartToScene("MainMenu", transition);
    }
}
