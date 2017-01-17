using UnityEngine;
using System.Collections;

public class EnemyLooper : Enemy {

	protected float shootRate = 3f;
	GameObject chaserPrefab;
	override protected void Start () {
		base.Start();
		health = 50f;
		turnSpeed = 20f;
		speed = 0.5f;
		bodyDamage = 20f;
		explosionSize = 2f;
		chaserPrefab = Resources.Load<GameObject>("Prefabs/Enemies/EnemyChaser");
		StartCoroutine("StartChaserProduction");
	}
	
	override protected void Update () {
		base.Update();
		speed = 4f - 3f*Mathf.Clamp01(Vector2.Distance(transform.position, Vector2.zero)/(0.5f*screenHorizontal));
		turnSpeed = 10f*Vector2.Distance(transform.position, Vector2.zero) + 20f;
		TurnTowardsPosition(Vector2.zero);
		transform.Translate(speed*Vector3.right*Time.deltaTime);
	}

	virtual protected IEnumerator StartChaserProduction()
	{
		for(;;){
			Shoot();
			yield return new WaitForSeconds(shootRate);
		}
	}

	virtual protected void Shoot(){
		Vector3 rot = transform.rotation.eulerAngles;
		GameObject chaser = GameObject.Instantiate(chaserPrefab);
		chaser.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z+45f);
		chaser.transform.position = transform.position + Vector3.back;

		GameObject chaser2 = GameObject.Instantiate(chaserPrefab);
		chaser2.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z-45f);
		chaser2.transform.position = transform.position + Vector3.back;
	}

	override protected void Die(){
		GameObject pickup = Instantiate(Resources.Load<GameObject>("Prefabs/Pickups/HealthPickup_Small"));
		pickup.transform.position = transform.position + Vector3.forward;
		base.Die();
	}
}
