using UnityEngine;
using System.Collections;

public class BossBase : Enemy {

	public GameObject player;

	private Vector2 target;
	private int targetCorner;//0=topleft,1=bottomleft,2=bottomright,3=topright
	private float falseRotation;

	override protected void Start(){
		base.Start();
		if(player == null){
			player = GameObject.FindWithTag("Player");
		}
		explosionSize = 2.7f;
		falseRotation = 0f;
		targetCorner = Random.Range(0,1);
		if(targetCorner == 1){
			targetCorner = 3;
			target = TopRightCorner();
		} else {
			target = TopLeftCorner();
		}
	}
	
	override protected void Update(){
		base.Update();
		if(player == null) {
			player = GameObject.FindWithTag("Player");
		}
		if(DistanceFromTarget() < 0.5f){
			PickCorner();
		}
		Hover();
		TurnTowardsPlayer();
	}

	override protected void Die(){
		Destroy(gameObject);
		GameObject explosion = ParticleFactory.MakeExplosion(explosionSize);
		explosion.transform.position = transform.position;
		GameObject shockWave = Instantiate(Resources.Load<GameObject>("Prefabs/PlayerProjectiles/ShockWave"));
		shockWave.transform.position = transform.position;
		GameObject pickup = Instantiate(Resources.Load<GameObject>("Prefabs/Pickups/HealthPickup"));
		pickup.transform.position = transform.position + Vector3.forward;
	}

	protected void TurnTowardsPlayer () {
		if(player == null) return;
		TurnTowardsPosition(player.transform.position);
	}

	protected float PlayerRot(Vector2 pos){
		return Mathf.Atan2((player.transform.position.y - pos.y),(player.transform.position.x - pos.x))*Mathf.Rad2Deg;
	}

	protected void TurnFalseTo(Vector2 target) {

		float newAngle = Mathf.Atan2((target.y - transform.position.y),(target.x - transform.position.x))*Mathf.Rad2Deg;
		if (newAngle < 0) {
			newAngle += 360;
		}
		falseRotation = newAngle;
	}

	protected void PickCorner(){
		switch(targetCorner){
			case 0: //top left
				if(Random.value > 0.7f){	
					target = BottomLeftCorner();
					targetCorner = 1;
				} else {
					target = TopRightCorner();
					targetCorner = 3;
				}
				break;
			case 2: //bottom right
				target = TopRightCorner();
				targetCorner = 3;
				break;
			case 3: //top right
			if(Random.value > 0.7f){	
				target = BottomRightCorner();
				targetCorner = 2;
			} else {
				target = TopLeftCorner();
				targetCorner = 0;
			}
			break;
			case 1: //bottom left
				target = TopLeftCorner();
				targetCorner = 0;
				break;
		}
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
		TurnFalseTo(target);

		Move(speed*Mathf.Clamp((position - target).magnitude, 0.7f, 1f));
	}

	private void Move(float speed){
		Vector3 direction = new Vector3(Mathf.Cos(falseRotation*Mathf.Deg2Rad), Mathf.Sin(falseRotation*Mathf.Deg2Rad), 0);
		direction = direction.normalized*speed*Time.deltaTime;
		//Debug.Log(direction.x + "," + direction.y + "," + direction.z);
		transform.position = new Vector3(transform.position.x + direction.x, 
										 transform.position.y + direction.y, 
										 transform.position.z);
	}

	private float DistanceFromTarget(){
		return Vector2.Distance(transform.position, target);
	}
}
