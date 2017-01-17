using UnityEngine;
using System.Collections;

public class EnemyScatter : EnemySniper {

	override protected void Start () {
		base.Start();
		rocketPrefab = Resources.Load<GameObject>("Prefabs/EnemyProjectiles/EnemyBullet");
		fireRate = 2.3f;
	}

	override protected void MakeBullet(){
		Vector3 rot = transform.rotation.eulerAngles;
		for(int n = 0; n < 6; n ++){
			GameObject bullet = GameObject.Instantiate(rocketPrefab);
			bullet.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z -16f*2 + 16f * n);
			bullet.transform.position = transform.position + Vector3.back;
		}
	}
}
