using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerWave : PlayerBullet {

	private List<GameObject> hitList;
	public float minSpeed;
	public float deccel;
	private SpriteRenderer rend;
	private float count;
	override protected void Start(){
		base.Start();
		hitList = new List<GameObject>();
		rend = GetComponent<SpriteRenderer>();
	}

	override protected void Update () {
		base.Update();
		rend.color = new Color(1f,1f,1f,Mathf.Clamp01(count));
		count += 11f*Time.deltaTime;
		speed -= deccel*Time.deltaTime;
		if(speed < minSpeed){
			speed = minSpeed;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		Enemy e = other.gameObject.GetComponent<Enemy>();
		if(e != null && !hitList.Contains(other.gameObject)){
			hitList.Add(other.gameObject);
			e.TakeDamage(damage);
		}
	}
}
