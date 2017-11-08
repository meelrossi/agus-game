using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgusCamera : MonoBehaviour {

	public GameObject ball;
	Vector3 offset;

	void Start () {
		offset = transform.position - ball.transform.position;
	}
	
	void LateUpdate () {
		Vector3 newpos = new Vector3 (0, ball.transform.position.y + offset.y, ball.transform.position.z + offset.z);
		transform.position = newpos;
	}

}
