using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartButton : MonoBehaviour {
	public GameObject spawn;

	void Start () {
		GetComponent<Button>().onClick.AddListener(() => {  
														spawn.SetActive(true);
														GameObject.Find("NextShip").SetActive(false);
														GameObject.Find("PrevShip").SetActive(false);
														GameObject.Find("BackButton").SetActive(false);
														if(GameObject.Find("ScoreManager") != null){GameObject.Find("ScoreManager").GetComponent<ScoreManager>().Begin();}
														SoundControl.instance.StartMusic();
														gameObject.SetActive(false);
														});
	}
}
