using UnityEngine;
using System.Collections;

public class EnemyTele : Enemy {

	public GameObject player;
	private GameObject bulletPrefab;
	private GameObject largeBulletPrefab;

	private Vector2 targetPosition;
	private float counter;
	private float fireRate;

	private float teleportCounter;
	private float teleportRate;
	private bool teleporting;

	override protected void Start () {
		base.Start();
		if(player == null){
			player = GameObject.FindWithTag("Player");
		}

		explosionSize = 1.5f;
		health = 50f;
		bulletPrefab = Resources.Load<GameObject>("Prefabs/EnemyProjectiles/EnemyBullet");
		largeBulletPrefab = Resources.Load<GameObject>("Prefabs/EnemyProjectiles/EnemyLargeBullet");

		speed = 0.2f;
		turnSpeed = 200f;

		targetPosition = Vector2.zero;
		counter = 0f;
		fireRate = 0.3f;
		teleportCounter = 4f;
		teleportRate = 3f;
	}
	
	override protected void Update () {
		base.Update();
		if(player == null) {
			player = GameObject.FindWithTag("Player");
		}
		if(teleporting){
			Teleport();
			return;
		}
		TurnTowardsPlayer();
		transform.Translate(-speed*Vector3.right*Time.deltaTime);
		if(teleportCounter > teleportRate){
			teleporting = true;
			PickNewPosition();
		}
		if(counter > fireRate){
			Fire();
		}
		counter += Time.deltaTime;
		teleportCounter += Time.deltaTime;
	}

	private void MakeBullet(){
		Vector3 rot = transform.rotation.eulerAngles;
		
		GameObject bullet = GameObject.Instantiate(bulletPrefab);
		bullet.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z + Random.Range(-5f, 5f));
		bullet.transform.position = transform.position + Vector3.back;
	}

	private void Make360Bullet(){
		Vector3 rot = transform.rotation.eulerAngles;
		for(int n = 0; n < 12; n ++){
			GameObject bullet = GameObject.Instantiate(largeBulletPrefab);
			bullet.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z -5*30f + 30f * n);
			bullet.transform.position = transform.position + Vector3.back;
		}
	}

	protected void Fire(){
		MakeBullet();
		counter = 0f;
	}

	private void Teleport(){
		turnSpeed = 300f;
		TurnTowardsPosition(targetPosition);
		transform.Translate(speed*3.2f*Vector3.right*Time.deltaTime);
		if(CloseEnaugh()){
			//create effect here
			transform.position = targetPosition;
			//create effect
			teleportCounter = 0f;
			teleporting = false;
			turnSpeed = 200f;
			counter = -0.5f;
			Make360Bullet();
		}
	}

	private bool CloseEnaugh(){
		float angle = transform.rotation.eulerAngles.z;

		float newAngle = Mathf.Atan2((targetPosition.y - transform.position.y),(targetPosition.x - transform.position.x))*Mathf.Rad2Deg;
		if (newAngle < 0) {
			newAngle += 360;
		}

		float turnDegree = newAngle - angle;//how far to turn to match newAngle
		float difference = Mathf.Abs(turnDegree);
		if(difference > 180f) difference = 360f - difference;
		return difference < 1.5f;
	}

	private void PickNewPosition(){
		targetPosition = new Vector2(Random.Range(-screenHorizontal/2f + 1.5f,screenHorizontal/2f - 1.5f),
									 Random.Range(-screenVertical/2f + 1.5f,screenVertical/2f - 1.5f));
		if(player == null) return;
		while(Vector2.Distance(targetPosition,player.transform.position)<3.5f || Vector2.Distance(targetPosition,transform.position)<3.5f){
			targetPosition = new Vector2(Random.Range(-screenHorizontal/2f + 1.5f,screenHorizontal/2f - 1.5f),
									 Random.Range(-screenVertical/2f + 1.5f,screenVertical/2f - 1.5f));
		}
	}

	protected void TurnTowardsPlayer () {
		if(player == null) return;
		TurnTowardsPosition(player.transform.position);
	}
}
