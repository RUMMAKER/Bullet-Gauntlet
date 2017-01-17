using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship_3 : Ship {

	GameObject smallBeamPrefab;
	GameObject midBeamPrefab;
	GameObject largeBeamPrefab;
	GameObject largeBeamInstance;

	public AudioClip laserSound;

	override protected void Start(){
		base.Start();
		primaryFire = "LMB";
		secondaryFire = "RMB";
		specialFire = "space";

		smallBeamPrefab = Resources.Load<GameObject>("Prefabs/PlayerProjectiles/SmallBeam");
		midBeamPrefab = Resources.Load<GameObject>("Prefabs/PlayerProjectiles/MediumBeam");
		largeBeamPrefab = Resources.Load<GameObject>("Prefabs/PlayerProjectiles/PlayerBigLazer");

		emission = Resources.Load<GameObject>("Prefabs/Effects/Emissions/SmokeEmission_3");

		primaryFireCooldown = 0.7f;
		primaryFireCost = 1.5f;
		secondaryFireCooldown = 1.2f;
		secondaryFireCost = 3f;
		specialFireCooldown = 2f;
		specialFireCost = 15f;

		maxHealth = 20;
		maxEnergy = 50;
		health = 20;
		energy = 50;
		energyRecoverRate = 2.75f;
		maxSpeed = 3.5f;
		accelSpeed = 100f;
		decelSpeed = 50f;
		turnSpeed = 900f;
		explosionSize = 2.6f;
		active = false;
		
		transform.rotation = Quaternion.Euler(0, 0, 90);
	}

	override protected void Update(){
		if(largeBeamInstance != null){
			turnSpeed = 100f;
		} else {
			turnSpeed = 900f;
		}
		base.Update();
	}

	override protected void Fire1(){
		GameObject obj = Instantiate(smallBeamPrefab);
		obj.transform.position = transform.position + Vector3.forward;
		obj.transform.rotation = transform.rotation;
	}

	override protected void Fire2(){
		GameObject obj = Instantiate(midBeamPrefab);
		obj.transform.position = transform.position + Vector3.forward;
		obj.transform.rotation = transform.rotation;
	}

	override protected void Fire3(){
		largeBeamInstance = Instantiate(largeBeamPrefab);
		largeBeamInstance.transform.parent = transform;
		largeBeamInstance.transform.localPosition = Vector3.right*0.1f + Vector3.forward*3f;
		largeBeamInstance.transform.localRotation = Quaternion.Euler(0,0,0);

		SoundControl.instance.PlaySound(laserSound);
	}

	override protected void PrimaryShoot(){
		if(largeBeamInstance == null){
			base.PrimaryShoot();
		}
	}

	override protected void SecondaryShoot(){
		if(largeBeamInstance == null){
			base.SecondaryShoot();
		}
	}
}
