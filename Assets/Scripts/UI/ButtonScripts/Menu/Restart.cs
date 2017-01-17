using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {

	public GameObject lvl;
	private Image selfImage;
	void Awake() {
		selfImage = GetComponent<Image>();
		GetComponent<Button>().onClick.AddListener(() => { StartCoroutine("ChangeLevelAfterDelay");});

	}
	void OnEnable() {
		StartCoroutine("LerpEffect");
	}

	private IEnumerator LerpEffect()
	{
		for(float n = 0; n <= 1; n += 0.05f){
			selfImage.color = new Color(1,1,1,n);	
			transform.localPosition = Vector3.up*n*50f;
			yield return null;
		}
		lvl.SetActive(true);
	}

	private IEnumerator ChangeLevelAfterDelay()
	{
		GameObject fade = GameObject.Find("Canvas").transform.Find("FadeInPanel").gameObject;
		fade.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name,LoadSceneMode.Single);
	}
}
