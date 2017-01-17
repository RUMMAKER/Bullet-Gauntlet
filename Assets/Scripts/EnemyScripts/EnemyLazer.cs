using UnityEngine;
using System.Collections;

public class EnemyLazer : EnemySniper {

	override protected void Start () {
		base.Start();
		health = 12f;
		rocketPrefab = Resources.Load<GameObject>("Prefabs/EnemyProjectiles/EnemyBigBeam");
		timeSinceFired = 2f;
		fireRate = 4f;
	}

	override protected void Update () {
		if(timeSinceFired < 2.6f){//laser lifetime
			turnSpeed = 10f;
			speed = 0.1f;
		} else {
			turnSpeed = 300f;
			speed = 1.1f;
		}
		base.Update();
	}

	override protected void MakeBullet(){
		for(int n = 0; n < 6; n ++){
			GameObject lazer = GameObject.Instantiate(rocketPrefab);
			lazer.transform.parent = transform;
			lazer.transform.localPosition = Vector3.right*0.2f + Vector3.forward;
			lazer.transform.localRotation = Quaternion.Euler(0f,0f,0f);
		}
	}
}
