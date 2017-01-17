using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NextButton : MonoBehaviour {

	void Start () {
		GetComponent<Button>().onClick.AddListener(() => { SelectNextShip();});
		Player.instance.NextShip();
		Player.instance.PrevShip();
	}
	
	public void SelectNextShip(){
		Player.instance.NextShip();
	}
}