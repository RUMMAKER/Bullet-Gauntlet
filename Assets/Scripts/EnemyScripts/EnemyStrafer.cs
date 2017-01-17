using UnityEngine;
using System.Collections;

public class EnemyStrafer : Enemy {

	private float turnDestination;
	override protected void Start () {
		base.Start();
		explosionSize = 1.5f;
		speed = 2.3f;
		turnSpeed = 200f;
		turnDestination = 0f;
	}
	
	override protected void Update () {
		base.Update();
		TurnTowardsAngle(turnDestination);
		transform.Translate(speed*Vector3.right*Time.deltaTime);
		KeepInBounds();
	}

	private void UpdateDirection(int side){
		switch(side){
			case 0://up
				if(transform.rotation.eulerAngles.z > 90f && transform.rotation.eulerAngles.z < 180f){
					turnDestination = 225f;
				} else {
					turnDestination = 315f;
				}
				break;
			case 1://left
				if(transform.rotation.eulerAngles.z > 180f && transform.rotation.eulerAngles.z < 270f){
					turnDestination = 315f;
				} else {
					turnDestination = 45f;
				}
				break;
			case 2://down
				if(transform.rotation.eulerAngles.z > 180f && transform.rotation.eulerAngles.z < 270f){
					turnDestination = 135f;
				} else {
					turnDestination = 45f;
				}
				break;
			case 3://right
				if(transform.rotation.eulerAngles.z > 0f && transform.rotation.eulerAngles.z < 90f){
					turnDestination = 135f;
				} else {
					turnDestination = 225f;
				}
				break;
		}
	}

	override protected void KeepInBounds(){
		if(transform.position.x > screenHorizontal/2f){
			transform.position = new Vector3(screenHorizontal/2f,transform.position.y,transform.position.z);
			UpdateDirection(3);
		} else if(transform.position.x < -screenHorizontal/2f){
			transform.position = new Vector3(-screenHorizontal/2f,transform.position.y,transform.position.z);
			UpdateDirection(1);
		} 
		if(transform.position.y > screenVertical/2f){
			transform.position = new Vector3(transform.position.x,screenVertical/2f,transform.position.z);
			UpdateDirection(0);
		} else if(transform.position.y < -screenVertical/2f){
			transform.position = new Vector3(transform.position.x,-screenVertical/2f,transform.position.z);
			UpdateDirection(2);
		}
	}
}
