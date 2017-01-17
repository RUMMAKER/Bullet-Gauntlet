using UnityEngine;
using System.Collections;

public class EnemySniper : Enemy {

	public GameObject player;

	private Vector2 target;
	private int targetCorner;//0=topleft,1=bottomleft,2=bottomright,3=topright
	private float falseRotation;

	protected float timeSinceFired;
	protected float fireRate;
	protected GameObject rocketPrefab;

	
	override protected void Start () {
		base.Start();
		if(player == null){
			player = GameObject.FindWithTag("Player");
		}
		explosionSize = 1.7f;
		health = 6f;
		rocketPrefab = Resources.Load<GameObject>("Prefabs/EnemyProjectiles/EnemyRocket");
		timeSinceFired = 0f;
		fireRate = 1.5f;

		
		speed = 1.1f;
		turnSpeed = 100f;
		falseRotation = 0f;
		targetCorner = Random.Range(0,4);
		PickCorner();
	}
	
	override protected void Update () {
		base.Update();
		if(player == null) {
			player = GameObject.FindWithTag("Player");
		}

		if(DistanceFromPlayer() < 2f){
			PickCorner();
		}
		Hover();
		TurnTowardsPlayer();
		
		if(timeSinceFired > fireRate){
			Fire();
		}
		timeSinceFired += Time.deltaTime;
	}

	virtual protected void MakeBullet(){
		GameObject rocket = GameObject.Instantiate(rocketPrefab);
		rocket.transform.rotation = Quaternion.Euler(0,0,90f + transform.rotation.eulerAngles.z);
		rocket.transform.position = transform.position + Vector3.back;

		GameObject rocket2 = GameObject.Instantiate(rocketPrefab);
		rocket2.transform.rotation = Quaternion.Euler(0,0,-90f + transform.rotation.eulerAngles.z);
		rocket2.transform.position = transform.position + Vector3.back;
	}

	protected void Fire(){
		MakeBullet();
		timeSinceFired = 0f;
	}


	protected void TurnTowardsPlayer () {
		if(player == null) return;
		TurnTowardsPosition(player.transform.position);
	}

	protected void TurnFalseTo(Vector2 target) {

		float newAngle = Mathf.Atan2((target.y - transform.position.y),(target.x - transform.position.x))*Mathf.Rad2Deg;
		if (newAngle < 0) {
			newAngle += 360;
		}
		falseRotation = newAngle;
	}

	private void PickCorner(){
		switch(targetCorner){
			case 0: 
			case 2:
				if(Meh()){	
					target = BottomLeftCorner();
					targetCorner = 1;
				} else {
					target = TopRightCorner();
					targetCorner = 3;
				}
				break;
			case 1:
			case 3:
			if(Meh()){	
				target = TopLeftCorner();
				targetCorner = 0;
			} else {
				target = BottomRightCorner();
				targetCorner = 2;
			}
			break;
		}
	}

	private bool Meh(){
		return Random.value > 0.5f;
	}

	private Vector2 BottomLeftCorner(){
		return new Vector2(Random.Range(-screenHorizontal/2f + 1f, -screenHorizontal/2f + 2f), Random.Range(-screenVertical/2f + 1f, -screenVertical/2f + 2f));
	}
	private Vector2 TopLeftCorner(){
		return new Vector2(Random.Range(-screenHorizontal/2f + 1f, -screenHorizontal/2f + 2f), Random.Range(screenVertical/2f - 1f, screenVertical/2f - 2f));
	}
	private Vector2 BottomRightCorner(){
		return new Vector2(Random.Range(screenHorizontal/2f - 1f, screenHorizontal/2f - 2f), Random.Range(-screenVertical/2f + 1f, -screenVertical/2f + 2f));
	}
	private Vector2 TopRightCorner(){
		return new Vector2(Random.Range(screenHorizontal/2f - 1f, screenHorizontal/2f - 2f), Random.Range(screenVertical/2f - 1f, screenVertical/2f - 2f));
	}

	private void Hover(){
		Vector2 position = transform.position;
		if((position - target).magnitude > 0.3f){
			TurnFalseTo(target);
		}

		Move(speed*Mathf.Clamp((position - target).magnitude, 0.2f, 1f));
	}

	private void Move(float speed){
		Vector3 direction = new Vector3(Mathf.Cos(falseRotation*Mathf.Deg2Rad), Mathf.Sin(falseRotation*Mathf.Deg2Rad), 0);
		direction = direction.normalized*speed*Time.deltaTime;
		//Debug.Log(direction.x + "," + direction.y + "," + direction.z);
		transform.position = new Vector3(transform.position.x + direction.x, 
										 transform.position.y + direction.y, 
										 transform.position.z);
	}

	//distance between target and player
	private float DistanceFromPlayer(){
		if(player == null) return 10f;
		return Vector2.Distance(player.transform.position, target);
	}

}
