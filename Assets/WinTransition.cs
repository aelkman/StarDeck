using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTransition : MonoBehaviour
{
    public Animator transition;
    public PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMapOnWin() {
        UpdatePlayerStats();
        AudioManager.Instance.PlayButtonPress();
        if(MainManager.Instance.isBossBattle) {
            GameManager.Instance.demoComplete = true;
            GameManager.Instance.RestartToScene("MainMenu", transition);
        }
        else {
            StartCoroutine(LoadLevel("Map"));
        }
    }

    IEnumerator LoadLevel(string sceneName) {
        transition.SetTrigger("Start");
        StartCoroutine(FadeAudioSource.StartFade(AudioManager.Instance.currentBattleMusic, 1, 0));
        yield return new WaitForSeconds(1.0f);

        GameManager.Instance.LoadScene(sceneName);
        
    }

    private void UpdatePlayerStats() {
        if(MainManager.Instance != null) {
            MainManager.Instance.playerHealth = playerStats.health;
        }
    }
}
