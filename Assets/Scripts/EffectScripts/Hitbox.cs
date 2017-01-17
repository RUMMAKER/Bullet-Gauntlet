using UnityEngine;
using System.Collections;

public class Hitbox : MonoBehaviour {

	private SpriteRenderer rend;
	void Start () {
		rend = GetComponent<SpriteRenderer>();
		StartCoroutine("LerpEffect");
	}

	private IEnumerator LerpEffect()
	{
		for(float n = 0; n <= 0.58; n += 0.02f){
			rend.color = new Color(1,1,1,n);	
			yield return null;
		}
	}
}
