using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {

	//public float speed;
	public float affect;//greater affect = greater movement
	public float maxAffect;
	private Vector3 startPos;
	//private Vector2 velocity;
	void Start () {
		startPos = transform.position;
	}
	
	void Update () {
		Vector2 change = affect*GetMousePos();
		if(change.magnitude > maxAffect){
			change = change.normalized*maxAffect;
		}
		transform.position = startPos - (Vector3)(change);
	}

	private Vector2 GetMousePos () {
		return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
