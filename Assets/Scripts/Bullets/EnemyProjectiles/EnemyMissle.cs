using UnityEngine;
using System.Collections;

public class EnemyMissle : EnemyBullet {

	private float maxSpeed;
	GameObject emission;
	private float particleRate = 0.001f;
	override protected void Start () {
		base.Start();
		maxSpeed = 6f;
		emission = Resources.Load<GameObject>("Prefabs/Effects/Emissions/RocketEmissions/EnemyRocketTrail");
		StartCoroutine("StartTrail");
	}
	
	override protected void Update () {
		base.Update();
		speed += 5f*Time.deltaTime;
		if(speed > maxSpeed)speed = maxSpeed;
	}

	void OnTriggerEnter2D(Collider2D other){
		Ship s = other.gameObject.GetComponent<Ship>();
		if(s != null){
			s.TakeDamage(damage);
			GameObject explosion = ParticleFactory.MakeExplosion(1.3f);
			explosion.transform.position = transform.position;
			Destroy(gameObject);
		}
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
