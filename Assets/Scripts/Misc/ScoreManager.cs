using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {
	public static ScoreManager instance;

	private int maxScore = 10000;
	private float startTime;
	private int levelTime;
	
	void Awake(){
		if(instance == null){
			DontDestroyOnLoad(gameObject);
			instance = this;
		} else if(instance != this) {
			Destroy(gameObject);
		}
	}

	public void Begin() {
		startTime = Time.timeSinceLevelLoad;
		levelTime = GameObject.Find("Spawner").GetComponent<WaveSpawner>().GetTime();
	}

	public int GetTime(){
		return (int)(Time.timeSinceLevelLoad - startTime);
	}

	public int GetDamageTaken(){
		return (int)(GameObject.FindWithTag("Player").GetComponent<Ship>().GetTotalDamageTaken());
	}

	public int GetScore(){
		int score = maxScore 
					+ (int)((levelTime-GetTime())*6f) 
					- (int)(100f*GameObject.FindWithTag("Player").GetComponent<Ship>().GetTotalDamageTaken());
		if(score < 0){
			score = 0;
		}
		if(Player.instance.hardMode){
			score = (int)((float)score * 1.5f);
		}
		return score;
	}
}
