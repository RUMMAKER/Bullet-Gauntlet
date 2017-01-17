using UnityEngine;
using System.Collections;

public class LevelSetter : MonoBehaviour {

	[System.Serializable]
	public enum levelType {
		MENU, LEVEL
	}

	public levelType t;

	void Start () {
		switch(t){
			case levelType.LEVEL: 
				SoundControl.instance.Reset();
				break;
			case levelType.MENU: 
				SoundControl.instance.MenuReset();
				break;
		}
	}
}
