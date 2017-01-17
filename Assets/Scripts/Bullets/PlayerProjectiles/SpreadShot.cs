using UnityEngine;
using System.Collections;

public class SpreadShot : PlayerBullet {

	private GameObject shrapnelPrefab;
	override protected void Start(){
		base.Start();
		shrapnelPrefab = Resources.Load<GameObject>("Prefabs/PlayerProjectiles/SpreadShrapnel");
	}

	override protected void Update () {
		base.Update();
	}

	void OnTriggerEnter2D(Collider2D other){
		Enemy e = other.gameObject.GetComponent<Enemy>();
		if(e != null){
			e.TakeDamage(damage);
			Die();
		}
	}

	override protected void Die(){
		//create 360 spread
		KeepInBounds();
		for(int n = 0; n < 60; n ++){
			GameObject obj = Instantiate(shrapnelPrefab);
			obj.transform.parent = transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.Euler(0,0,-29*6f + 6f*n);
			obj.transform.parent = null;
			obj.transform.Translate(Vector3.right*0.25f);
		}
		Destroy(gameObject);
	}
}
