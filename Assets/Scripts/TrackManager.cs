using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour {

	public static TrackManager instance = null;

	// Track prefabs
	public GameObject smallBridgePrefab;
	public GameObject bridgePrefab;
	public GameObject groundPrefab;
	public GameObject starteEdgePrefab;
	public GameObject endEdgePrefab;
	public GameObject groundIslandPrefab;
	public GameObject coinPrefab;

	public GameObject wavePrefab;

	private List<GameObject> groundPool = new List<GameObject>();
	private List<GameObject> bridgePool = new List<GameObject>();
	private List<GameObject> smallBridgePool = new List<GameObject>();
	private List<GameObject> startEdgePool = new List<GameObject>();
	private List<GameObject> endEdgePool = new List<GameObject>();
	private List<GameObject> groundIslandPool = new List<GameObject>();

	private Vector3 groundSize;
	private Vector3 bridgeSize;
	private Vector3 smallBridgeSize;
	private Vector3 startEdgeSize;
	private Vector3 endEdgeSize;

	private List<GameObject> track = new List<GameObject> ();
	private List<GameObject> coins = new List<GameObject> ();
	private bool trackPassed = false;

	public class TrackTag	{
		public static string BRIDGE = "bridge";
		public static string SMALL_BRIDGE = "small_bridge";
		public static string GROUND = "ground";
		public static string START_EDGE = "start_edge";
		public static string END_EDGE = "end_edge";
		public static string GROUND_ISLAND = "ground_island";
	}

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	void Start () {
		Renderer[] renderer = groundPrefab.GetComponentsInChildren<Renderer> ();
		groundSize = renderer[2].bounds.size;

		renderer = smallBridgePrefab.GetComponentsInChildren<Renderer> ();
		smallBridgeSize = renderer[0].bounds.size;

		renderer = bridgePrefab.GetComponentsInChildren<Renderer> ();
		bridgeSize = renderer[0].bounds.size;

		renderer = starteEdgePrefab.GetComponentsInChildren<Renderer> ();
		startEdgeSize = renderer[1].bounds.size;

		renderer = endEdgePrefab.GetComponentsInChildren<Renderer> ();
		endEdgeSize = renderer[1].bounds.size;

		generatePools ();
		generateStartTrack ();
	}
	
	void Update () {
		
	}

	public List<GameObject> getTrack() {
		return track;
	}

	private void generatePools() {
		for (int i = 0; i < 10; i++) {
			GameObject bridge = Instantiate (bridgePrefab, new Vector3 (0, 0, 0), Quaternion.identity);
			bridge.name = TrackTag.BRIDGE;
			bridge.SetActive (false);
			bridgePool.Add (bridge);

			GameObject smallBridge = Instantiate (smallBridgePrefab, new Vector3 (0, 0, 0), Quaternion.identity);
			smallBridge.name = TrackTag.SMALL_BRIDGE;
			smallBridge.SetActive(false);
			smallBridgePool.Add (smallBridge);

			GameObject startEdge = Instantiate (starteEdgePrefab, new Vector3 (0, 0, 0), Quaternion.identity);
			startEdge.name = TrackTag.START_EDGE;
			startEdge.SetActive(false);
			startEdgePool.Add (startEdge);

			GameObject endEdge = Instantiate (endEdgePrefab, new Vector3 (0, 0, 0), Quaternion.identity);
			endEdge.name = TrackTag.END_EDGE;
			endEdge.SetActive(false);
			endEdgePool.Add (endEdge);

			GameObject groundIsland = Instantiate (groundIslandPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
			groundIsland.name = TrackTag.GROUND_ISLAND;
			groundIsland.SetActive(false);
			groundIslandPool.Add (groundIsland);
		}

		for (int i = 0; i < 30; i++) {
			GameObject ground = Instantiate (groundPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
			ground.name = TrackTag.GROUND;
			ground.SetActive (false);
			groundPool.Add (ground);
		}
	}
		
	private void generateStartTrack() {
		for (int i = -1; i < 15; i++) {
			GameObject ground = groundPool [0];
			ground.transform.position = new Vector3(0, 0, i * groundSize.z);
			groundPool.RemoveAt (0);
			ground.SetActive (true);
			track.Add (ground);
		}
	}

	public void updateTrack(float ballPosition) {
		GameObject ground = track [3];
		if (ballPosition > ground.transform.position.z) {
			returnTrackToPool (ballPosition);
			putNextTrack ();
			ObstaclesManager.instance.putObstacle ();

			GameObject obstacle = ObstaclesManager.instance.getObstacles ()[3];
			if (obstacle != null && ballPosition > obstacle.transform.position.z) {
				ObstaclesManager.instance.returnObstacleToPool (ballPosition);
			}

			putCoinsOnTrack ();
			removeUncollectedCoins (ballPosition);
		}
		Vector3 currPos = wavePrefab.transform.position;
		wavePrefab.transform.position = new Vector3 (currPos.x, currPos.y, ballPosition + 1000f);

	}

	private void putCoinsOnTrack() {

		GameObject lastTrack = track [track.Count - 1];
		GameObject beforeLastTrack = track [track.Count - 2];
		if (beforeLastTrack.name.Equals(TrackTag.END_EDGE) || beforeLastTrack.name.Equals(TrackTag.START_EDGE) ||
			lastTrack.name.Equals(TrackTag.END_EDGE) || lastTrack.name.Equals(TrackTag.START_EDGE)) {
			return;
		}
		int xPos = Random.Range (-1, 2);
		for (int i = 1; i < 5; i++) {
			GameObject coin = Instantiate (coinPrefab, new Vector3 (xPos, 0.4f, lastTrack.transform.position.z - (i*1.5f)), Quaternion.identity);
			coin.transform.Rotate (new Vector3 (90, 0, 0));
			coin.transform.localScale = new Vector3 (2, 2, 2);
			coins.Add (coin);
		}

	}

	private void removeUncollectedCoins(float ballPosition) {
		for(int i = 0; i < coins.Count ; i++) {
			GameObject c = coins [i];
			if (c == null) {
				Destroy (c);
				coins.RemoveAt (i);
			} else if (c.transform.position.z < ballPosition) {
				coins.RemoveAt (i);
			} 
		}
	}

	private Vector3 getSize(string name) {
		switch (name) 
		{
		case "bridge":
			return bridgeSize;
		case "ground":
			return groundSize;
		case "start_edge":
			return startEdgeSize;
		case "end_edge":
			return endEdgeSize;
		case "small_bridge":
			return smallBridgeSize;
		default:
			return new Vector3 ();
		}
	}

	private void putNextTrack() {
		int trackProb = Random.Range (1, 101);
		int gIslandProb = Random.Range (1, 101);
		if (trackProb < 50) {
			putGround ();
		} else if (trackProb < 70) {
			putGap ();
		} else if (trackProb < 90) {
			putBridge ();
		} else {
			putSmallBridge ();
		}

		if (gIslandProb < 70) {
			putGroundIsland ();
		}
	}

	private void putGap() {
		GameObject lastTrack = track [track.Count - 1];

		GameObject startEdge = getTrackFromPool (startEdgePool);
		float endPosition = lastTrack.transform.position.z + startEdgeSize.z;

		startEdge.transform.position = new Vector3 (0, 0, endPosition);

		GameObject endEdge = getTrackFromPool (endEdgePool);
		float endEdgePosition = endPosition + startEdgeSize.z + groundSize.z;
		endEdge.transform.position = new Vector3 (0, 0, endEdgePosition);

		track.Add (startEdge);
		track.Add (endEdge);
	}

	private void putBridge() {
		GameObject lastTrack = track [track.Count - 1];
		float endPosition = lastTrack.transform.position.z + bridgeSize.z;

		GameObject bridge = getTrackFromPool (bridgePool);
		bridge.transform.position = new Vector3 (0, 0, endPosition);

		track.Add (bridge);
	}

	private void putGround() {
		GameObject lastTrack = track [track.Count - 1];
		float endPosition = lastTrack.transform.position.z + groundSize.z;

		GameObject ground = getTrackFromPool (groundPool);
		ground.transform.position = new Vector3 (0, 0, endPosition);

		track.Add (ground);
	}

	private void putSmallBridge() {
		GameObject lastTrack = track [track.Count - 1];
		float endPosition = lastTrack.transform.position.z + startEdgeSize.z;

		GameObject startEdge = getTrackFromPool (startEdgePool);
		startEdge.transform.position = new Vector3 (0, 0, endPosition);

		GameObject smallBridge = getTrackFromPool (smallBridgePool);
		int xPos = Random.Range (-1, 2);
		smallBridge.transform.position = new Vector3 (xPos, 0, endPosition + 2*startEdgeSize.z);

		GameObject endEdge = getTrackFromPool (endEdgePool);
		endEdge.transform.position = new Vector3 (0, 0, endPosition + startEdgeSize.z + smallBridgeSize.z);

		track.Add (startEdge);
		track.Add (smallBridge);
		track.Add (endEdge);
	}

	private void putGroundIsland() {
		GameObject lastTrack = track [track.Count - 1];
		float endPosition = lastTrack.transform.position.z;

		GameObject groundIsland = getTrackFromPool (groundIslandPool);
		int xPos = Random.value < 0.5f ? -3 : 3;
		groundIsland.transform.position = new Vector3 (xPos, -1, endPosition);

		track.Add (groundIsland);
	}

	private void returnTrackToPool(float ballPosition) {
		for (int i = 0; i < track.Count; i++) {
			GameObject firstTrack = track [0];
			if (firstTrack.transform.position.z > ballPosition - 10f) {
				return;
			}
			track.RemoveAt (0);
			firstTrack.SetActive (false);
			switch (firstTrack.name) {
			case "bridge":
				bridgePool.Add (firstTrack);
				break;
			case "ground":
				groundPool.Add (firstTrack);
				break;
			case "start_edge":
				startEdgePool.Add (firstTrack);
				break;
			case "end_edge":
				endEdgePool.Add (firstTrack);
				break;
			case "small_bridge":
				smallBridgePool.Add (firstTrack);
				break;
			case "ground_island":
				groundIslandPool.Add (firstTrack);
				break;
			default:
				break;
			}
		}
	}

	private GameObject getTrackFromPool(List<GameObject> pool) {
		GameObject t = pool [0];
		pool.RemoveAt (0);
		t.SetActive (true);
		return t;
	}

	public void returnTrack(GameObject t) {
		switch (t.name) {
		case "bridge":
			bridgePool.Add (t);
			break;
		case "ground":
			groundPool.Add (t);
			break;
		case "start_edge":
			startEdgePool.Add (t);
			break;
		case "end_edge":
			endEdgePool.Add (t);
			break;
		case "small_bridge":
			smallBridgePool.Add (t);
			break;
		case "ground_island":
			groundIslandPool.Add (t);
			break;
		default:
			break;
		}
	}

	public void restartGame() {
		for (int i = 0; i < track.Count; i++) {
			track [i].SetActive (false);
			returnTrack (track [i]);
		}
		track = new List<GameObject>();
		trackPassed = false;
		generateStartTrack ();
	}
}
