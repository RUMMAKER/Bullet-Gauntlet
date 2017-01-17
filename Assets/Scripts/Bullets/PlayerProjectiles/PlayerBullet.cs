using UnityEngine;
using System.Collections;

public class PlayerBullet : Bullet {

	override protected void Start(){
		base.Start();
	}

	override protected void Update () {
		base.Update();
	}

	void OnTriggerEnter2D(Collider2D other){
		Enemy e = other.gameObject.GetComponent<Enemy>();
		if(e != null){
			e.TakeDamage(damage);
			Destroy(gameObject);
		}
	}
}
