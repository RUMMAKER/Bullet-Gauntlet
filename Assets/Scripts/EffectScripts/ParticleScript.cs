using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SpriteRenderer))]
public class ParticleScript : MonoBehaviour {

	
	public float startSpeed;
	public float endSpeed;
	public float startSize;
	public float endSize;
	public float lifeTime;
	public Color startColor;
	public Color endColor;

	private float timeSinceAlive;
	private float speed;
	private float size;
	private SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Awake () {
		startSpeed += Random.Range(0f, startSpeed/4f);
		endSpeed += Random.Range(0f, endSpeed/4f);
		startSize += Random.Range(0f, startSize/4f);
		endSize += Random.Range(0f, endSize/4f);
		lifeTime += Random.Range(0f, lifeTime/4f);

		timeSinceAlive = 0.1f;
		speed = startSpeed;
		size = startSize;
		spriteRenderer = GetComponent<SpriteRenderer>();
		Fade();
		Shrink();
	}
	
	// Update is called once per frame
	void Update () {
		if(timeSinceAlive > lifeTime) Destroy(gameObject);
		Move();
		Fade();
		Shrink();
		timeSinceAlive += Time.deltaTime;
	}

	public void SetAll(float startSpeed, float endSpeed, float startSize, float endSize, float lifeTime, Color startColor, Color endColor){
		this.startSpeed = startSpeed;
		this.endSpeed = endSpeed;
		this.startSize = startSize;
		this.endSize = endSize;
		this.lifeTime = lifeTime;
		this.startColor = startColor;
		this.endColor = endColor;
		Fade();
		Shrink();
	}

	private void Move(){
		speed = Mathf.Lerp(startSpeed, endSpeed, (timeSinceAlive/lifeTime));
		transform.Translate(Vector3.right*speed*Time.deltaTime);
	}

	private void Fade(){
		spriteRenderer.color = Color.Lerp(startColor, endColor, (timeSinceAlive/lifeTime));
	}

	private void Shrink(){
		size = Mathf.Lerp(startSize, endSize, (timeSinceAlive/lifeTime));
		transform.localScale = new Vector3(size,size,1);
	}
}
