using UnityEngine;
using System.Collections;

public class HomingMissle : Rocket {

	protected float turnSpeed;
	private Transform targetEnemy;
	private Transform player;
	private float count = 0f;
	override protected void Start () {
		base.Start();
		turnSpeed = 200f;
		targetEnemy = FindNearestEnemy();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	override protected void Update () {
		turnSpeed += 50f*Time.deltaTime;
		if(turnSpeed > 600f) turnSpeed = 600f;
		if(targetEnemy == null){
			if(count > 3f) {
				Destroy(gameObject);
				return;
			}
			if(player != null){
				TurnTowardsAngle(Mathf.Atan2((player.position.y - transform.position.y),(player.position.x - transform.position.x))*Mathf.Rad2Deg);
			}
			targetEnemy = FindNearestEnemy();
		} else {
			TurnTowardsAngle(Mathf.Atan2((targetEnemy.position.y - transform.position.y),(targetEnemy.position.x - transform.position.x))*Mathf.Rad2Deg);
		}
		base.Update();
		KeepInBounds();
		count += Time.deltaTime;
	}

	private Transform FindNearestEnemy(){
		//NOT nearest, random
		GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
		if(enemyList.Length < 1){
			return null;
		}
		targetEnemy = enemyList[Random.Range(0,enemyList.Length)].transform;
		return targetEnemy;
	}

	virtual protected void TurnTowardsAngle (float newAngle) {
		if (newAngle < 0) {
			newAngle += 360;
		}
		float angle = transform.rotation.eulerAngles.z;
		float turnDegree = newAngle - angle;//how far to turn to match newAngle
		float turnRate = turnSpeed * Time.deltaTime;
		float difference = Mathf.Abs(turnDegree);
		if(difference > 180f) difference = 360f - difference;

		if(turnRate > difference)//will over turn
		{
			turnRate = difference;
		}
		
		if(turnDegree < 180 && turnDegree > 0)
		{
			transform.Rotate(0,0,turnRate);
		}
		else if(turnDegree >= 180 && turnDegree < 360)
		{
			transform.Rotate(0,0,turnRate*-1);
		}
		else if(turnDegree < 0 && turnDegree >= -180)
		{
			transform.Rotate(0,0,turnRate*-1);
		}
		else if(turnDegree < 0 && turnDegree < -180)
		{
			transform.Rotate(0,0,turnRate);
		}
	}
}
