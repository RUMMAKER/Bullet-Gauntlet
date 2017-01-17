using UnityEngine;
using System.Collections;

public class ExplosionSound : MonoBehaviour {

	public AudioClip[] sounds;
	void Awake () {

		int rando = Random.Range(0,sounds.Length);
		GetComponent<AudioSource>().pitch = Random.Range(0.8f,1f);
		GetComponent<AudioSource>().PlayOneShot(sounds[rando], Random.Range(0.3f,0.4f));
	}

}
