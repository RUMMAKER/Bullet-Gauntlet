using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelChangeScript : MonoBehaviour {

	public string levelName;
	virtual protected void Awake() {
		GetComponent<Button>().onClick.AddListener(() => { StartCoroutine("ChangeLevelAfterDelay");});
	}

	private IEnumerator ChangeLevelAfterDelay()
	{
		GameObject fade = GameObject.Find("Canvas").transform.Find("FadeInPanel").gameObject;
		fade.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene(levelName, LoadSceneMode.Single);
	}
}
