using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinLable : MonoBehaviour {

	private Image selfImage;
	public GameObject[] enables;

	void Awake() {
		selfImage = GetComponent<Image>();
	}

	void OnEnable() {
		StartCoroutine("LerpEffect");
	}

	private IEnumerator LerpEffect()
	{
		for(float n = 0; n <= 1; n += 0.05f){
			selfImage.color = new Color(1,1,1,n);	
			transform.localPosition = Vector3.up*n*120f;
			yield return null;
		}
		foreach(GameObject g in enables){
			g.SetActive(true);
		}
	}
}
