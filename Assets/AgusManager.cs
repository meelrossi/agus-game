using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgusManager : MonoBehaviour {

	public static AgusManager instance = null;

	public GameObject ball;
	public List<GameObject> terrain;
	public List<GameObject> obstacles;
	public GameObject wave;
	public GameObject hurdlePrefab;
	public GameObject palmPrefab;

	Vector3 size;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	void Start () {

	}
	
	void Update () {
		float ballPosition = ball.transform.position.z;
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
			if (ball.transform.position.z > hurdle.transform.position.z + 10f) {
				obstacles.RemoveAt (0);
				Destroy (hurdle);
			}
		}
	
	}


}
