using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgusManager : MonoBehaviour {

	public static AgusManager instance = null;

	public BallScript ball;
	public List<GameObject> terrain;
	public List<GameObject> obstacles;
	public GameObject wave;
	public GameObject hurdlePrefab;
	public GameObject palmPrefab;
	public GameObject pausePanel;
	public GameObject gameOverPanel;

	public Text scoreText;
	int score = 0;
	public Text metersText;
	float meters = 0;

	bool paused = false;
	Vector3 size;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	void Start () {
		pausePanel.SetActive (false);
		gameOverPanel.SetActive (false);
	}
	
	void Update () {
		float ballPosition = ball.gameObject.transform.position.z;
		TrackManager.instance.updateTrack (ballPosition);
	}

	void moveWave() {
		
	}

	void AddTerrain() {
	}

	void addObstacle(float zpos) {
		int rand = (int) (Random.value * 100f);

		if (rand < 30) {
			int pos = rand % 2 == 0 ? 1 : -1;
			string obsType = rand % 3 == 0 ? "palm" : "hurdle";
			if (obsType == "palm") {
				obstacles.Add (Instantiate (palmPrefab, new Vector3 (pos, 0, zpos), Quaternion.identity));
			} else {
				obstacles.Add(Instantiate (hurdlePrefab, new Vector3 (pos, 0, zpos), Quaternion.identity));
			}
		}
	}

	void removeObstacles() {
		if (obstacles.Count > 0) {
			GameObject hurdle = obstacles [0];
			if (ball.gameObject.transform.position.z > hurdle.transform.position.z + 10f) {
				obstacles.RemoveAt (0);
				Destroy (hurdle);
			}
		}
	
	}

	public void GameOver() {
		StartCoroutine (gameOverRoutine());
	}

	IEnumerator gameOverRoutine() {
		ball.GetComponent<Animation>().Play();
		ball.PauseBall (true);
		yield return new WaitForSeconds (1.5f);
		gameOverPanel.SetActive (true);
	}

	public void PauseGame () {
		paused = !paused;
		if (paused) {
			pausePanel.SetActive (true);
		} else {
			pausePanel.SetActive (false);
		}
		ball.PauseBall (paused);
	}

	public void RestartGame() {
		pausePanel.SetActive (false);
		gameOverPanel.SetActive (false);
		paused = false;
		ball.restartGame ();
		ObstaclesManager.instance.restartGame ();
		TrackManager.instance.restartGame ();
		score = 0;
		meters = 0;
	}

	public void addScore(int s) {
		score += s;
		scoreText.text = score + "";
	}
}
