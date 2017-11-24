using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesManager : MonoBehaviour {

	public static ObstaclesManager instance = null;

	// Obstacles prefabs
	public GameObject hurdle1Prefab;
	public GameObject hurdle2Prefab;
	public GameObject hurdle3Prefab;
	public GameObject palmPrefab;
	public GameObject grassPrefab;

	private List<GameObject> hurdle1Pool = new List<GameObject> ();
	private List<GameObject> hurdle2Pool = new List<GameObject> ();
	private List<GameObject> hurdle3Pool = new List<GameObject> ();
	private List<GameObject> palmPool    = new List<GameObject> ();
	private List<GameObject> grassPool   = new List<GameObject> ();

	private List<GameObject> obstacles = new List<GameObject> ();

	public class ObstacleTag	{
		public static string HURDLE_1 = "hurdle1";
		public static string HURDLE_2 = "hurdle2";
		public static string HURDLE_3 = "hurdle3";
		public static string PALM     = "palm";
		public static string GRASS    = "grass";
	}

	// Use this for initialization
	void Start () {
		generatePools ();
	}

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	private void generatePools() {
		for (int i = 0; i < 10; i++) {
			GameObject hurdle1 = Instantiate (hurdle1Prefab, new Vector3 (0, 0, 0), Quaternion.identity);
			hurdle1.name = ObstacleTag.HURDLE_1;
			hurdle1.SetActive (false);
			hurdle1Pool.Add (hurdle1);

			GameObject hurdle2 = Instantiate (hurdle2Prefab, new Vector3 (0, 0, 0), Quaternion.identity);
			hurdle2.name = ObstacleTag.HURDLE_2;
			hurdle2.SetActive (false);
			hurdle2Pool.Add (hurdle2);

			GameObject hurdle3 = Instantiate (hurdle3Prefab, new Vector3 (0, 0, 0), Quaternion.identity);
			hurdle3.name = ObstacleTag.HURDLE_3;
			hurdle3.SetActive (false);
			hurdle3Pool.Add (hurdle3);

			GameObject palm = Instantiate (palmPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
			palm.name = ObstacleTag.PALM;
			palm.SetActive (false);
			palmPool.Add (palm);

			GameObject grass = Instantiate (grassPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
			grass.name = ObstacleTag.GRASS;
			grass.SetActive (false);
			grassPool.Add (grass);
		}
	}

	public void returnObstacleToPool() {
		GameObject firstObstacle = obstacles [0];
		obstacles.RemoveAt (0);
		returnObstacleToPool (firstObstacle);
	}

	private void returnObstacleToPool(GameObject obstalce) {
		obstalce.SetActive (false);
		switch (obstalce.name) {
		case "hurdle1":
			hurdle1Pool.Add (obstalce);
			break;
		case "hurdle2":
			hurdle2Pool.Add (obstalce);
			break;
		case "hurdle3":
			hurdle3Pool.Add (obstalce);
			break;
		case "palm":
			palmPool.Add (obstalce);
			break;
		case "grass":
			grassPool.Add (obstalce);
			break;
		default:
			break;
		}
	}

	public void putObstacle() {
		List<GameObject> track = TrackManager.instance.getTrack ();
		GameObject lastTrack = track [track.Count - 1];
		if (lastTrack.name == "small_bridge") {
			return;
		}

		GameObject obstacle = getRandomObstacle ();

		if (obstacle.name == "grass" && lastTrack.name == "bridge") {
			returnObstacleToPool (obstacle);
			return;
		}

		int xPos = Random.Range (-1, 1);
		obstacle.transform.position = new Vector3 (xPos, 0, lastTrack.transform.position.z);
		obstacles.Add (obstacle);
	}

	private GameObject getObstacleFromPool(List<GameObject> pool) {
		GameObject obstacle = pool [0];
		pool.RemoveAt (0);
		obstacle.SetActive (true);
		return obstacle;
	}

	private GameObject getRandomObstacle() {
		int prob = Random.Range (1, 100);

		if (prob < 20) {
			return getObstacleFromPool (hurdle1Pool);
		} else if (prob < 40) {
			return getObstacleFromPool (hurdle2Pool);
		} else if (prob < 60) {
			return getObstacleFromPool (hurdle3Pool);
		} else if (prob < 80) {
			return getObstacleFromPool (palmPool);
		} else {
			return getObstacleFromPool (grassPool);
		}
	}

}
