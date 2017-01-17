using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship_1 : Ship {

	GameObject bulletPrefab;
	GameObject spreadPrefab;
	GameObject afterImage;

	public AudioClip shootingSound;
	override protected void Start(){
		base.Start();
		primaryFire = "LMB";
		secondaryFire = "RMB";
		specialFire = "space";

		bulletPrefab = Resources.Load<GameObject>("Prefabs/PlayerProjectiles/ScatterRound");
		spreadPrefab = Resources.Load<GameObject>("Prefabs/PlayerProjectiles/360SpreadShot");
		afterImage = Resources.Load<GameObject>("Prefabs/Effects/AfterImage");

		emission = Resources.Load<GameObject>("Prefabs/Effects/Emissions/SmokeEmission_1");

		primaryFireCooldown = 0.4f;
		primaryFireCost = 0.2f;
		secondaryFireCooldown = 0f;
		secondaryFireCost = 2.5f;
		specialFireCooldown = 1f;
		specialFireCost = 15f;

		maxHealth = 20;
		maxEnergy = 50;
		health = 20;
		energy = 50;
		energyRecoverRate = 1.5f;
		maxSpeed = 3.8f;
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
		for(int n = 0; n < 8; n ++){
			GameObject bullet = GameObject.Instantiate(bulletPrefab);

			bullet.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z -4f*3f + 4f * n);
			bullet.transform.position = transform.position + Vector3.up*Random.Range(-0.2f, 0.2f) + Vector3.left*Random.Range(-0.2f, 0.2f);
		}
		SoundControl.instance.PlaySound(shootingSound);
	}

	override protected void Fire2(){
		Vector2 direction = GetMousePos() - (Vector2)transform.position;
		if(direction.magnitude > 2.3f){
			direction = direction.normalized*2.3f;
		}
		GameObject obj = Instantiate(afterImage);
		obj.transform.rotation = transform.rotation;
		obj.transform.position = transform.position;
		transform.position = transform.position + (Vector3)direction;
	}

	override protected void Fire3(){
		GameObject spread = GameObject.Instantiate(spreadPrefab);
		spread.transform.rotation = Quaternion.Euler(0,0,transform.rotation.eulerAngles.z);
		spread.transform.position = transform.position + Vector3.forward;
	}	

	override protected void SecondaryShoot(){
		if(energy < secondaryFireCost) return;
		if(secondaryFire == "LMB"){
			if (Input.GetMouseButtonDown(0)){
				//if energy sufficient
				if(secondaryFireTimer > secondaryFireCooldown){
					Fire2();
					UseEnergy(secondaryFireCost);
					secondaryFireTimer = 0;
				}
			}
		} else if(secondaryFire == "RMB"){
			if (Input.GetMouseButtonDown(1)){
				//if energy sufficient
				if(secondaryFireTimer > secondaryFireCooldown){
					Fire2();
					UseEnergy(secondaryFireCost);
					secondaryFireTimer = 0;
				}
			}
		} else {
			if (Input.GetKeyDown(secondaryFire)){
				//if energy sufficient
				if(secondaryFireTimer > secondaryFireCooldown){
					Fire2();
					UseEnergy(secondaryFireCost);
					secondaryFireTimer = 0;
				}
			}
		}
	}
}
