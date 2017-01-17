using UnityEngine;
using System.Collections;

//require spriterenderer
public class EnemyRoamer : Enemy {

	public GameObject player;
	private GameObject bulletPrefab;

	private Vector2 targetPosition;
	private float counter;
	private float fireRate;
	private SpriteRenderer spriteRenderer;

	override protected void Start () {
		base.Start();
		if(player == null){
			player = GameObject.FindWithTag("Player");
		}

		explosionSize = 1.5f;
		health = 8f;
		bulletPrefab = Resources.Load<GameObject>("Prefabs/EnemyProjectiles/EnemyBullet");

		speed = 2.1f;
		turnSpeed = 90f;

		targetPosition = Vector2.zero;
		counter = 0f;
		fireRate = 4f;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	override protected void Update () {
		base.Update();
		if(player == null) {
			player = GameObject.FindWithTag("Player");
		}

		if(counter < fireRate*0.9f){
			turnSpeed = 90f;
			spriteRenderer.color = new Color(1f,1f,1f,Mathf.Lerp(1f,0.1f,3f*counter));
			TurnTowardsPosition(targetPosition);
			transform.Translate(Mathf.Lerp(speed*0.6f,speed,3f*counter)*Vector3.right*Time.deltaTime);
		} else {
			turnSpeed = 270f;
			spriteRenderer.color = new Color(1f,1f,1f,Mathf.Lerp(0.1f,1f,3f*(counter-fireRate*0.9f)));
			TurnTowardsPlayer();
			transform.Translate(Mathf.Lerp(speed,speed*0.6f,3f*(counter-fireRate*0.9f))*Vector3.right*Time.deltaTime);
		}
		if(counter > fireRate){
			Fire();
			PickNewPosition();
		}
		counter += Time.deltaTime;
		//KeepInBounds();
	}

	private void MakeBullet(){
		Vector3 rot = transform.rotation.eulerAngles;
		for(int n = 0; n < 3; n ++){
			GameObject bullet = GameObject.Instantiate(bulletPrefab);
			bullet.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z -7f + 7f * n);
			bullet.transform.parent = transform;
			bullet.transform.localPosition = Vector3.back + (n-1)*Vector3.up*0.2f;
			bullet.transform.parent = null;
		}
	}

	protected void Fire(){
		MakeBullet();
		counter = 0f;
	}

	private void PickNewPosition(){
		targetPosition = new Vector2(Random.Range(-screenHorizontal/2f + 1.5f,screenHorizontal/2f - 1.5f),
									 Random.Range(-screenVertical/2f + 1.5f,screenVertical/2f - 1.5f));
	}

	protected void TurnTowardsPlayer () {
		if(player == null) return;
		TurnTowardsPosition(player.transform.position);
	}
}
