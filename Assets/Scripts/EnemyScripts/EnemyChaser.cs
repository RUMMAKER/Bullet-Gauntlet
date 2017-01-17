using UnityEngine;
using System.Collections;


//!!!!!!!!
//move speed = how close rotation is towards player, faster if facing player, slower if not, so can have slow turn speed
//but still be able to reach player
//IDEA!!
public class EnemyChaser : Enemy {

	public GameObject player;
	private float rando;
	private float maxTurnSpeed;

	private float particleRate = 0.003f;
	GameObject emission;
	override protected void Start () {
		base.Start();
		if(player == null){
			player = GameObject.FindWithTag("Player");
		}

		bodyDamage = 50f;
		explosionSize = 1.5f;
		rando = Random.value;
		health = 4f;

		speed = 3.4f;
		maxTurnSpeed = 650f + rando*100f;
		emission = Resources.Load<GameObject>("Prefabs/Effects/Emissions/SmokeEmission_Looper_Mid");
		StartCoroutine("StartTrail");
	}
	
	override protected void Update () {
		base.Update();
		if(player == null) {
			player = GameObject.FindWithTag("Player");
		}

		turnSpeed = maxTurnSpeed*Mathf.Clamp(angleFromPlayer(), 30f, 180f)/180f;
		TurnTowardsPlayer();
		transform.Translate(speed*Vector3.right*Time.deltaTime);

		KeepInBounds();
	}

	protected void TurnTowardsPlayer () {
		if(player == null) return;
		TurnTowardsPosition(player.transform.position);
	}

	private float angleFromPlayer(){
		if(player == null) return 0;
		Vector2 playerPos = player.transform.position;
		float angle = transform.rotation.eulerAngles.z;

		float newAngle = Mathf.Atan2((playerPos.y - transform.position.y),(playerPos.x - transform.position.x))*Mathf.Rad2Deg;
		if (newAngle < 0) {
			newAngle += 360;
		}

		float turnDegree = newAngle - angle;//how far to turn to match newAngle
		float difference = Mathf.Abs(turnDegree);
		if(difference > 180f) difference = 360f - difference;
		return difference;
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
		particle.transform.parent = null;
	}
}
