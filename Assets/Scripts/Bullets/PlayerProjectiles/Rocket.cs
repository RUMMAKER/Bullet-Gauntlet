using UnityEngine;
using System.Collections;

public class Rocket : PlayerBullet {
	float actualSpeed;
	float accel;
	GameObject emission;
	private float particleRate = 0.001f;

	override protected void Start () {
		base.Start();
		actualSpeed = -1f;
		accel = 35f;

		emission = Resources.Load<GameObject>("Prefabs/Effects/Emissions/RocketEmissions/PlayerRocketTrail");
		StartCoroutine("StartTrail");
	}
	
	override protected void Update () {
		base.Update();
	}

	void OnTriggerEnter2D(Collider2D other){
		Enemy e = other.gameObject.GetComponent<Enemy>();
		if(e != null){
			e.TakeDamage(damage);
			GameObject explosion = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PlayerProjectiles/RocketExplosion"));
			explosion.transform.position = transform.position + Vector3.forward;
			Destroy(gameObject);
		}
	}

	protected override void Move(){
		if(actualSpeed < speed){
			actualSpeed += accel*Time.deltaTime;
			if(actualSpeed > speed) actualSpeed = speed;
		}
		transform.Translate(new Vector3(actualSpeed*Time.deltaTime, 0f, 0f));
	}

	private IEnumerator StartTrail()
	{
		for(;;){
			Particle();
			yield return new WaitForSeconds(particleRate);
		}
	}

	private void Particle(){
		Vector3 rot = transform.rotation.eulerAngles+new Vector3(0,0,180);
		GameObject particle = Instantiate(emission);
		particle.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z + Random.Range(-5f, 5f));
		particle.transform.parent = transform;
		particle.transform.localPosition = Vector3.zero;
		particle.transform.localPosition += Vector3.forward
										 + Vector3.up*Random.Range(-0.02f, 0.02f)
										 + Vector3.left*0.3f;
		particle.transform.parent = null;
	}
}
