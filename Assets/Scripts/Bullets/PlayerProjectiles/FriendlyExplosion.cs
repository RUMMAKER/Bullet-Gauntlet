using UnityEngine;
using System.Collections;

public class FriendlyExplosion : MonoBehaviour {

	public float damage = 1f;
	// Use this for initialization
	void Start () {
		Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, 0.5f, 1<<8, -10f, 10f);
		foreach(Collider2D c in col){
			Enemy e = c.gameObject.GetComponent<Enemy>();
			if(e != null){
				e.TakeDamage(damage);
			}
		}
	}
}
