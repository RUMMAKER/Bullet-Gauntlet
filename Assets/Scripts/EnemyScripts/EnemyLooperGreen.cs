using UnityEngine;
using System.Collections;

public class EnemyLooperGreen : EnemyLooper {

	GameObject rocketPrefab;
	override protected void Start () {
		rocketPrefab = Resources.Load<GameObject>("Prefabs/EnemyProjectiles/EnemyRocket");
		base.Start();
		shootRate = 2f;
	}
	
	override protected void Update () {
		base.Update();
	}

	override protected IEnumerator StartChaserProduction()
	{
		for(;;){
			MakeRockets();
			yield return new WaitForSeconds(0.2f);
			MakeRockets();
			yield return new WaitForSeconds(shootRate);
		}
	}

	private void MakeRockets(){

		GameObject obj = Instantiate(rocketPrefab);
		obj.transform.parent = transform;
		obj.transform.localPosition = Vector3.back;
		obj.transform.localRotation = Quaternion.Euler(0,0,-45f);
		obj.transform.parent = null;

		GameObject obj2 = Instantiate(rocketPrefab);
		obj2.transform.parent = transform;
		obj2.transform.localPosition = Vector3.back;
		obj2.transform.localRotation = Quaternion.Euler(0,0,45f);
		obj2.transform.parent = null;
	}
}
