using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DockScript : MonoBehaviour {

	private bool active;
	private float accel = 1f;
	private float accelAccel = 20f;
	private float maxAccel = 10f;
	private float maxSpeed = 150f;
	private float speed = 0f;

	protected float particleRate = 0.01f;
	protected GameObject emission;
	// Use this for initialization
	void Start () {
		active = false;
		GameObject startButton = GameObject.Find("StartButton");
		if(startButton != null){
			startButton.GetComponent<Button>().onClick.AddListener(() => { StartGame(); });
		}
		emission = Resources.Load<GameObject>("Prefabs/Effects/Emissions/SmokeEmission_Dock");
		StartCoroutine("StartTrail");
	}
	
	// Update is called once per frame
	void Update () {
		if(!active) return;
		accel += accelAccel*Time.deltaTime;
		if(accel > maxAccel) accel = maxAccel;
		speed += accel*Time.deltaTime;
		if(speed > maxSpeed) speed = maxSpeed;
		transform.position = transform.position + Vector3.down*speed*Time.deltaTime;

		if(transform.position.y < -20) Destroy(gameObject);
	}

	private void StartGame(){
		active = true;
	}

	private IEnumerator StartTrail()
	{
		for(;;){
			Particle(Vector3.forward + Vector3.left*1.3f + Vector3.down*5f);
			Particle(Vector3.forward + Vector3.right*1.3f + Vector3.down*5f);
			Particle(Vector3.forward + Vector3.left*0.68f + Vector3.down*5.7f);
			Particle(Vector3.forward + Vector3.right*0.68f + Vector3.down*5.7f);
			Particle(Vector3.forward + Vector3.down*6.2f);
			yield return new WaitForSeconds(particleRate);
		}
	}

	private void Particle(Vector3 startPos){
		Vector3 rot = transform.rotation.eulerAngles+new Vector3(0,0,-90);
		GameObject particle = Instantiate(emission);
		particle.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z + Random.Range(-5f, 5f));
		particle.transform.parent = transform;
		particle.transform.localPosition = startPos;
		particle.transform.parent = null;
	}
}
