using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public float size;
	private bool exploded = false;
	private bool finishExplosion = false;
	private float life;

	public void SetSize(float size){
		this.size = size;
		size = size*1.05f;//too lazy to resize everything
		life = 0.3f + size*0.2f;
	}

	void Update () {
		if(!exploded){
			Explode();
		}
		if(finishExplosion){
			Destroy(gameObject);
		}
	}

	private void HelperExplode(){
		GameObject obj = ParticleFactory.MakeParticle(0f, 0f, size*1.6f, size*1.4f, life*0.7f, new Color(1f,1f,0.5f,1f), new Color(1f,1f,0.5f,0.2f));
		obj.transform.position = transform.position + Vector3.forward;

		GameObject obj2 = ParticleFactory.MakeParticle(0f, 0f, size, size*0.4f, life, new Color(1f,0.1f,0.5f,1f), new Color(0.7f,0.6f,0.05f,0.2f));
		obj2.transform.position = transform.position + Vector3.forward*2f;

		GameObject obj3 = ParticleFactory.MakeHollowParticle(0f, 0f, size/10f, size/8f, life*0.5f, new Color(1f,1f,0.5f,1f), new Color(1f,0.9f,0.5f,0.5f));
		obj3.transform.position = transform.position;

		GameObject obj4 = ParticleFactory.MakeHollowParticle(0f, 0f, size/12f, size/5f, life*0.3f, new Color(1f,1f,1f,0.5f), new Color(1f,1f,1f,0.1f));
		obj4.transform.position = transform.position;

		float angle1 = Random.Range(0f,360f);
		float angle2 = Random.Range(0f,360f);
		float angle3 = angle2 + 180f + Random.Range(-40f,40f);
		float angle4 = Random.Range(0f,360f);
		StartCoroutine(ExplosionTrail(size*1.1f*Random.Range(0.9f,1.1f), angle1));
		StartCoroutine(ExplosionTrail(size*Random.Range(0.9f,1.1f), angle2));
		StartCoroutine(ExplosionTrail(size*0.9f*Random.Range(0.9f,1.1f), angle3));
		StartCoroutine(ExplosionTrail(size*0.7f*Random.Range(0.9f,1.1f), angle4));
		exploded = true;
		SoundControl.instance.PlayExplodeSound(size);
	}

	private void Explode(){
		if(size < 2.5f){
			HelperExplode();
		} else {
			//multi boss explosion
			StartCoroutine(MultiExplosion(size));
			exploded = true;
		}
	}

	private IEnumerator ExplosionTrail(float size, float angle)
	{
		Vector2 direction = new Vector2(Mathf.Cos(angle*Mathf.Deg2Rad), Mathf.Sin(angle*Mathf.Deg2Rad));
		direction.Normalize();
		float accelDistance = 1f;
		for(int n = 1; n <= 10; n ++){
			accelDistance += 0.05f;
			float s = size - n*0.12f;
			GameObject obj = MakeParticle(s, n/10f);
			obj.transform.parent = transform;
			obj.transform.localPosition = (Vector3)(n*accelDistance*direction*s/9f) + Vector3.forward*2f
										  + new Vector3(Random.Range(-s/80f, s/80f), Random.Range(-s/80f, s/80f), 0f);
			obj.transform.parent = null;
			yield return null;
		}
		finishExplosion = true;
	}

	private IEnumerator MultiExplosion(float size)
	{
		for(int n = 0; n < 5; n ++){
			GameObject obj = ParticleFactory.MakeExplosion(Random.Range(1.4f, 1.8f));
			obj.transform.position = transform.position 
									 + Vector3.left * Random.Range(-0.6f, 0.6f)
									 + Vector3.up * Random.Range(-0.6f, 0.6f);
			yield return new WaitForSeconds(0.1f);
		}
		HelperExplode();
	}

	private GameObject MakeParticle(float s, float distance){
		if(s < 0.1f) {
			s = 0.1f;
		}
		GameObject effect = ParticleFactory.MakeParticle(0f, 0f, s, s/2f, life - distance*0.1f, new Color(1f,distance,0.2f,1f), new Color(0.7f,0.6f,0.05f,0.2f));
		return effect;
	}
}
