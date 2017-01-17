using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float speed = 15f;
	public float damage = 1f;

	private float screenVertical;
	private float screenHorizontal;

	virtual protected void Start(){
		screenVertical = Camera.main.orthographicSize * 2.0f;
		screenHorizontal = screenVertical * Screen.width / Screen.height;
	}

	virtual protected void Update () {
		DestroyWhenOutOfBounds();
		Move();
	}

	public void SetSpeed(float speed){
		this.speed = speed;
	}

	virtual protected void Move(){
		transform.Translate(new Vector3(speed*Time.deltaTime, 0f, 0f));
	}

	protected void DestroyWhenOutOfBounds(){
		if( transform.position.x > screenHorizontal*0.5f + 0.5f || 
			transform.position.x < -screenHorizontal*0.5f - 0.5f ||
			transform.position.y > screenVertical*0.5f + 0.5f ||
			transform.position.y < -screenVertical*0.5f - 0.5f)
		{
			Die();
		}
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

	virtual protected void Die(){
		Destroy(gameObject);
	}
}
