using UnityEngine;
using System.Collections;

public class PlayerMedLazer : MonoBehaviour {

	public float damage = 10f;
	public float lifeTime = 0.3f;
	public float width = 0.14f;
	
	private float count = 0f;
	private LineRenderer lineRend;
	private Vector3 endPos;
	void Start () {
		lineRend = gameObject.GetComponent<LineRenderer>();

		RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(width, width), transform.rotation.eulerAngles.z, new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z*Mathf.Deg2Rad), Mathf.Sin(transform.rotation.eulerAngles.z*Mathf.Deg2Rad)), 25f, 1<<8, -10f, 10f);
		if(hit.collider != null){
			Enemy e = hit.collider.gameObject.GetComponent<Enemy>();
			if(e != null){
				e.TakeDamage(damage);
			}
			endPos = hit.point;
			lineRend.SetPositions(new Vector3[2]{transform.position, endPos});
		} else{
			Vector3 end = new Vector3(Mathf.Cos(transform.rotation.eulerAngles.z*Mathf.Deg2Rad), Mathf.Sin(transform.rotation.eulerAngles.z*Mathf.Deg2Rad),0f);
			endPos = transform.position + 25f*end.normalized;
			lineRend.SetPositions(new Vector3[2]{transform.position, endPos});
		}
		lineRend.SetWidth(width,width);
	}

	void Update () {
		lineRend.SetColors(new Color(0.5f,0.8f,1f,(lifeTime-count)/lifeTime), new Color(0.5f,0.8f,1f,(lifeTime-count)/lifeTime));

		lineRend.SetPositions(new Vector3[2]{transform.position + (endPos - transform.position)*count/lifeTime, endPos});
		lineRend.SetWidth(width*(lifeTime-count)/lifeTime, width*(lifeTime-count)/lifeTime);
		if(count > lifeTime) Destroy(gameObject);
		count += Time.deltaTime;
	}
}
