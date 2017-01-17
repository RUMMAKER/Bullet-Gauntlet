using UnityEngine;
using System.Collections;

public class EnemyLooperRed : EnemyLooper {

	GameObject bigBulletPrefab;
	override protected void Start () {
		bigBulletPrefab = Resources.Load<GameObject>("Prefabs/EnemyProjectiles/EnemyLargeBullet");
		base.Start();
		shootRate = 1.5f;
	}
	
	override protected void Update () {
		base.Update();
	}

	override protected IEnumerator StartChaserProduction()
	{
		yield return new WaitForSeconds(0.5f);
		for(;;){
			MakeBigBullets();
			yield return new WaitForSeconds(0.2f);
			MakeBullets();
			yield return new WaitForSeconds(shootRate);
		}
	}

	private void MakeBigBullets(){
		for(int n = 0; n < 8; n ++){
			GameObject obj = Instantiate(bigBulletPrefab);
			obj.transform.parent = transform;
			obj.transform.localPosition = Vector3.back;
			obj.transform.localRotation = Quaternion.Euler(0,0,-3*45f + 45f*n);
			obj.transform.parent = null;
		}
	}

	private void MakeBullets(){
		for(int n = 0; n < 8; n ++){
			GameObject obj = Instantiate(bigBulletPrefab);
			obj.transform.parent = transform;
			obj.transform.localPosition = Vector3.back;
			obj.transform.localRotation = Quaternion.Euler(0,0,-3*45f + 45f*n + 22.5f);
			obj.transform.parent = null;
		}
	}
}
