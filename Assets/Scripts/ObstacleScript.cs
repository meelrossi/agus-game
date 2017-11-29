using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour {

	public AudioSource sound;

	void Start(){
		sound = GetComponent<AudioSource> ();
	}
	public void OnTriggerEnter(Collider col) {
		if (col.CompareTag ("Ball")) {
			sound.Play();
			AgusManager.instance.GameOver ();
		}
	}
}
