﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour {

	public static TrackManager instance = null;

	public GameObject smallBridgePrefab;
	public GameObject bridgePrefab;
	public GameObject groundPrefab;
	public GameObject starteEdgePrefab;
	public GameObject endEdgePrefab;

	private List<GameObject> groundPool = new List<GameObject>();
	private List<GameObject> bridgePool = new List<GameObject>();
	private List<GameObject> smallBridgePool = new List<GameObject>();
	private List<GameObject> startEdgePool = new List<GameObject>();
	private List<GameObject> endEdgePool = new List<GameObject>();

	private Vector3 groundSize;
	private Vector3 bridgeSize;
	private Vector3 smallBridgeSize;
	private Vector3 startEdgeSize;
	private Vector3 endEdgeSize;

	private List<GameObject> track = new List<GameObject>();
	private bool trackPassed = false;

	public class TrackTag	{
		public static string BRIDGE = "bridge";
		public static string SMALL_BRIDGE = "small_bridge";
		public static string GROUND = "ground";
		public static string START_EDGE = "start_edge";
		public static string END_EDGE = "end_edge";
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

	private void generatePools() {
		for (int i = 0; i < 4; i++) {
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
		}

		for (int i = 0; i < 20; i++) {
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
		GameObject ground = track [2];
		if (ballPosition > ground.transform.position.z) {
			returnTrackToPool ();
			putNextTrack ();
		}

	}

	private void putObstacles() {
		GameObject lastTrack = track [track.Count - 1];

			
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
		int prob = Random.Range (1, 100);
		if (prob < 50) {
			putGround ();
		} else if (prob < 70) {
			putGap ();
		} else if (prob < 90) {
			putBridge ();
		} else {
			putSmallBridge ();
		}
	}

	private void putGap() {
		GameObject lastTrack = track [track.Count - 1];
		float endPosition = lastTrack.transform.position.z + startEdgeSize.z;

		GameObject startEdge = getTrackFromPool (startEdgePool);
		startEdge.transform.position = new Vector3 (0, 0, endPosition);

		GameObject endEdge = getTrackFromPool (endEdgePool);
		float endEdgePosition = endPosition + startEdgeSize.z + groundSize.z;
		endEdge.transform.position = new Vector3 (0, 0, endEdgePosition);

		track.Add (startEdge);
		track.Add (endEdge);
	}

	private void putBridge() {
		GameObject lastTrack = track [track.Count - 1];
		float endPosition = lastTrack.transform.position.z + getSize(lastTrack.name).z;

		GameObject bridge = getTrackFromPool (bridgePool);
		bridge.transform.position = new Vector3 (0, 0, endPosition);

		track.Add (bridge);
	}

	private void putGround() {
		GameObject lastTrack = track [track.Count - 1];
		float endPosition = lastTrack.transform.position.z + getSize(lastTrack.name).z;

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
		int xPos = Random.Range (-1, 1);
		smallBridge.transform.position = new Vector3 (xPos, 0, endPosition + 2*startEdgeSize.z);

		GameObject endEdge = getTrackFromPool (endEdgePool);
		endEdge.transform.position = new Vector3 (0, 0, endPosition + startEdgeSize.z + smallBridgeSize.z);

		track.Add (startEdge);
		track.Add (smallBridge);
		track.Add (endEdge);
	}

	private void returnTrackToPool() {
		GameObject firstTrack = track [0];
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
		default:
			break;
		}
	}

	private GameObject getTrackFromPool(List<GameObject> pool) {
		GameObject track = pool [0];
		pool.RemoveAt (0);
		track.SetActive (true);
		return track;
	}
}