using UnityEngine;
using System.Collections;

public class SoundControl : MonoBehaviour {
	public static SoundControl instance;

	public AudioClip[] explosionSound;
	public AudioClip menuSong;
	public AudioClip gameSong;
	private AudioSource wind;
	private AudioSource music;
	private AudioSource soundEffects;

	private bool musicMuted;
	private bool soundMuted;

	private float pitch;
	private float pitchChangeSpeed;
	private float volume;
	private float volumeChangeSpeed;

	private bool stopDynamic;

	void Awake(){
		if(instance == null){
			DontDestroyOnLoad(gameObject);
			instance = this;
		} else if(instance != this) {
			Destroy(gameObject);
		}

		AudioSource[] AudioSources = GetComponents<AudioSource>();
		wind = AudioSources[0];
		music = AudioSources[1];
		soundEffects = AudioSources[2];
	}

	void Start(){
		musicMuted = false;
		soundMuted = false;
		stopDynamic = false;

		pitch = 1f;
		pitchChangeSpeed = 1f;
		volume = 0.02f;
		volumeChangeSpeed = 0.03f;
		music.Play();

		wind.volume = 0.03f;
		music.volume = 0.05f;
		soundEffects.volume = 0.3f;
	}

	void Update(){
		DynamicWind();
	}

	public void MuteMusic(){
		musicMuted = !musicMuted;
		music.mute = musicMuted;
	}

	public void MuteSound(){
		soundMuted = !soundMuted;
		wind.mute = soundMuted;
		soundEffects.mute = soundMuted;
	}

	public void Reset(){
		//state soundControl sould be in at start of level
		StartWind();
		music.Stop();
		music.clip = gameSong;
	}

	public void MenuReset(){
		//state soundControl should be in at start of level-select and start screen
		wind.Stop();
		if(music.clip != menuSong){
			music.Stop();
			music.clip = menuSong;
			StartMusic();
		}
	}

	public void PlayExplodeSound(float loudness){
		float actualLoud = loudness*soundEffects.volume;
		if(soundEffects.volume != 0f && !soundMuted){
			if(loudness < 1.6f){
				soundEffects.PlayOneShot(explosionSound[Random.Range(0,3)], actualLoud);
			} else if(loudness < 2.5f){
				soundEffects.PlayOneShot(explosionSound[Random.Range(1,4)], actualLoud);
			} else {
				soundEffects.PlayOneShot(explosionSound[3], actualLoud);
			}
		}
	}

	public void StartMusic(){
		StartCoroutine("StartMusicLerp");
	}

	public void StopMusic(){
		StartCoroutine("StopEffect");
	}

	public void StartWind(){
		stopDynamic = false;
		wind.volume = 0f;
		wind.Play();
	}

	public void StopWind(){
		stopDynamic = true;
		StartCoroutine("StopEffectWind");
	}

	private IEnumerator StartMusicLerp()
	{
		music.volume = 0f;
		music.Play();
		for(int n = 0; n < 10; n ++){
			music.volume += (0.05f/10f);
			yield return null;
		}
	}

	private IEnumerator StopEffect()
	{
		for(int n = 0; n < 40; n ++){
			music.volume -= (0.05f/40f);
			yield return null;
		}
		music.Stop();
	}

	private IEnumerator StopEffectWind()
	{
		float init = wind.volume;
		for(int n = 0; n < 180; n ++){
			wind.volume -= (init/180f);
			yield return null;
		}
		wind.Stop();
	}

	public bool IsMusicMute(){
		return musicMuted;
	}
	public bool IsSoundMute(){
		return soundMuted;
	}

	public void PlaySound(AudioClip c){
		if(soundMuted)return;
		soundEffects.PlayOneShot(c,0.3f);
	}

	//dynamic wind effect////////////////////
	void DynamicWind () {
		if(soundMuted || stopDynamic) return;

		if(wind.pitch != pitch) {
			ChangePitch();
		} else {
			PickPitch();
		}

		if(wind.volume != volume) {
			ChangeVolume();
		} else {
			PickVolume();
		}
	}

	void PickPitch(){
		pitchChangeSpeed = Random.Range(0.2f,1f);
		pitch = Random.Range(0.5f,1f);
	}

	void ChangePitch(){
		float change = Time.deltaTime*pitchChangeSpeed;
		if(wind.pitch < pitch) {
			wind.pitch += change;
			if(wind.pitch > pitch) {
				wind.pitch = pitch;
			}
		} else {
			wind.pitch -= change;
			if(wind.pitch < pitch) {
				wind.pitch = pitch;
			}
		}
	}

	void PickVolume(){
		volumeChangeSpeed = Random.Range(0.03f,0.08f);
		volume = Random.Range(0.04f,0.08f);
	}

	void ChangeVolume(){
		float change = Time.deltaTime*volumeChangeSpeed;
		if(wind.volume < volume) {
			wind.volume += change;
			if(wind.volume > volume) {
				wind.volume = volume;
			}
		} else {
			wind.volume -= change;
			if(wind.volume < volume) {
				wind.volume = volume;
			}
		}
	}
}
