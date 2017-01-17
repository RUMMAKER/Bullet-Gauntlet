using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	protected float health;
	protected float speed;
	protected float turnSpeed;

	protected float screenVertical;
	protected float screenHorizontal;
	protected float explosionSize;
	protected float bodyDamage;

	private GameObject spark;
	private float sparkTimer;

	protected virtual void Start () {
		spark = Resources.Load<GameObject>("Prefabs/Effects/Spark");
		screenVertical = Camera.main.orthographicSize * 2.0f;
		screenHorizontal = screenVertical * Screen.width / Screen.height;
		bodyDamage = 10f;
	}
	
	protected virtual void Update(){
		sparkTimer += Time.deltaTime;
	}

	virtual protected void KeepInBounds(){
		if(transform.position.x > screenHorizontal/2f){
			transform.position = new Vector3(screenHorizontal/2f,transform.position.y,transform.position.z);
		} else if(transform.position.x < -screenHorizontal/2f){
			transform.position = new Vector3(-screenHorizontal/2f,transform.position.y,transform.position.z);
		} 
		if(transform.position.y > screenVertical/2f){
			transform.position = new Vector3(transform.position.x,screenVertical/2f,transform.position.z);
		} else if(transform.position.y < -screenVertical/2f){
			transform.position = new Vector3(transform.position.x,-screenVertical/2f,transform.position.z);
		}
	}

	protected bool OutOfBounds(){
		return (transform.position.x > screenHorizontal/2f || transform.position.x < -screenHorizontal/2f || transform.position.y > screenVertical/2f || transform.position.y < -screenVertical/2f);
	}

	public void TakeDamage(float dmg){
		health -= dmg;
		if(health <= 0){
			Die();
		}
	}

	virtual protected void Die(){
		Destroy(gameObject);
		GameObject explosion = ParticleFactory.MakeExplosion(explosionSize);
		explosion.transform.position = transform.position;
	}

	protected void TurnTowardsPosition (Vector2 pos) {
		TurnTowardsAngle(Mathf.Atan2((pos.y - transform.position.y),(pos.x - transform.position.x))*Mathf.Rad2Deg);
	}

	protected void TurnTowardsAngle (float newAngle) {
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

	void OnTriggerStay2D(Collider2D other) {
		Ship s = other.gameObject.GetComponent<Ship>();
		if(s != null){

			if(sparkTimer > 0.1f){
				GameObject obj = Instantiate(spark);
				obj.transform.position = other.transform.position + new Vector3(Random.Range(-0.1f,0.1f),Random.Range(-0.1f,0.1f),-1);
				obj.transform.rotation = Quaternion.Euler(0,0,Random.Range(0,360f));
				sparkTimer = 0f;
			}

			float dmg = bodyDamage*Time.deltaTime;
			if(dmg > health) dmg = health;
			if(s.GetHealth() > 0f){
				s.TakeDamage(dmg);
			}
			TakeDamage(dmg);
		}
	}
}
