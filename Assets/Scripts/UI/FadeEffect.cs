using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour {

	private Image selfImage;
	private bool fade;//true = fade in, false = fade out
	void Awake() {
		fade = true;
		selfImage = GetComponent<Image>();
	}

	void OnEnable() {
		if(fade) {
			StartCoroutine("FadeIn");
		} else {
			StartCoroutine("FadeOut");
		}
	}

	private IEnumerator FadeIn()
	{
		for(float n = 1; n >= 0; n -= 0.05f){
			selfImage.color = new Color(0,0,0,n);
			yield return null;
		}
		fade = false;
		gameObject.SetActive(false);
	}

	private IEnumerator FadeOut()
	{
		for(float n = 0; n <= 1; n += 0.05f){
			selfImage.color = new Color(0,0,0,n);
			yield return null;
		}
	}
}
