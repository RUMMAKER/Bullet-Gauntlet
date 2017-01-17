using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship_2 : Ship {

	GameObject rocketPrefab;
	GameObject guidedPrefab;
	GameObject homingPrefab;

	override protected void Start(){
		base.Start();
		primaryFire = "LMB";
		secondaryFire = "RMB";
		specialFire = "space";

		rocketPrefab = Resources.Load<GameObject>("Prefabs/PlayerProjectiles/Rocket");
		guidedPrefab = Resources.Load<GameObject>("Prefabs/PlayerProjectiles/TargetedMissle");
		homingPrefab = Resources.Load<GameObject>("Prefabs/PlayerProjectiles/HomingMissle");

		emission = Resources.Load<GameObject>("Prefabs/Effects/Emissions/SmokeEmission_2");

		primaryFireCooldown = 0.4f;
		primaryFireCost = 0.25f;
		secondaryFireCooldown = 1f;
		secondaryFireCost = 2.5f;
		specialFireCooldown = 1f;
		specialFireCost = 15f;

		maxHealth = 20;
		maxEnergy = 50;
		health = 20;
		energy = 50;
		energyRecoverRate = 1.5f;
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
		GameObject bullet = GameObject.Instantiate(rocketPrefab);
		bullet.transform.rotation = Quaternion.Euler(0,0,transform.rotation.eulerAngles.z);
		bullet.transform.position = transform.position;
	}

	override protected void Fire2(){
		Vector3 rot = transform.rotation.eulerAngles;
		for(int n = 0; n < 3; n ++){
			GameObject bullet = GameObject.Instantiate(guidedPrefab);

			bullet.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z -40f + 40f * n);
			bullet.transform.position = transform.position;
		}
	}
	
	override protected void Fire3(){
		Vector3 rot = transform.rotation.eulerAngles;
		for(int n = 0; n < 12; n ++){
			GameObject bullet = GameObject.Instantiate(homingPrefab);

			bullet.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z -30f*5 + 30f * n);
			bullet.transform.position = transform.position;
		}
	}
}
