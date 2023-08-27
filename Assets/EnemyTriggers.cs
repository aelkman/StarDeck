using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyTriggers : MonoBehaviour
{
    public bool isEnemyCollision;
    public Animator transition;
    public float transitionTime = 1.0f;

    // Start is called before the first frame update
    void Start() {
        isEnemyCollision = false;
    }
    private void OnCollisionEnter2D(Collision2D other)  {
        isEnemyCollision = true;
        Debug.Log("enemy collision!");
    }

    void Update()
    {
        if(isEnemyCollision) {
            isEnemyCollision = false;
            LoadNextLevel();
        }
    }

    public void LoadNextLevel() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int sceneIndex) {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }
}
