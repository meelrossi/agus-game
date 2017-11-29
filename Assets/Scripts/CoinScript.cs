using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour {

	public AudioSource coinSound;

	void Start () {
		coinSound = GetComponent<AudioSource> ();
	}

	public void OnTriggerEnter(Collider other) {
		StartCoroutine (playSound());
	}

	IEnumerator playSound(){
		coinSound.Play ();
		AgusManager.instance.addScore (1);
		yield return new WaitForSeconds (0.05f);
		Destroy(this.gameObject);
	}
}
