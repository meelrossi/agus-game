using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour {

	public void OnTriggerEnter(Collider col) {
		if (col.CompareTag ("Ball")) {
			AgusManager.instance.GameOver ();
		}
	}
}
