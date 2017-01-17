using UnityEngine;
using System.Collections;

public class EnemyDeployer : Enemy {

	private float radius;
	private float direction;
	private GameObject fighterPrefab;
	private float fireCounter;

	private float particleRate = 0.01f;
	GameObject emission;
	override protected void Start () {
		base.Start();
		radius = Random.Range(3.5f, 3.7f);
		direction = Random.Range(0,2);
		if(direction == 0f){
			direction = -1f;
		}

		health = 70f;
		bodyDamage = 20f;
		turnSpeed = 25f;
		speed = 0.25f;
		explosionSize = 2f;
		fighterPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Enemy_0");
		emission = Resources.Load<GameObject>("Prefabs/Effects/Emissions/SmokeEmission_D");
		StartCoroutine("StartTrail");
	}
	
	override protected void Update () {
		base.Update();
		TurnTo(direction*90f + 30f*(direction*radius - direction*DistanceFromCenter()));
		transform.Translate(speed*Vector3.right*Time.deltaTime);
		if(fireCounter > 4f) {
			StartCoroutine("MakeFighters");
		}
		fireCounter += Time.deltaTime;
	}

	private void TurnTo(float angle){
		TurnTowardsAngle(Mathf.Atan2((-transform.position.y),(-transform.position.x))*Mathf.Rad2Deg + angle);
	}

	private float DistanceFromCenter(){
		return Vector2.Distance(transform.position, Vector2.zero);
	}

	private IEnumerator MakeFighters()
	{
		fireCounter = 0f;
		Vector3 rot = transform.rotation.eulerAngles;
		for(int n = 0; n < 4; n ++){
			GameObject fighter = GameObject.Instantiate(fighterPrefab);
			fighter.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
			fighter.transform.parent = transform;
			fighter.transform.localPosition = Vector3.back + Vector3.right*0.3f;
			fighter.transform.parent = null;
			yield return new WaitForSeconds(0.3f);
		}
		fireCounter = 0f;
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
										 + Vector3.left*0.1f;
		//particle.transform.parent = null;
	}

	override protected void Die(){
		GameObject pickup = Instantiate(Resources.Load<GameObject>("Prefabs/Pickups/HealthPickup_Small"));
		pickup.transform.position = transform.position + Vector3.forward;
		base.Die();
	}
}
