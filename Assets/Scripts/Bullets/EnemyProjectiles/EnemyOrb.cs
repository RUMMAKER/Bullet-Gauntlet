using UnityEngine;
using System.Collections;

public class EnemyOrb : EnemyBullet {

	GameObject shrapnelPrefab;
	private float decel;
	override protected void Start () {
		base.Start();
		shrapnelPrefab = Resources.Load<GameObject>("Prefabs/EnemyProjectiles/EnemyLargeBullet");
		decel = 1f;
	}
	
	override protected void Update () {
		KeepInBounds();
		if(speed <= 0f) Die();
		decel += 4f*Time.deltaTime;
		if(decel > 7f) decel = 7f;
		speed -= decel*Time.deltaTime;
		base.Update();
	}

	void OnTriggerEnter2D(Collider2D other){
		Ship s = other.gameObject.GetComponent<Ship>();
		if(s != null){
			s.TakeDamage(damage);
			Die();
		}
	}

	override protected void Die(){
		for(int n = 0; n < 16; n ++){
			GameObject obj = Instantiate(shrapnelPrefab);
			obj.transform.parent = transform;
			obj.transform.localPosition = Vector3.left*speed*Time.deltaTime*2f;
			obj.transform.localRotation = Quaternion.Euler(0,0,-7*22.5f + 22.5f*n);
			obj.transform.parent = null;
		}
		Destroy(gameObject);
	}
}
