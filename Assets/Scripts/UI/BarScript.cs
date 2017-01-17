using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

	public float direction;//-1 = (move inner)left, 1 = right
	float width;//the length
	Image healthFill;

	private float shakeCounter;
	private bool shaking;
	private Vector3 truePosition;

	private float hpValue = 1f;
	private Image selfImage;
	private Image lableImage;
	void Start () {
		healthFill = transform.GetChild(0).GetComponent<Image>();
		selfImage = GetComponent<Image>();
		width = ((RectTransform)transform).sizeDelta.x;
		truePosition = transform.localPosition;
		lableImage = transform.parent.gameObject.GetComponent<Image>();
	}
	void Update() {
		if(shaking){
			Shake();
		}
	}
	public void DisplayHealth (float hp) {
		//hp *should* be between 0 and 1
		hp = Mathf.Clamp(hp, 0, 1);
		healthFill.transform.localPosition = Vector3.right*direction*(width*hp - width);
		hpValue = hp;
	}
	public void StartShake(){
		shaking = true;
	}
	private void Shake(){
		if(shakeCounter > 0.3f){
			shaking = false;
			shakeCounter = 0f;
			transform.localPosition = truePosition;
			return;
		}
		transform.localPosition = truePosition + Vector3.up*Random.Range(-7f,7f) + Vector3.left*Random.Range(-5f,5f);
		shakeCounter += Time.deltaTime;
	}

	public void Activate(){
		StartCoroutine("LerpEffect");
	}

	private IEnumerator LerpEffect()
	{
		for(float n = 0; n <= 1; n += 0.02f){
			selfImage.color = new Color(0,0,0,n*0.4f);
			lableImage.color = new Color(1,1,1,n);
			if(n <= hpValue){
				healthFill.transform.localPosition = Vector3.right*direction*(width*n - width);
			}
			yield return null;
		}
		DisplayHealth(hpValue);
	}
}
