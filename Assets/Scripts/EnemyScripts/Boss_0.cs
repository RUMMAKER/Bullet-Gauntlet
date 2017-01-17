using UnityEngine;
using System.Collections;

public class Boss_0 : BossBase {

	private float timeSinceFired;
	private float timeSinceFired2;
	private GameObject rocketPrefab;
	private GameObject misslePrefab;

	private float chargeSpeed;
	private bool charging;
	private bool recovered;
	private bool firing2;
	private float windup;
	private float recoverCounter = 0f;

	private float count;
	override protected void Start () {
		base.Start();
		firing2 = false;
		recovered = true;
		charging = false;
		if(player == null){
			player = GameObject.FindWithTag("Player");
		}
		explosionSize = 2.7f;
		bodyDamage = 150f;
		health = 500f;
		rocketPrefab = Resources.Load<GameObject>("Prefabs/EnemyProjectiles/EnemyRocket");
		misslePrefab = Resources.Load<GameObject>("Prefabs/EnemyProjectiles/EnemyMissle");
		timeSinceFired2 = 0f;
		
		speed = 1f;
		turnSpeed = 210f;
	}
	
	override protected void Update () {
		count += Time.deltaTime;
		if(health > 400f && count < 25f) {
			StageOne();
		} else if(health > 300f) {
			StageTwo();
		} else {
			StageThree();
		}
	}

	private void StageOne() {
		base.Update();
		if(timeSinceFired > 0.7f) {
			Fire();
		}
		timeSinceFired += Time.deltaTime;
	}

	private void StageTwo() {
		StageOne();
		if(timeSinceFired2 > 1.5f*Mathf.Clamp01(health/300f) && !firing2) {
			timeSinceFired2 = 0f;
			Fire2();
		}
		timeSinceFired2 += Time.deltaTime;
	}

	private void StageThree() {
		if(charging){
			windup += Time.deltaTime;
			if(windup > Mathf.Clamp(health/200f,0.8f,1.5f)) {
				Charge();
			}
			timeSinceFired += Time.deltaTime;
			timeSinceFired2 += Time.deltaTime;
			return;
		}
		if(!recovered){

			if(recoverCounter < 0.6f) {
				base.Update();
				recoverCounter += Time.deltaTime;
			} else {
				if(timeSinceFired2 <= 1.5f*Mathf.Clamp01(health/300f)) 
				{
					timeSinceFired2 = 1.5f*Mathf.Clamp01(health/300f);
				}
				recovered = true;
			}
			timeSinceFired += Time.deltaTime;
			timeSinceFired2 += Time.deltaTime;
			return;
		}
		StageTwo();
		if(!charging && (timeSinceFired2 > 1.5*(health/300f) || !firing2)) {
			StartCharge();
		}
	}

	private void Charge(){
		turnSpeed = 14f + Mathf.Clamp01(chargeSpeed*2f)*4f;
		TurnTowardsPlayer();

		float tempSpeed;
		tempSpeed = Mathf.Lerp(0f, speed*7f, chargeSpeed*1.8f);

		transform.Translate(tempSpeed*Vector3.right*Time.deltaTime);
		
		if (OutOfBounds()) { //stop charging
			charging = false;
			chargeSpeed = 0f;
			turnSpeed = 300f;
			PickCorner();
			return;
		}
		chargeSpeed += Time.deltaTime;
	}

	private void StartCharge(){
		charging = true;
		windup = 0f;
		recoverCounter = 0f;
		recovered = false;
	}

	private void MakeRocket(){
		GameObject rocket = GameObject.Instantiate(rocketPrefab);
		rocket.transform.rotation = Quaternion.Euler(0,0,90f + transform.rotation.eulerAngles.z);
		rocket.transform.position = transform.position + Vector3.back;

		GameObject rocket2 = GameObject.Instantiate(rocketPrefab);
		rocket2.transform.rotation = Quaternion.Euler(0,0,-90f + transform.rotation.eulerAngles.z);
		rocket2.transform.position = transform.position + Vector3.back;

		GameObject rocket3 = GameObject.Instantiate(rocketPrefab);
		rocket3.transform.rotation = Quaternion.Euler(0,0,-120f + transform.rotation.eulerAngles.z);
		rocket3.transform.position = transform.position + Vector3.back;

		GameObject rocket4 = GameObject.Instantiate(rocketPrefab);
		rocket4.transform.rotation = Quaternion.Euler(0,0,120f + transform.rotation.eulerAngles.z);
		rocket4.transform.position = transform.position + Vector3.back;
	}

	private void MakeMissle(){
		Vector3 rot = transform.rotation.eulerAngles;
		for(int n = 0; n < 7; n ++){
			GameObject bullet = GameObject.Instantiate(misslePrefab);
			bullet.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z -3*16f + 16f * n);
			bullet.transform.parent = transform;
			bullet.transform.localPosition = Vector3.down*0.45f + Vector3.up*0.15f*n + Vector3.back;
			bullet.transform.parent = null;
		}
	}

	private IEnumerator MakeMissleThreeTimes()
	{
		for(int n = 0; n < 3; n ++){
			MakeMissle();
			yield return new WaitForSeconds(0.5f);
		}
		MakeMissle();
		firing2 = false;
		timeSinceFired2 = 0f;
	}

	private void Fire(){
		MakeRocket();
		timeSinceFired = 0f;
	}

	protected void Fire2(){
		firing2 = true;
		StartCoroutine("MakeMissleThreeTimes");
	}
}
