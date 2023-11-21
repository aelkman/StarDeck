using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveButton : MonoBehaviour
{
    public Animator crossfade;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Leave() {
        StartCoroutine(LeaveCoroutine());
    }

    IEnumerator LeaveCoroutine() {
        AudioManager.Instance.PlayButtonPress();
        crossfade.SetTrigger("Start");

        yield return new WaitForSeconds(1.0f);

        GameManager.Instance.LoadScene("Map");
    }
}
