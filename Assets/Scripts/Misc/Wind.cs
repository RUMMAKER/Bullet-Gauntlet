using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour {

	private AudioSource source;
	private float pitch;
	private float pitchChangeSpeed;

	private float volume;
	private float volumeChangeSpeed;
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
		pitch = source.pitch;
		pitchChangeSpeed = 1f;

		volume = source.volume;
		volumeChangeSpeed = 0.3f;
	}
	
	// Update is called once per frame
	void Update () {
		if(source.pitch != pitch) {
			ChangePitch();
		} else {
			PickPitch();
		}

		if(source.volume != volume) {
			ChangeVolume();
		} else {
			PickVolume();
		}
	}

	void PickPitch(){
		pitchChangeSpeed = Random.Range(0.2f,1f);
		pitch = Random.Range(0.5f,1f);
	}

	void ChangePitch(){
		float change = Time.deltaTime*pitchChangeSpeed;
		if(source.pitch < pitch) {
			source.pitch += change;
			if(source.pitch > pitch) {
				source.pitch = pitch;
			}
		} else {
			source.pitch -= change;
			if(source.pitch < pitch) {
				source.pitch = pitch;
			}
		}
	}

	void PickVolume(){
		volumeChangeSpeed = Random.Range(0.1f,0.3f);
		volume = Random.Range(0.1f,0.2f);
	}

	void ChangeVolume(){
		float change = Time.deltaTime*volumeChangeSpeed;
		if(source.volume < volume) {
			source.volume += change;
			if(source.volume > volume) {
				source.volume = volume;
			}
		} else {
			source.volume -= change;
			if(source.volume < volume) {
				source.volume = volume;
			}
		}
	}
}
