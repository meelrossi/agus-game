using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("up")) {
			Vector3 vel = GetComponent<Rigidbody> ().velocity;
			if (vel.z < 10f) {
				GetComponent<Rigidbody> ().AddForce (Vector3.forward * 10f);
			}
		}
		if (Input.GetKeyUp ("left")) {
			Vector3 curr_pos = transform.position;
			if (curr_pos.x > -1) {
				this.transform.position = new Vector3 (curr_pos.x - 1, curr_pos.y, curr_pos.z);
			}
		}
		if (Input.GetKeyUp ("right")) {
			Vector3 curr_pos = transform.position;
			if (curr_pos.x < 1) {
				this.transform.position = new Vector3 (curr_pos.x + 1, curr_pos.y, curr_pos.z);
			}
		}

		if (Input.GetKeyUp ("space")) {
			GetComponent<Rigidbody> ().AddForce (Vector3.up * 500f);
		} 
	}
}
