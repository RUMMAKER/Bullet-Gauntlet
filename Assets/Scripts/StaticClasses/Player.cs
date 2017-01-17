using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public static Player instance;
	public bool hardMode;

	private GameObject ship;
	private int shipIndex = 0;
	private int shipTotal = 5;

	void Awake(){
		if(instance == null){
			DontDestroyOnLoad(gameObject);
			instance = this;
		} else if(instance != this) {
			Destroy(gameObject);
		}
		hardMode = false;
	}

	public void NextShip(){
		shipIndex++;
		if(shipIndex > shipTotal-1) shipIndex = 0;
		GameObject.Destroy(ship);
		ship = ShipFactory.MakePlayerShip(shipIndex);
		if(hardMode){
			StartCoroutine(ChangeHealthAfterDelay(ship.GetComponent<Ship>()));
		}
	}
	public void PrevShip(){
		shipIndex--;
		if(shipIndex < 0) shipIndex = shipTotal-1;
		GameObject.Destroy(ship);
		ship = ShipFactory.MakePlayerShip(shipIndex);
		if(hardMode){
			StartCoroutine(ChangeHealthAfterDelay(ship.GetComponent<Ship>()));
		}
	}

	private IEnumerator ChangeHealthAfterDelay(Ship s)
	{
		yield return null;
		s.SetHealth(1f);
	}

	//PlayerPrefs/saving
	private void Save(){
		PlayerPrefs.Save();
	}
	public void SetLevel(int l){
		if(hardMode){
			SetHard(l-1);
		}
		if(GetLevel() < l){
			PlayerPrefs.SetInt("lvl", l);
			Save();
		}
	}
	public void ForceSetLevel(int l){
		PlayerPrefs.SetInt("lvl", l);
		Save();
	}
	public int GetLevel(){
		if(!PlayerPrefs.HasKey("lvl")){
			PlayerPrefs.SetInt("lvl", 0);
			Save();
			return 0;
		} else {
			return PlayerPrefs.GetInt("lvl");
		}
	}

	public void SetHard(int l){
		PlayerPrefs.SetInt("hard"+l, 1);
		Save();
	}
	public bool GetHard(int l){
		if(!PlayerPrefs.HasKey("hard"+l)){
			PlayerPrefs.SetInt("hard"+l, 0);
			Save();
			return false;
		} else {
			return PlayerPrefs.GetInt("hard"+l) == 1;
		}
	}
}
