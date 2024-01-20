using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
	public Button restartButton;

	void Start () {
		Button btn = restartButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
		// Debug.Log ("You have clicked the button!");
        GameManager.Instance.RestartGame();
	}

    void OnClick() {

    }
}
