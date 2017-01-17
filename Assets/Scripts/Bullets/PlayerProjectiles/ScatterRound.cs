using UnityEngine;
using System.Collections;
//require spriterend
public class ScatterRound : PlayerBullet {

	private float lifeTime;
	private SpriteRenderer spriteRenderer;
	private float count;
	override protected void Start(){
		base.Start();
		lifeTime = 0.4f;
		spriteRenderer = GetComponent<SpriteRenderer>();
		StartCoroutine("TimeOut");
	}

	override protected void Update () {
		base.Update();
		spriteRenderer.color = new Color(1f-0.4f*count/lifeTime,1f-0.4f*count/lifeTime,1f-0.4f*count/lifeTime,1f-0.4f*count/lifeTime);
		count += Time.deltaTime;
		speed -= Time.deltaTime*8f;
	}

	void OnTriggerEnter2D(Collider2D other){
		Enemy e = other.gameObject.GetComponent<Enemy>();
		if(e != null){
			e.TakeDamage(damage);
			Destroy(gameObject);
		}
	}

	private IEnumerator TimeOut() {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
