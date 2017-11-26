using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour {

	bool paused = false;
	public GameObject pauseButton;

	// Use this for initialization
	void Start () {
	}

	public void PauseGame() {
		if (!paused) {
			paused = true;
			gameObject.SetActive (true);
		} else {
			paused = false;
			gameObject.SetActive (false);
		}
	}
}
