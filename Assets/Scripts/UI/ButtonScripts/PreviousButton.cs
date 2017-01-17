using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PreviousButton : MonoBehaviour {

	void Start () {
		GetComponent<Button>().onClick.AddListener(() => { SelectNextShip();});
	}

	public void SelectNextShip(){
		Player.instance.PrevShip();
	}
}