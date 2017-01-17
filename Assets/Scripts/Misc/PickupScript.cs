using UnityEngine;
using System.Collections;

public class PickupScript : MonoBehaviour {

	public float healAmount;
	public GameObject player;
	void Start () {
		if(player == null){
			player = GameObject.FindWithTag("Player");
		}
	}
	
	void Update () {
		if(player == null){
			player = GameObject.FindWithTag("Player");
		}
		transform.Rotate(0,0,50f * Time.deltaTime);
		if(player != null) {
			float speed = Mathf.Clamp((1f/(0.01f+Vector2.Distance(transform.position, player.transform.position))),0f,5f);
			transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed*speed*Time.deltaTime);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		Ship s = other.gameObject.GetComponent<Ship>();
		if(s != null){
			Die(s);
		}
	}

	protected void Die(Ship s){
		Destroy(gameObject);
		s.Heal(healAmount);
	}
}
