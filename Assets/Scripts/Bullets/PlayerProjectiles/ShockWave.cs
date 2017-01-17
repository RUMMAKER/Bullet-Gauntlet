using UnityEngine;
using System.Collections;

public class ShockWave : MonoBehaviour {

	void Start () {
		Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, 10f, 1<<9, -10f, 10f);
		foreach(Collider2D c in col){
			Destroy(c.gameObject);
		}
	}
}
