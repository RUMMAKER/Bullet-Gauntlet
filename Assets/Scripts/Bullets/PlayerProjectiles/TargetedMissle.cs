using UnityEngine;
using System.Collections;

public class TargetedMissle : Rocket {

	protected float turnSpeed;
	protected Vector2 target;
	override protected void Start () {
		base.Start();
		turnSpeed = 100f;
		target = GetMousePos();
	}
	
	override protected void Update () {
		TurnTowardsAngle(Mathf.Atan2((target.y - transform.position.y),(target.x - transform.position.x))*Mathf.Rad2Deg);
		base.Update();
	}

	protected Vector2 GetMousePos () {
		return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
