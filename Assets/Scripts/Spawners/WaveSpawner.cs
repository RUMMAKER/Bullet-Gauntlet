using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour {
	public int level;
	private float screenVertical;
	private float screenHorizontal;
	
	private int waveIndex = 0;
	private float timeBetweenWaves = 5f;
	private float waveTimer = 0f;

	private bool paused;
	private WaveType currentWaveType;
	private List<Transform> currentWaveEnemies = new List<Transform>();

	[System.Serializable]
	public enum Side {
		LEFT, UP, RIGHT, CENTER, FARUP
	}

	[System.Serializable]
	public enum WaveType {
		TIME, WAIT, HYBRID
	}

	[System.Serializable]
	public class Wave {
		public WaveType waveType;//if wait, will not end wave until all enemy in wave has been killed
		public float waveTime;
		public List<SpawnInfo> enemyList = new List<SpawnInfo>();
	}

	[System.Serializable]
	public class SpawnInfo {
		public Transform enemy;
		public Side spawnDirection;
	}

	public Wave[] waves;

	void Start () {
		screenVertical = Camera.main.orthographicSize * 2.0f;
		screenHorizontal = screenVertical * Screen.width / Screen.height;
		gameObject.SetActive(false);
	}
	
	void Update () {
		if(paused){
			if(currentWaveType == WaveType.WAIT){
				paused = false;
				foreach(Transform tr in currentWaveEnemies){
					if(tr != null){
						paused = true;
						return;
					}
				}
				currentWaveEnemies.Clear();
			} else { //HYBRID
				waveTimer += Time.deltaTime;
				if(waveTimer >= timeBetweenWaves){
					spawnNextWave();
					return;
				}
				paused = false;
				foreach(Transform tr in currentWaveEnemies){
					if(tr != null){
						paused = true;
						return;
					}
				}
				currentWaveEnemies.Clear();
				spawnNextWave();
				return;
			}
		}
		else if(waveTimer >= timeBetweenWaves){
			spawnNextWave();
		}
		waveTimer += Time.deltaTime;
	}

	private void Win(){
		KongregateAPIBehaviour.instance.SubmitStat("Score"+level, ScoreManager.instance.GetScore());
		KongregateAPIBehaviour.instance.SubmitStat("DMG"+level, ScoreManager.instance.GetDamageTaken());
		KongregateAPIBehaviour.instance.SubmitStat("Time"+level, ScoreManager.instance.GetTime());
		if(Player.instance.hardMode){
			KongregateAPIBehaviour.instance.SubmitStat("Hard"+level, 1);//1 means user did hardmode for this lvl
		}

		SoundControl.instance.StopMusic();
		Player.instance.SetLevel(level + 1);
		GameObject winLable = GameObject.Find("GameFinishButtons").transform.Find("WinLable").gameObject;
		winLable.SetActive(true);
		gameObject.SetActive(false);
	}

	private void spawnNextWave(){
		if(waveIndex >= waves.Length) {
			Win();
			return;
		}
		waveTimer = 0;
		spawnWave(waves[waveIndex]);
		waveIndex ++;
	}

	private void spawnWave(Wave w){
		currentWaveType = w.waveType;
		if(currentWaveType == WaveType.WAIT || currentWaveType == WaveType.HYBRID){
			paused = true;
		} else {
			paused = false;
		}
		timeBetweenWaves = w.waveTime;
		foreach(SpawnInfo info in w.enemyList){
			spawnEnemy(info);
		}
		if(paused){
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			foreach(GameObject obj in enemies){
				currentWaveEnemies.Add(obj.transform);
			}
		}
	}

	private void spawnEnemy(SpawnInfo info){
		switch(info.spawnDirection){
			case Side.LEFT: 
				GameObject obj1 = spawnEnemyAt(info.enemy, new Vector2(-screenHorizontal*0.5f - 0.6f, Random.Range(0f, screenVertical/2f)));
				obj1.transform.rotation = Quaternion.Euler(0,0,0);
				break;
			case Side.UP: 
				GameObject obj2 = spawnEnemyAt(info.enemy, new Vector2(Random.Range(-screenHorizontal/2f, screenHorizontal/2f), screenVertical*0.5f + 0.6f));
				obj2.transform.rotation = Quaternion.Euler(0,0,-90);
				break;
			case Side.RIGHT: 
				GameObject obj3 = spawnEnemyAt(info.enemy, new Vector2(screenHorizontal*0.5f + 0.6f, Random.Range(0f, screenVertical/2f)));
				obj3.transform.rotation = Quaternion.Euler(0,0,180);
				break;
			case Side.CENTER://used for spawning in messages
				spawnEnemyAt(info.enemy, Vector2.zero);
				break;
			case Side.FARUP://used for certain bosses too big and need to spawn further
				GameObject obj4 = spawnEnemyAt(info.enemy, new Vector2(Random.Range(-screenHorizontal/2f, screenHorizontal/2f), screenVertical*0.5f + 2f));
				obj4.transform.rotation = Quaternion.Euler(0,0,-90);
				break;
		}
	}

	private GameObject spawnEnemyAt(Transform enemy, Vector2 pos){
		GameObject obj = Instantiate(enemy.gameObject);
		obj.transform.position = pos;
		return obj;
	}

	public int GetTime(){
		float t = 0f;
		foreach(Wave w in waves){
			t += w.waveTime;
		}
		return (int)t;
	}
}
