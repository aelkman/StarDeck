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
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }

    IEnumerator LoadLevel(int sceneIndex) {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene(sceneIndex);
        
    }

    private void UpdatePlayerStats() {
        if(MainManager.Instance != null) {
            MainManager.Instance.playerHealth = playerStats.health;
        }
    }
}
