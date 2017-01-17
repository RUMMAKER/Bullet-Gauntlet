using UnityEngine;
using System.Collections;

public class PlayerBigLazer : MonoBehaviour {

	public float turnSpeed;
	public float chargeTime;
	public float lifeTime;
	public Color chargeColor;
	public Color startColor;
	public Color endColor;
	public float chargeWidth;
	public float finalWidth;
	public float damage;
	public GameObject pulseEffectPrefab;
	
	private LineRenderer lineRenderer;
	private float count = 0f;
	private LineRenderer innerRenderer;
	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetPositions(new Vector3[2]{Vector3.zero, 25f*Vector3.right});
		lineRenderer.SetColors(chargeColor, chargeColor);
		lineRenderer.SetWidth(chargeWidth,chargeWidth);

		GameObject innerLine = Instantiate(Resources.Load<GameObject>("Prefabs/Effects/InnerLine"));
		innerRenderer = innerLine.GetComponent<LineRenderer>();
		innerRenderer.SetColors(Color.white, Color.white);
		innerRenderer.SetWidth(0f,0f);
		innerRenderer.SetPositions(new Vector3[2]{Vector3.zero, 25f*Vector3.right});
		innerLine.transform.parent = transform;
		innerLine.transform.localRotation = Quaternion.Euler(0,0,0);
		innerLine.transform.localPosition = Vector3.back;

		if(pulseEffectPrefab != null){
			GameObject pulse = Instantiate(pulseEffectPrefab);
			pulse.transform.parent = transform;
			pulse.transform.localPosition = Vector3.zero;
		}
	}
	
	void Update () {
		transform.Rotate(new Vector3(0,0,turnSpeed*Time.deltaTime));

		if(count < chargeTime){
			lineRenderer.SetColors(chargeColor, chargeColor);
			lineRenderer.SetWidth(chargeWidth,chargeWidth);
		} else {
			lineRenderer.SetColors(Color.Lerp(startColor, endColor, (count-chargeTime)/(lifeTime-chargeTime)), Color.Lerp(startColor, endColor, (count-chargeTime)/(lifeTime-chargeTime)));
			lineRenderer.SetWidth(Mathf.Lerp(chargeWidth, finalWidth, 10f*(count-chargeTime)/(lifeTime-chargeTime)),Mathf.Lerp(chargeWidth, finalWidth, 10f*(count-chargeTime)/(lifeTime-chargeTime)));
			
			innerRenderer.SetColors(Color.Lerp(startColor*1.3f, endColor, (count-chargeTime)/(lifeTime-chargeTime)), Color.Lerp(startColor*1.3f, endColor, (count-chargeTime)/(lifeTime-chargeTime)));
			innerRenderer.SetWidth(Mathf.Lerp(chargeWidth/2f, finalWidth/2f, 10f*(count-chargeTime)/(lifeTime-chargeTime)),Mathf.Lerp(chargeWidth/2f, finalWidth/2f, 10f*(count-chargeTime)/(lifeTime-chargeTime)));
			DamageEnemy();
		}

		if(count > lifeTime){
			Destroy(gameObject);
		}
		count += Time.deltaTime;
	}

	protected void DamageEnemy(){
		RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(finalWidth, finalWidth), transform.rotation.eulerAngles.z, new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z*Mathf.Deg2Rad), Mathf.Sin(transform.rotation.eulerAngles.z*Mathf.Deg2Rad)), 25f, 1<<8, -10f, 10f);
		foreach(RaycastHit2D hit in hits){
			if(hit.collider != null){
				Enemy e = hit.collider.gameObject.GetComponent<Enemy>();
				if(e != null){
					e.TakeDamage(damage*Time.deltaTime);
				}
			}
		}
	}
}
