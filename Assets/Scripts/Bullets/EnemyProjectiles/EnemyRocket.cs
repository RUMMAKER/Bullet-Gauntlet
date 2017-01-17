using UnityEngine;
using System.Collections;

public class EnemyRocket : EnemyBullet {
	public GameObject player;
	private float turnSpeed;
	private float chaseCounter = 0;
	private float accel = 5.5f;

	private float particleRate = 0.001f;
	GameObject emission;
	override protected void Start () {
		base.Start();
		turnSpeed = 240f;
		if(player == null){
			player = GameObject.FindWithTag("Player");
		}

		emission = Resources.Load<GameObject>("Prefabs/Effects/Emissions/RocketEmissions/EnemyRocketTrail");
		StartCoroutine("StartTrail");
	}
	
	override protected void Update () {
		if(player == null) {
			player = GameObject.FindWithTag("Player");
		}
		speed += Time.deltaTime*accel;
		if(speed > 5.5f) speed = 5.5f;
		chaseCounter += Time.deltaTime;
		base.Update();
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

	protected override void Move(){
		TurnTowardsPlayer();
		transform.Translate(new Vector3(speed*Time.deltaTime, 0f, 0f));
	}

	protected void TurnTowardsPlayer () {
		if(player == null) return;
		Vector2 playerPos = player.transform.position;
		float angle = transform.rotation.eulerAngles.z;

		float newAngle = Mathf.Atan2((playerPos.y - transform.position.y),(playerPos.x - transform.position.x))*Mathf.Rad2Deg;
		if (newAngle < 0) {
			newAngle += 360;
		}
		float turnDegree = newAngle - angle;//how far to turn to match newAngle
		float difference = Mathf.Abs(turnDegree);
		float turnRate = Mathf.Clamp((turnSpeed - chaseCounter*200f), 0f, 400f) * Time.deltaTime;
		if(difference > 180f) difference = 360f - difference;

		if(turnRate > difference)//will over turn
		{
			turnRate = difference;
		}

		if(turnDegree < 180 && turnDegree > 0)
		{
			transform.Rotate(0,0,turnRate);
		}
		else if(turnDegree >= 180 && turnDegree < 360)
		{
			transform.Rotate(0,0,turnRate*-1);
		}
		else if(turnDegree < 0 && turnDegree >= -180)
		{
			transform.Rotate(0,0,turnRate*-1);
		}
		else if(turnDegree < 0 && turnDegree < -180)
		{
			transform.Rotate(0,0,turnRate);
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
