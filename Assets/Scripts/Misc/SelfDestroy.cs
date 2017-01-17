using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour {

	public float time;
	private float timer = 0f;
	void Update(){
		if (timer > time) {
			Destroy(gameObject);
		}
		timer += Time.deltaTime;
	}
}
