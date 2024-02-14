using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu Instance;
    public GameObject menu;
    public GameObject confirmLeave;
    private bool menuActive = false;
    private bool confirmLeaveBool = false;
    private bool backoutLeave = false;
    public TextMeshProUGUI confirmButtonText;

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
            if(confirmLeave.activeSelf == true) {
                backoutLeave = true;
            }
            AudioManager.Instance.PlayButtonPress();
            menuActive = !menuActive;
            menu.SetActive(menuActive);
            // backoutLeave = false;
        }
    }

    public void BackButton() {
        AudioManager.Instance.PlayButtonPress();
        SetActive(false);
    }

    public void ConfirmLeave() {
        AudioManager.Instance.PlayButtonPress();
        confirmLeaveBool = true;
    }

    public void BackOutLeave() {
        AudioManager.Instance.PlayButtonPress();
        backoutLeave = true;
    }

    public void QuitClick() {
        confirmButtonText.text = "Quit";
        confirmLeave.SetActive(true);
        StartCoroutine(QuitClickTimed());
    }

    public IEnumerator QuitClickTimed() {
        AudioManager.Instance.PlayButtonPress();

        yield return new WaitUntil(() => confirmLeaveBool || backoutLeave);
        
        if(confirmLeaveBool) {
            yield return new WaitForSeconds(0.5f);
            // SetActive(false);
            confirmLeaveBool = false;
            Application.Quit();
        }
        else if(backoutLeave) {
            confirmLeave.SetActive(false);
            backoutLeave = false;
        }

    }

    public void MainMenuButton() {
        confirmButtonText.text = "Home";
        confirmLeave.SetActive(true);
        StartCoroutine(MainMenuButtonDelayed());
    }

    private IEnumerator MainMenuButtonDelayed() {
        AudioManager.Instance.PlayButtonPress();

        yield return new WaitUntil(() => confirmLeaveBool || backoutLeave);

        if(confirmLeaveBool) {
            yield return new WaitForSeconds(0.5f);
            confirmLeave.SetActive(false);
            confirmLeaveBool = false;
            SetActive(false);
            GameManager.Instance.RestartToScene("MainMenu", null);
        }
        else if(backoutLeave) {
            confirmLeave.SetActive(false);
            backoutLeave = false;
        }
    }

    public void RestartButton() {
        confirmButtonText.text = "Restart";
        confirmLeave.SetActive(true);
        StartCoroutine(RestartButtonDelayed());
    }

    private IEnumerator RestartButtonDelayed() {
        AudioManager.Instance.PlayButtonPress();

        yield return new WaitUntil(() => confirmLeaveBool || backoutLeave);

        if(confirmLeaveBool) {
            yield return new WaitForSeconds(0.5f);
            confirmLeave.SetActive(false);
            confirmLeaveBool = false;
            SetActive(false);
            GameManager.Instance.RestartGame();
        }
        else if(backoutLeave) {
            confirmLeave.SetActive(false);
            backoutLeave = false;
        }
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
