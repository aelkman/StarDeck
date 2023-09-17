using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public Animator transition;
    private CanvasGroup cg;
    public void Start()
    {
        if (transition == null) {
            transition = GameObject.Find("Crossfade").GetComponent<Animator>();
        }
        // reset animator to Entry state
        transition.Rebind();
        transition.Update(0f);
        cg = transition.gameObject.GetComponent<CanvasGroup>();
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        Debug.Log("Start: SceneLoaded1");
    }

    private void OnSceneUnloaded(Scene current)
    {
        Debug.Log("OnSceneUnloaded: " + current);
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    void Update()
    {
        if (Input.GetKey("space"))
        {
            Debug.Log("Quitting Scene1");
            ChangeScene();
        }
    }

    void ChangeScene()
    {
        Debug.Log("Changing to Scene2");

        SceneManager.LoadScene("Scene2");
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy");
    }
}
