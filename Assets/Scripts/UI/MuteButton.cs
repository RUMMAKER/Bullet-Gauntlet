using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour {

	public Sprite unMuted;
	public Sprite muted;
	private Image selfImage;

	[System.Serializable]
	public enum muteType {
		MUSIC, SOUND
	}

	public muteType t;
	void Awake() {
		selfImage = GetComponent<Image>();
	}
	void Start () {
		GetComponent<Button>().onClick.AddListener(() => { Mute();});
		if(t == muteType.MUSIC){
			if(SoundControl.instance.IsMusicMute()){
				selfImage.sprite = muted;
			} else {
				selfImage.sprite = unMuted;
			}
		} else {
			if(SoundControl.instance.IsSoundMute()){
				selfImage.sprite = muted;
			} else {
				selfImage.sprite = unMuted;
			}
		}
	}

	public void Mute(){
		if(t == muteType.MUSIC){
			SoundControl.instance.MuteMusic();
			if(SoundControl.instance.IsMusicMute()){
				selfImage.sprite = muted;
			} else {
				selfImage.sprite = unMuted;
			}
		} else {
			SoundControl.instance.MuteSound();
			if(SoundControl.instance.IsSoundMute()){
				selfImage.sprite = muted;
			} else {
				selfImage.sprite = unMuted;
			}
		}
	}
	
}
