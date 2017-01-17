using UnityEngine;
using System.Collections;

public class Ship_4 : Ship {

	private bool slowTime;
	private float timeSlowCost;
	GameObject wavePrefab;
	GameObject bulletPrefab;
	private bool alternatingPrimary = false;

	public AudioClip shootingSound;
	override protected void Start () {
		base.Start();
		bulletPrefab = Resources.Load<GameObject>("Prefabs/PlayerProjectiles/SpeedyPlayerBullet");
		wavePrefab = Resources.Load<GameObject>("Prefabs/PlayerProjectiles/PlayerWave");
		emission = Resources.Load<GameObject>("Prefabs/Effects/Emissions/SmokeEmission_2");
		transform.rotation = Quaternion.Euler(0, 0, 90);

		slowTime = false;

		maxHealth = 20;
		maxEnergy = 50;
		health = 20;
		energy = 50;
		energyRecoverRate = 3f;
		maxSpeed = 3.5f;
		accelSpeed = 100f;
		decelSpeed = 50f;
		turnSpeed = 900f;
		explosionSize = 2.6f;
		active = false;

		primaryFireCooldown = 0.3f;
		primaryFireCost = 0.3f;

		secondaryFireCooldown = 0f;
		secondaryFireCost = 0f;
		timeSlowCost = 7f;

		specialFireCooldown = 1f;
		specialFireCost = 15f;
	}
	
	override protected void Update () {
		base.Update();
		if(slowTime){
			UseEnergy(timeSlowCost*Time.deltaTime);
			if(energy < 0.2f){
				NormalTime();
			}
		}
	}

	override protected void Fire1(){
		Vector3 rot = transform.rotation.eulerAngles;	
		GameObject bullet = GameObject.Instantiate(bulletPrefab);
		bullet.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
		if(alternatingPrimary){
			bullet.transform.parent = transform;
			bullet.transform.localPosition = Vector3.zero;
			bullet.transform.localPosition += Vector3.forward + Vector3.up*0.07f;
			bullet.transform.parent = null;

			GameObject flare = ParticleFactory.MakeFlare(Random.Range(0.3f,0.6f), Random.Range(1.7f,2f), 0.4f, Color.black, Color.yellow);
			flare.transform.parent = transform;
			flare.transform.position = bullet.transform.position;
			flare.transform.localPosition += Vector3.right*Random.Range(0.1f,0.13f) + Vector3.up*Random.Range(-0.03f,0.05f);
			flare.transform.localRotation = Quaternion.identity;
		} else {
			bullet.transform.parent = transform;
			bullet.transform.localPosition = Vector3.zero;
			bullet.transform.localPosition += Vector3.forward + Vector3.up*-0.07f;
			bullet.transform.parent = null;

			GameObject flare = ParticleFactory.MakeFlare(Random.Range(0.3f,0.6f), Random.Range(1.7f,2f), 0.4f, Color.black, Color.yellow);
			flare.transform.parent = transform;
			flare.transform.position = bullet.transform.position;
			flare.transform.localPosition += Vector3.right*Random.Range(0.1f,0.13f) + Vector3.up*Random.Range(-0.05f,0.03f);
			flare.transform.localRotation = Quaternion.identity;
		}
		
		alternatingPrimary = !alternatingPrimary;
		SoundControl.instance.PlaySound(shootingSound);
	}

	override protected void Fire2(){
		if(energy < 1f){
			return;
		}
		SlowTime();
	}

	protected void EndFire2(){
		NormalTime();
	}

	override protected void Fire3(){
		GameObject wave = GameObject.Instantiate(wavePrefab);
		wave.transform.rotation = transform.rotation;
		wave.transform.parent = transform;
		wave.transform.localPosition = Vector3.right*0.3f + Vector3.forward;
		wave.transform.parent = null;
	}

	void OnDisable(){
		NormalTime();
	}

	void SlowTime(){
		Time.timeScale = 0.5f;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
		slowTime = !slowTime;
	}

	void NormalTime(){
		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
		slowTime = !slowTime;
	}

	override protected void SecondaryShoot(){//toggle
		if(energy < secondaryFireCost) return;

		if (Input.GetMouseButtonDown(1)){
			Fire2();
		} else if(Input.GetMouseButtonUp(1)){
			EndFire2();
		}
	}
	
}
