using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour {

	public float scrollSpeed;
	private Vector2 savedOffset;
	void Start () {
		savedOffset = GetComponent<Renderer>().sharedMaterial.GetTextureOffset("_MainTex");
	}
	
	void Update () {
		float y = Mathf.Repeat(Time.time * scrollSpeed, 1f);
		Vector2 offset = new Vector2(GetComponent<Renderer>().sharedMaterial.GetTextureOffset("_MainTex").x, y+savedOffset.y);
		GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
	}

	void OnDisable(){
		GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
	}
}
