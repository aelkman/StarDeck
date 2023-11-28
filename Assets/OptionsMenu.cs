using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu Instance;
    public GameObject menu;
    private bool menuActive = false;

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape) && SceneManager.GetActiveScene().name != "MainMenu") {
            AudioManager.Instance.PlayButtonPress();
            menuActive = !menuActive;
            menu.SetActive(menuActive);
        }
    }

    public void BackButton() {
        AudioManager.Instance.PlayButtonPress();
        SetActive(false);
    }

    public void QuitClick() {
        StartCoroutine(QuitClickTimed());
    }

    public IEnumerator QuitClickTimed() {
        AudioManager.Instance.PlayButtonPress();
        yield return new WaitForSeconds(0.5f);
        SetActive(false);
        Application.Quit();
    }

    public void MainMenuButton() {
        StartCoroutine(MainMenuButtonDelayed());
    }

    private IEnumerator MainMenuButtonDelayed() {
        AudioManager.Instance.PlayButtonPress();
        yield return new WaitForSeconds(0.5f);
        SetActive(false);
        GameManager.Instance.RestartToScene("MainMenu", null);
    }

    public void RestartButton() {
        StartCoroutine(RestartButtonDelayed());
    }

    private IEnumerator RestartButtonDelayed() {
        AudioManager.Instance.PlayButtonPress();
        yield return new WaitForSeconds(0.5f);
        SetActive(false);
        GameManager.Instance.RestartGame();
    }

    private void SetActive(bool active) {
        menuActive = active;
        menu.SetActive(menuActive);
    }

    public void ToggleButton() {
        menuActive = !menuActive;
        menu.SetActive(menuActive);
    }
}
