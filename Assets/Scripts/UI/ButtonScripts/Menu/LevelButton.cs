using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelButton : MonoBehaviour {

	public bool hard;
	public int sceneNumber;
	void Start () {
		int maxLevel = Player.instance.GetLevel();
		if(!hard){
			if(maxLevel == sceneNumber){
				GetComponent<Image>().color = new Color(1f,0.8f,0.1f,1f);
				GetComponent<Button>().onClick.AddListener(() => { StartCoroutine("ChangeLevelAfterDelay"); });
			} else if(maxLevel > sceneNumber){
				GetComponent<Image>().color = new Color(0.4f,0.7f,0.4f,1f);
				GetComponent<Button>().onClick.AddListener(() => { StartCoroutine("ChangeLevelAfterDelay"); });
			} else {
				GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f,1f);
			}
		} else {
			if(maxLevel > sceneNumber){
				if(Player.instance.GetHard(sceneNumber)){
					GetComponent<Image>().color = new Color(0.7f,0.25f,0.25f,1f);
				} else {
					GetComponent<Image>().color = new Color(1f,0.33f,0.33f,1f);
				}
				GetComponent<Button>().onClick.AddListener(() => { StartCoroutine("ChangeLevelAfterDelay"); });
			} else {
				GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f,1f);
			}
		}
	}

	private IEnumerator ChangeLevelAfterDelay()
	{
		SoundControl.instance.StopMusic();
		GameObject fade = GameObject.Find("Canvas").transform.Find("FadeInPanel").gameObject;
		fade.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		Player.instance.hardMode = hard;
		SceneManager.LoadScene("Level_"+sceneNumber,LoadSceneMode.Single);
	}
}
