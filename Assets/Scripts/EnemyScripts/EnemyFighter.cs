using UnityEngine;
using System.Collections;

public class EnemyFighter : Enemy {

	public GameObject player;

	private float initialTurnSpeed;
	private float turnAccel;
	private float maxTurnSpeed;

	private bool hardTurning;
	private float timeSinceFired;
	private GameObject bulletPrefab;
	private float rando;

	override protected void Start () {
		base.Start();
		if(player == null){
			player = GameObject.FindWithTag("Player");
		}

		explosionSize = 1.55f;
		rando = Random.value;
		health = 2f;
		bulletPrefab = Resources.Load<GameObject>("Prefabs/EnemyProjectiles/EnemyBullet");
		timeSinceFired = 0f;

		hardTurning = false;
		speed = 2.1f;
		turnSpeed = 35f + rando*15f;
		initialTurnSpeed = turnSpeed;
		maxTurnSpeed = 500f;
		turnAccel = 180f + rando*50f;	
	}
	
	override protected void Update () {
		base.Update();
		if(player == null) {
			player = GameObject.FindWithTag("Player");
		}

		if(DistanceFromPlayer() > (2f + rando) || DistanceToEdge() < 1.2f){
			hardTurning = true;
		}

		if(hardTurning){
			hardTurn();
		} else {
			TurnAwayFromPlayer();
		}
		if(timeSinceFired > 0.7f){
			Fire();
		}
		timeSinceFired += Time.deltaTime;
		transform.Translate(speed*Vector3.right*Time.deltaTime);

		//KeepInBounds();
	}

	private void MakeBullet(){
		GameObject bullet = GameObject.Instantiate(bulletPrefab);
		bullet.transform.rotation = transform.rotation;
		bullet.transform.position = transform.position + Vector3.back;
	}

	protected void Fire(){
		MakeBullet();
		timeSinceFired = 0f;
	}

	private void hardTurn(){

		turnSpeed += turnAccel*Time.deltaTime;
		if(turnSpeed > maxTurnSpeed) turnSpeed = maxTurnSpeed;
		TurnTowardsPlayer();
		if(closeEnaugh() || (DistanceFromPlayer() < (2f + rando) && DistanceToEdge() > 1.2f)){
			hardTurning = false;
			turnSpeed = initialTurnSpeed;
		}
	}

	private bool closeEnaugh(){
		if(player == null) return true;
		Vector2 playerPos = player.transform.position;
		float angle = transform.rotation.eulerAngles.z;

		float newAngle = Mathf.Atan2((playerPos.y - transform.position.y),(playerPos.x - transform.position.x))*Mathf.Rad2Deg;
		if (newAngle < 0) {
			newAngle += 360;
		}

		float turnDegree = newAngle - angle;//how far to turn to match newAngle
		float difference = Mathf.Abs(turnDegree);
		if(difference > 180f) difference = 360f - difference;
		return difference < (2.5f + rando);
	}

	protected void TurnTowardsPlayer () {
		if(player == null) return;
		TurnTowardsPosition(player.transform.position);
	}

	protected void TurnAwayFromPlayer () {
		if(player == null) return;
		Vector2 playerPos = player.transform.position;
		float angle = transform.rotation.eulerAngles.z;

		float newAngle = Mathf.Atan2((playerPos.y - transform.position.y),(playerPos.x - transform.position.x))*Mathf.Rad2Deg;
		if (newAngle < 0) {
			newAngle += 360;
		}

		float turnDegree = newAngle - angle;//how far to turn to match newAngle
		float turnRate = turnSpeed * Time.deltaTime;

		if(turnDegree < 180 && turnDegree > 0)
		{
			transform.Rotate(0,0,-turnRate);
		}
		else if(turnDegree >= 180 && turnDegree < 360)
		{
			transform.Rotate(0,0,turnRate);
		}
		else if(turnDegree < 0 && turnDegree >= -180)
		{
			transform.Rotate(0,0,turnRate);
		}
		else if(turnDegree < 0 && turnDegree < -180)
		{
			transform.Rotate(0,0,-turnRate);
		}
		else{
			transform.Rotate(0,0,turnRate);
		}
	}

	private float DistanceToEdge(){
		return Mathf.Min(screenHorizontal/2f - Mathf.Abs(transform.position.x),
						 screenVertical/2f - Mathf.Abs(transform.position.y));
	}

	private float DistanceFromPlayer(){
		if(player == null) return 10f;
		Vector2 playerPos = player.transform.position;
		return Mathf.Sqrt(Mathf.Abs(playerPos.x - transform.position.x)*Mathf.Abs(playerPos.x - transform.position.x) + 
				Mathf.Abs(playerPos.y - transform.position.y)*Mathf.Abs(playerPos.y - transform.position.y));
	}
}
