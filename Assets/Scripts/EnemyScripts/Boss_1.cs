using UnityEngine;
using System.Collections;

public class Boss_1 : BossBase {

	private float primaryFireTimer;
	private float primaryRate;
	private GameObject bulletPrefab;
	private GameObject rocketPrefab;
	private float count;
	override protected void Start () {
		base.Start();
		if(player == null){
			player = GameObject.FindWithTag("Player");
		}

		bodyDamage = 150f;
		health = 500f;
		speed = 1f;
		turnSpeed = 200f;

		primaryRate = 1f;
		rocketPrefab = Resources.Load<GameObject>("Prefabs/EnemyProjectiles/EnemyRocket");
		bulletPrefab = Resources.Load<GameObject>("Prefabs/EnemyProjectiles/EnemyPlasma");
	}
	
	override protected void Update () {
		count += Time.deltaTime;
		if(health > 350f && count < 30f) {
			StageOne();
		} else if(health > 150f && count < 70f) {
			StageTwo();
		} else {
			StageThree();
		}
	}

	private void StageOne(){
		base.Update();
		if(primaryFireTimer > primaryRate){
			//Fire2();
			Fire();
		}
		primaryFireTimer += Time.deltaTime;
	}

	private void StageTwo(){
		base.Update();
		if(primaryFireTimer > primaryRate){
			Fire2();
		}
		primaryFireTimer += Time.deltaTime;
	}

	private void StageThree(){
		base.Update();
		if(primaryFireTimer > primaryRate){
			Fire3();
		}
		primaryFireTimer += Time.deltaTime;
	}

	private void MakeBulletAt(Vector3 pos, float angle){
		GameObject bullet = GameObject.Instantiate(bulletPrefab);
		bullet.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + angle);
		bullet.transform.parent = transform;
		bullet.transform.localPosition = Vector3.right * 0.25f + pos + Vector3.back;
		bullet.transform.parent = null;
	}

	private void MakeRocket(){
		GameObject rocket = GameObject.Instantiate(rocketPrefab);
		rocket.transform.rotation = Quaternion.Euler(0,0,90f + transform.rotation.eulerAngles.z);
		rocket.transform.position = transform.position + Vector3.back;

		GameObject rocket2 = GameObject.Instantiate(rocketPrefab);
		rocket2.transform.rotation = Quaternion.Euler(0,0,-90f + transform.rotation.eulerAngles.z);
		rocket2.transform.position = transform.position + Vector3.back;
	}

	private IEnumerator BulletSpiral(Vector3 startPos, float startAngle, float angleChange, int bulletCount, float fireRate)
	{
		float angle = startAngle;
		for(int n = 0; n < bulletCount; n ++){
			MakeBulletAt(startPos, angle);
			angle += angleChange;
			primaryFireTimer = 0f;
			yield return new WaitForSeconds(fireRate);
		}
	}

	private IEnumerator BulletSpiral2(Vector3 startPos, float startAngle, float angleChange, int bulletCount, float fireRate)
	{
		float angle = startAngle;
		MakeRocket();
		for(int n = 0; n < bulletCount; n ++){
			MakeBulletAt(startPos, angle);
			angle += angleChange;
			primaryFireTimer = 0f;
			yield return new WaitForSeconds(fireRate);
		}
		MakeRocket();
	}

	protected void Fire(){
		StartCoroutine(BulletSpiral(Vector3.up,70, -13, 9, 0.05f));
		StartCoroutine(BulletSpiral(Vector3.down,-70, 13, 9, 0.05f));
	}

	protected void Fire2(){
		StartCoroutine(BulletSpiral2(Vector3.right*0.2f,0, -13, 44, 0.03f));
	}

	protected void Fire3(){
		StartCoroutine(BulletSpiral(Vector3.up,70, -13, 9, 0.05f));
		StartCoroutine(BulletSpiral(Vector3.down,-70, 13, 9, 0.05f));
		StartCoroutine(BulletSpiral2(Vector3.right*0.2f,0, -13, 44, 0.03f));
	}
}
