using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeInText : MonoBehaviour {

	public Color textColor;
	private Text selfText;

	void Awake() {
		selfText = GetComponent<Text>();
	}

	void OnEnable() {
		StartCoroutine("LerpEffect");
	}

	private IEnumerator LerpEffect()
	{
		for(float n = 0; n <= 1; n += 0.05f){
			selfText.color = new Color(textColor.r,textColor.g,textColor.b,n);	
			yield return null;
		}
	}
}
