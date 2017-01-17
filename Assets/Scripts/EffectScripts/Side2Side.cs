using UnityEngine;
using System.Collections;

public class Side2Side : MonoBehaviour {

	public float speed;
	public int period;
	void Start () {
		StartCoroutine("LerpEffect");
	}
	
	private IEnumerator LerpEffect()
	{
		for(;;){
			for(int n = 0; n < period; n++){
				float edgeSlow = (float)n/((float)period/2f);
				if(n > period/2){
					edgeSlow = (float)(period - n)/((float)period/2f);
				}
				transform.Translate(Vector3.left*speed*Time.deltaTime*edgeSlow);
				yield return null;
			}
			for(int n = 0; n < period; n++){
				float edgeSlow = (float)n/((float)period/2f);
				if(n > period/2){
					edgeSlow = (float)(period - n)/((float)period/2f);
				}
				transform.Translate(Vector3.right*speed*Time.deltaTime*edgeSlow);
				yield return null;
			}
		}
	}
}
