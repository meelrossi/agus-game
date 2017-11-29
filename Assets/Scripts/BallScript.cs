using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

	private bool isMoving = false;
	private int dir = 0;
	private int destination = 0;
	private int count = 0;
	Vector3 savedVelocity;
	Vector3 savedAngularVelocity;
	public AudioSource moveSound;

	void Start () {
		moveSound = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (isMoving) {
			movingAnimation ();
		}

		if (Input.GetKey ("up")) {
			Vector3 vel = GetComponent<Rigidbody> ().velocity;
			if (vel.z < 10f) {
				GetComponent<Rigidbody> ().AddForce (Vector3.forward * 15f);
			}
		}
		if (Input.GetKeyUp ("left")) {
			Vector3 curr_pos = transform.position;
			if (curr_pos.x > -1) {
				this.dir = -1;
				this.destination = (int) curr_pos.x - 1;
				this.isMoving = true;
				moveSound.Play ();
			}
		}
		if (Input.GetKeyUp ("right")) {
			Vector3 curr_pos = transform.position;
			if (curr_pos.x < 1) {
				this.dir = 1;
				this.destination = (int) curr_pos.x + 1;
				this.isMoving = true;
				moveSound.Play ();
			}
		}

		if (Input.GetKeyUp ("space")) {
			if (transform.position.y > 0.5f)
				return;
			GetComponent<Rigidbody> ().AddForce (Vector3.up * 1200f);
		} 
	}

	private void movingAnimation() {
		Vector3 curr_pos = transform.position;
		float delta = (float) (curr_pos.x + (dir * 0.2f));
		this.transform.position = new Vector3 (delta, curr_pos.y, curr_pos.z);
		if (count == 5) {
			this.transform.position = new Vector3 (destination, curr_pos.y, curr_pos.z);
			isMoving = false;
			count = 0;
		}
		count++;
	}
		
	public void PauseBall(bool paused) {
		Rigidbody ball = GetComponent<Rigidbody> ();
		if (paused) {
			savedVelocity = ball.velocity;
			savedAngularVelocity = ball.angularVelocity;
			ball.isKinematic = true;
		} else {
			// un-paused
			ball.isKinematic = false;
			ball.velocity = savedVelocity;
			ball.angularVelocity = savedAngularVelocity;
			ball.WakeUp();
		}
	}

	public void restartGame() {
		transform.position = new Vector3 (0, 0.4f, 0);
		savedVelocity = new Vector3 (0, 0, 0);
		savedAngularVelocity = new Vector3 (0, 0, 0);
		PauseBall (false);
	}
}
