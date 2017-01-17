using UnityEngine;
using System.Collections;

public class EnemyLargeBullet : EnemyBullet {
	public float maxSpeed;
	public float accel;
	override protected void Start () {
		base.Start();
	}
	
	override protected void Update () {
		speed += accel*Time.deltaTime;
		if(speed > maxSpeed){
			speed = maxSpeed;
		}
		base.Update();
	}

	void OnTriggerEnter2D(Collider2D other){
		Ship s = other.gameObject.GetComponent<Ship>();
		if(s != null){
			GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Effects/EnemyHitEffect"));
			obj.transform.position = transform.position;
			s.TakeDamage(damage);
			Destroy(gameObject);
		}
	}
}
