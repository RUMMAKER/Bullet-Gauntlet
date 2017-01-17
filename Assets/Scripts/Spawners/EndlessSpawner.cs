using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EndlessSpawner : MonoBehaviour {
	
	public Text scoreLable;
	public Text scoreDisplay;

	private List<Transform> currentWaveEnemies = new List<Transform>();
	private float screenVertical;
	private float screenHorizontal;
	[System.Serializable]
	public class Spawnable {
		public Transform enemy;
		public int cost;
	}

	public Spawnable[] spawns;
	private float waveTime;
	private int waveCost;
	private int currentCost;
	private float currentWaveTime;

	private bool start5peace;
	private int wavesSurvived;
	void Start () {
		screenVertical = Camera.main.orthographicSize * 2.0f;
		screenHorizontal = screenVertical * Screen.width / Screen.height;

		waveTime = 1f;
		currentWaveTime = -3f;
		waveCost = 1;
		currentCost = 1;
		start5peace = true;
		wavesSurvived = 0;
		gameObject.SetActive(false);
		
	}
	
	void Update () {
		currentWaveTime += Time.deltaTime;
		if(currentWaveTime >= waveTime || Cleared()){
			currentWaveEnemies.Clear();
			SpawnNextWave();
		}
	}

	private void SpawnNextWave(){
		start5peace = false;
		waveTime += 3f;
		if(waveTime > 60f){
			waveTime = 60f;
		}
		waveCost += 3;
		if(waveCost > 60){
			waveCost = 60;
		}
		currentCost = waveCost;
		currentWaveTime = 0f;

		while(currentCost > 0){
			MakeEnemy();
		}
		wavesSurvived ++;
	}

	private void MakeEnemy(){
		int maxIndex = Random.Range(0,spawns.Length);
		while(spawns[maxIndex].cost > currentCost && maxIndex > 0){
			maxIndex--;
		}
		int subtractCost = spawns[maxIndex].cost;
		if (subtractCost == 35) {//The too big ship
			SpecialSpawn(spawns[maxIndex].enemy);
			currentCost -= subtractCost;
		} else {
			SpawnEnemy(spawns[maxIndex].enemy);
			currentCost -= subtractCost;
		}
	}

	private void SpecialSpawn(Transform enemy){
		GameObject obj4 = SpawnEnemyAt(enemy, new Vector2(Random.Range(-screenHorizontal/2f, screenHorizontal/2f), screenVertical*0.5f + 2f));
		obj4.transform.rotation = Quaternion.Euler(0,0,-90);
	}

	private void SpawnEnemy(Transform enemy){
		switch(Random.Range(0,3)){
			case 0: 
				GameObject obj1 = SpawnEnemyAt(enemy, new Vector2(-screenHorizontal*0.5f - 0.6f, Random.Range(0f, screenVertical/2f)));
				obj1.transform.rotation = Quaternion.Euler(0,0,0);
				break;
			case 1: 
				GameObject obj2 = SpawnEnemyAt(enemy, new Vector2(Random.Range(-screenHorizontal/2f, screenHorizontal/2f), screenVertical*0.5f + 0.6f));
				obj2.transform.rotation = Quaternion.Euler(0,0,-90);
				break;
			case 2: 
				GameObject obj3 = SpawnEnemyAt(enemy, new Vector2(screenHorizontal*0.5f + 0.6f, Random.Range(0f, screenVertical/2f)));
				obj3.transform.rotation = Quaternion.Euler(0,0,180);
				break;
		}
	}

	private GameObject SpawnEnemyAt(Transform enemy, Vector2 pos){
		GameObject obj = Instantiate(enemy.gameObject);
		obj.transform.position = pos;
		currentWaveEnemies.Add(obj.transform);
		return obj;
	}

	private bool Cleared(){
		if(start5peace)return false;
		bool flag = true;
		foreach(Transform tr in currentWaveEnemies){
			if(tr != null){
				return false;
			}
		}
		return flag;
	}

	void OnDisable(){
		if(start5peace)return;
		scoreLable.gameObject.SetActive(true);
		scoreDisplay.gameObject.SetActive(true);
		scoreDisplay.text = ""+wavesSurvived;
		KongregateAPIBehaviour.instance.SubmitStat("Endless", wavesSurvived);
	}
}