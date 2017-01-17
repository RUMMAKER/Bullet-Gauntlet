using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {

	[System.Serializable]
	public enum scoreType {
		TIME, DAMAGE, SCORE
	}

	public scoreType t;
	private Text selfText;

	void Awake() {
		selfText = GetComponent<Text>();
	}

	void OnEnable() {
		switch(t){
			case scoreType.TIME: 
				int time = ScoreManager.instance.GetTime();
				int mins = time/60;
				int secs = time%60;
				string m = ""+mins;
				string s = ""+secs;
				if(mins < 10){
					m = "0"+mins;
				}
				if(secs < 10){
					s = "0"+secs;
				}
				selfText.text = "" + m + ":" + s;
				break;
			case scoreType.DAMAGE: 
				selfText.text = ""+ScoreManager.instance.GetDamageTaken();
				break;
			case scoreType.SCORE: 
				if(Player.instance.hardMode){
					selfText.text = "(HardMode X1.5) "+ScoreManager.instance.GetScore();
				} else {
					selfText.text = ""+ScoreManager.instance.GetScore();
				}
				break;
		}
	}
}
