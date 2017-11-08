using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgusManager : MonoBehaviour {

	public static AgusManager instance = null;

	public GameObject ball;
	public List<GameObject> terrain;
	public List<GameObject> obstacles;
	public GameObject groundPrefab;
	public GameObject wave;
	public GameObject hurdlePrefab;
	public GameObject palmPrefab;

	public GameObject starteEdgePrefab;
	public GameObject endEdgePrefab;

	Vector3 size;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	void Start () {
		
		Renderer[] renderer = groundPrefab.GetComponentsInChildren<Renderer> ();
		size = renderer[1].bounds.size * 0.9f;
		for (int i = -1; i <= 15; i++) {
			terrain.Add (Instantiate (groundPrefab, new Vector3 (0, 0, size.z * i), Quaternion.identity));
		}
	}
	
	void Update () {
		GameObject ground = terrain [4];
		if (ball.transform.position.z > ground.transform.position.z) {
			GameObject newGround = terrain [0];
			Vector3 newPosition = terrain [terrain.Count - 1].transform.position;
			newPosition.z = newPosition.z + size.z;

			int rand = (int) (Random.value * 100f);
			if (rand < 10) {
				newGround = Instantiate (endEdgePrefab, new Vector3 (0, 0, 0), Quaternion.identity);
				newPosition.z += 10f;
			}

			newGround.transform.position = newPosition;
			terrain.RemoveAt (0);
			terrain.Insert (terrain.Count, newGround);
			addObstacle (newPosition.z);
			removeObstacles ();

		}
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
