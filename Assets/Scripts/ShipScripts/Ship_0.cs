using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship_0 : Ship {

	GameObject bulletPrefab;
	GameObject rocketPrefab;
	GameObject shockWavePrefab;
	
	private bool alternatingPrimary = false;
	public AudioClip shootingSound;

	override protected void Start(){
		base.Start();
		primaryFire = "LMB";
		secondaryFire = "RMB";
		specialFire = "space";

		bulletPrefab = Resources.Load<GameObject>("Prefabs/PlayerProjectiles/Bullet");
		rocketPrefab = Resources.Load<GameObject>("Prefabs/PlayerProjectiles/Rocket");
		shockWavePrefab = Resources.Load<GameObject>("Prefabs/PlayerProjectiles/ShockWave");

		emission = Resources.Load<GameObject>("Prefabs/Effects/Emissions/SmokeEmission_0");

		primaryFireCooldown = 0.13f;
		primaryFireCost = 0.2f;
		secondaryFireCooldown = 0.9f;
		secondaryFireCost = 2f;
		specialFireCooldown = 1f;
		specialFireCost = 15f;

		maxHealth = 20;
		maxEnergy = 50;
		health = 20;
		energy = 50;
		energyRecoverRate = 3;
		maxSpeed = 3.5f;
		accelSpeed = 100f;
		decelSpeed = 50f;
		turnSpeed = 900f;
		explosionSize = 2.6f;
		active = false;
		
		transform.rotation = Quaternion.Euler(0, 0, 90);
	}
	override protected void Update(){
		base.Update();
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
		Vector3 rot = transform.rotation.eulerAngles;
		GameObject rocket1 = GameObject.Instantiate(rocketPrefab);
		rocket1.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
		rocket1.transform.parent = transform;
		rocket1.transform.localPosition = Vector3.forward + Vector3.up*0.2f + Vector3.right*0.1f;
		rocket1.transform.parent = null;

		GameObject rocket2 = GameObject.Instantiate(rocketPrefab);
		rocket2.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
		rocket2.transform.parent = transform;
		rocket2.transform.localPosition = Vector3.forward + Vector3.up*-0.2f + Vector3.right*0.1f;
		rocket2.transform.parent = null;
	}

	override protected void Fire3(){
		GameObject obj = Instantiate(shockWavePrefab);
		obj.transform.position = transform.position;
	}
}
