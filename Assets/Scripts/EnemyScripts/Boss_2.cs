using UnityEngine;
using System.Collections;

public class Boss_2 : BossBase {

	private float primaryFireTimer;
	private float primaryRate;
	private float secondaryFireTimer;
	private float secondaryRate;
	private bool spraying;

	private GameObject bulletPrefab;
	private GameObject bigBulletPrefab;
	private GameObject bigBeamPrefab;
	private GameObject bigBeam;
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

		primaryFireTimer = 1f;
		primaryRate = 3f;
		secondaryFireTimer = -1f;
		secondaryRate = 1f;

		bulletPrefab = Resources.Load<GameObject>("Prefabs/EnemyProjectiles/EnemyBullet");
		bigBulletPrefab = Resources.Load<GameObject>("Prefabs/EnemyProjectiles/EnemyOrb");
		bigBeamPrefab = Resources.Load<GameObject>("Prefabs/EnemyProjectiles/EnemyBigBeam_Boss");
	}
	
	override protected void Update () {
		count += Time.deltaTime;
		if(health > 350f && count < 30f){
			StageOne();
		} else if(health > 150f){
			StageTwo();
		} else if(bigBeam == null){
			StageThree();
		} else {
			base.Update();
		}
	}

	private void StageOne(){
		base.Update();
		if(primaryFireTimer > primaryRate){
			Fire();
		}
		primaryFireTimer += Time.deltaTime;
	}

	private void StageTwo(){
		base.Update();
		if(bigBeam == null){
			turnSpeed = 200f;
			speed = 1f;
			secondaryFireTimer += Time.deltaTime;
		} else {
			turnSpeed = 10f;
			speed = 0.2f;
		}
		if(secondaryFireTimer > secondaryRate){
			Fire2();
			StartCoroutine("BulletOrbs");
		}
	}

	private void StageThree(){
		if(!spraying){
			turnSpeed = 200f;
			speed = 1f;
			StartCoroutine("BulletSpray");
			spraying = true;
		}
		StageOne();
	}

	private void MakeBulletAt(GameObject prefab, Vector3 pos, float angle){
		GameObject bullet = GameObject.Instantiate(prefab);
		bullet.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + angle);
		bullet.transform.parent = transform;
		bullet.transform.localPosition = Vector3.right * 0.25f + pos + Vector3.back;
		bullet.transform.parent = null;
	}

	private void MakeBeam(float rot, Vector3 pos){
		GameObject beam = GameObject.Instantiate(bigBeamPrefab);
		beam.transform.parent = transform;
		beam.transform.localRotation = Quaternion.Euler(0,0,rot);
		beam.transform.localPosition = Vector3.forward*2f + pos;
		bigBeam = beam;
	}

	private void MakeManyBeams(){
		MakeBeam(0f, Vector3.zero);
		MakeBeam(0f, Vector3.up*0.1f);
		MakeBeam(0f, Vector3.down*0.1f);
	}

	private IEnumerator BulletOrbs()
	{
		MakeBulletAt(bigBulletPrefab, Vector3.zero, 35f);
		MakeBulletAt(bigBulletPrefab, Vector3.zero, -35f);
		primaryFireTimer = 0f;
		yield return new WaitForSeconds(0.3f);
		MakeBulletAt(bigBulletPrefab, Vector3.zero, 55f);
		MakeBulletAt(bigBulletPrefab, Vector3.zero, -55f);
		primaryFireTimer = 0f;
	}

	private IEnumerator BulletOrbsMeh()
	{
		MakeBulletAt(bigBulletPrefab, Vector3.zero, 40f);
		primaryFireTimer = 0f;
		yield return new WaitForSeconds(0.3f);
		MakeBulletAt(bigBulletPrefab, Vector3.zero, 0f);
		primaryFireTimer = 0f;
		yield return new WaitForSeconds(0.3f);
		MakeBulletAt(bigBulletPrefab, Vector3.zero, -40f);
		primaryFireTimer = 0f;
	}

	private IEnumerator BulletSpray()
	{
		while(true){
			MakeBulletAt(bulletPrefab, Vector3.left*0.1f, Random.Range(-70f,70f));
			yield return new WaitForSeconds(0.1f);
		}
	}

	protected void Fire(){
		StartCoroutine("BulletOrbsMeh");
		primaryFireTimer = 0f;
	}

	protected void Fire2(){
		MakeManyBeams();
		secondaryFireTimer = 0f;
	}
}