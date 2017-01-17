using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour {

	public Color startColor;
	public Color endColor;
	public float startSize;
	public float endSize;
	public float rate;
	public float lifeTime;
	private float countTotal;
	private float rateCount;
	private SpriteRenderer spriteRenderer;
	
	void Start(){
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update () {
		if(rateCount > rate){
			rateCount = 0f;
		}
		//set size to be relative to rateCount
		spriteRenderer.color = Color.Lerp(startColor, endColor, rateCount/rate);
		float size = Mathf.Lerp(startSize, endSize, rateCount/rate);
		transform.localScale = new Vector3(size,size,1);
		//
		if(countTotal > lifeTime){
			Destroy(gameObject);
		}
		countTotal += Time.deltaTime;
		rateCount += Time.deltaTime;
	}
}
