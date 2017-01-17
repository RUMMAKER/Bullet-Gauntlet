using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class Ship : MonoBehaviour {
	protected string primaryFire = "LMB";
	protected string secondaryFire = "RMB";
	protected string specialFire = "space";

	protected float maxHealth = 50;
	protected float maxEnergy = 50;
	protected float health = 50;
	protected float energy = 50;
	protected float energyRecoverRate = 1;
	protected float maxSpeed = 4f;
	protected float accelSpeed = 20f;
	protected float decelSpeed = 10f;
	protected float turnSpeed = 500f;
	protected float explosionSize = 2f;
	protected bool active = false;

	private float primaryFireTimer;
	protected float primaryFireCooldown;
	protected float secondaryFireTimer;//protected hack for Ship_1
	protected float secondaryFireCooldown;
	private float specialFireTimer;
	protected float specialFireCooldown;
	protected float primaryFireCost;
	protected float secondaryFireCost;
	protected float specialFireCost;

	private float screenVertical;
	private float screenHorizontal;
	private BarScript healthBar;
	private BarScript energyBar;
	private Vector2 velocity = Vector2.zero;

	protected float particleRate = 0.003f;
	protected GameObject emission;

	private float totalDamageTaken = 0f;//used by score
	private float sizeRad;//should be hitbox radius

	virtual protected void Start(){
		sizeRad = GetComponent<CircleCollider2D>().radius;
		screenVertical = Camera.main.orthographicSize * 2.0f;
		screenHorizontal = screenVertical * Screen.width / Screen.height;

		GameObject tempHp = GameObject.Find("HpBar");
		if(tempHp != null){
			healthBar = tempHp.GetComponent<BarScript>();
		}

		GameObject tempEnergy = GameObject.Find("EnergyBar");
		if(tempEnergy != null){
			energyBar = tempEnergy.GetComponent<BarScript>();
		}

		GameObject startButton = GameObject.Find("StartButton");
		if(startButton != null){
			startButton.GetComponent<Button>().onClick.AddListener(() => { StartCoroutine("StartShipAfterTime"); });
		}
	}

	private IEnumerator StartShipAfterTime()
	{
		StartCoroutine("StartTrail");
		yield return new WaitForSeconds(2f);
		active = true;
		GameObject hitbox = Instantiate(Resources.Load<GameObject>("Prefabs/Effects/Hitbox"));
		hitbox.transform.parent = transform;
		hitbox.transform.localPosition = Vector3.back;
		energyBar.Activate();
		healthBar.DisplayHealth(health/maxHealth);
		healthBar.Activate();
	}

	virtual protected void Update(){
		if(!active) return;

		PrimaryShoot();
		SecondaryShoot();
		SpecialShoot();
		TurnTowardsMouse();
		WASDMove();
		KeepInBounds();

		primaryFireTimer += Time.deltaTime;
		secondaryFireTimer += Time.deltaTime;
		specialFireTimer += Time.deltaTime;
		UseEnergy(-Time.deltaTime*energyRecoverRate);//recoverEnergy
	}

	virtual protected void PrimaryShoot(){
		if(energy < primaryFireCost) return;
		if(primaryFire == "LMB"){
			if (Input.GetMouseButton(0)){
				//if energy sufficient
				if(primaryFireTimer > primaryFireCooldown){
					Fire1();
					UseEnergy(primaryFireCost);
					primaryFireTimer = 0;
				}
			}
		} else if(primaryFire == "RMB"){
			if (Input.GetMouseButton(1)){
				//if energy sufficient
				if(primaryFireTimer > primaryFireCooldown){
					Fire1();
					UseEnergy(primaryFireCost);
					primaryFireTimer = 0;
				}
			}
		} else {
			if (Input.GetKey(primaryFire)){
				//if energy sufficient
				if(primaryFireTimer > primaryFireCooldown){
					Fire1();
					UseEnergy(primaryFireCost);
					primaryFireTimer = 0;
				}
			}
		}
	}

	virtual protected void SecondaryShoot(){
		if(energy < secondaryFireCost) return;
		if(secondaryFire == "LMB"){
			if (Input.GetMouseButton(0)){
				//if energy sufficient
				if(secondaryFireTimer > secondaryFireCooldown){
					Fire2();
					UseEnergy(secondaryFireCost);
					secondaryFireTimer = 0;
				}
			}
		} else if(secondaryFire == "RMB"){
			if (Input.GetMouseButton(1)){
				//if energy sufficient
				if(secondaryFireTimer > secondaryFireCooldown){
					Fire2();
					UseEnergy(secondaryFireCost);
					secondaryFireTimer = 0;
				}
			}
		} else {
			if (Input.GetKey(secondaryFire)){
				//if energy sufficient
				if(secondaryFireTimer > secondaryFireCooldown){
					Fire2();
					UseEnergy(secondaryFireCost);
					secondaryFireTimer = 0;
				}
			}
		}
	}

	virtual protected void SpecialShoot(){
		if(energy < specialFireCost) return;
		if(specialFire == "LMB"){
			if (Input.GetMouseButton(0)){
				//if energy sufficient
				if(specialFireTimer > specialFireCooldown){
					Fire3();
					UseEnergy(specialFireCost);
					specialFireTimer = 0;
				}
			}
		} else if(specialFire == "RMB"){
			if (Input.GetMouseButton(1)){
				//if energy sufficient
				if(specialFireTimer > specialFireCooldown){
					Fire3();
					UseEnergy(specialFireCost);
					specialFireTimer = 0;
				}
			}
		} else {
			if (Input.GetKey(specialFire)){
				//if energy sufficient
				if(specialFireTimer > specialFireCooldown){
					Fire3();
					UseEnergy(specialFireCost);
					specialFireTimer = 0;
				}
			}
		}
	}

	abstract protected void Fire1();
	abstract protected void Fire2();
	abstract protected void Fire3();

	protected Vector2 GetMousePos () {
		return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	protected void TurnTowardsMouse () {

		Vector2 mousePos = GetMousePos();
		float angle = transform.rotation.eulerAngles.z;

		float newAngle = Mathf.Atan2((mousePos.y - transform.position.y),(mousePos.x - transform.position.x))*Mathf.Rad2Deg;
		if (newAngle < 0) {
			newAngle += 360;
		}

		float turnDegree = newAngle - angle;//how far to turn to match newAngle
		float turnRate = turnSpeed * Time.deltaTime;
		float difference = Mathf.Abs(turnDegree);
		if(difference > 180f) difference = 360f - difference;

		if(turnRate > difference)//will over turn
		{
			turnRate = difference;
		}

		if(turnDegree < 180 && turnDegree > 0)
		{
			transform.Rotate(0,0,turnRate);
		}
		else if(turnDegree >= 180 && turnDegree < 360)
		{
			transform.Rotate(0,0,turnRate*-1);
		}
		else if(turnDegree < 0 && turnDegree >= -180)
		{
			transform.Rotate(0,0,turnRate*-1);
		}
		else if(turnDegree < 0 && turnDegree < -180)
		{
			transform.Rotate(0,0,turnRate);
		}
	}

	public void SetActive(bool active){
		this.active = active;
	}
	public void SetHealth(float health){
		this.health = health;
		if(this.health > maxHealth) this.health = maxHealth;
	}
	public void SetSpeed(float speed){
		maxSpeed = speed;
	}
	public void SetEnergy(float energy){
		this.energy = energy;
		if(this.energy > maxEnergy) this.energy = maxEnergy;
		if(this.energy < 0) this.energy = 0;
		if(energyBar != null){
			energyBar.DisplayHealth(energy/maxEnergy);
		}
	}
	public void UseEnergy(float amount){
		SetEnergy(energy - amount);
	}

	public bool IsActive(){
		return active;
	}
	public float GetHealth(){
		return health;
	}
	public float GetEnergy(){
		return energy;
	}
	public void TakeDamage(float dmg){
		SetHealth(health - dmg);
		totalDamageTaken += dmg;
		if(healthBar != null){
			healthBar.DisplayHealth(health/maxHealth);
			healthBar.StartShake();
			energyBar.StartShake();
		}
		if(health <= 0f){
			Die();
		}
	}
	public void Heal(float dmg){//heal doesnt shake healthbar
		SetHealth(health + dmg);
		healthBar.DisplayHealth(health/maxHealth);
	}

	protected void WASDMove () {
		Vector2 direction = new Vector2(0, 0);
		if (Input.GetKey("w")) {
			direction += Vector2.up;
		}
		if (Input.GetKey("s")) {
			direction += Vector2.down;
		}
		if (Input.GetKey("a")) {
			direction += Vector2.left;
		}
		if (Input.GetKey("d")) {
			direction += Vector2.right;
		}
		//move
		Accelerate(direction);
		transform.position = transform.position + Time.deltaTime*(Vector3)velocity;
		Decelerate();
	}

	private void Accelerate(Vector2 dir){
		velocity += dir.normalized*accelSpeed*Time.deltaTime;
		if(velocity.magnitude > maxSpeed){
			velocity = velocity.normalized*maxSpeed;
		}
	}
	private void Decelerate(){
		Vector2 decel = velocity.normalized*decelSpeed*Time.deltaTime;
		if(velocity.x > 0){
			velocity.x -= decel.x;
			if(velocity.x < 0) velocity.x = 0;
		}
		else{
			velocity.x -= decel.x;
			if(velocity.x > 0) velocity.x = 0;
		}

		if(velocity.y > 0){
			velocity.y -= decel.y;
			if(velocity.y < 0) velocity.y = 0;
		}
		else{
			velocity.y -= decel.y;
			if(velocity.y > 0) velocity.y = 0;
		}
	}

	protected void KeepInBounds(){
		if(transform.position.x > screenHorizontal/2f - sizeRad){
			transform.position = new Vector3(screenHorizontal/2f - sizeRad,transform.position.y,transform.position.z);
		} else if(transform.position.x < -screenHorizontal/2f + sizeRad){
			transform.position = new Vector3(-screenHorizontal/2f + sizeRad,transform.position.y,transform.position.z);
		} 
		if(transform.position.y > screenVertical/2f - sizeRad){
			transform.position = new Vector3(transform.position.x,screenVertical/2f - sizeRad,transform.position.z);
		} else if(transform.position.y < -screenVertical/2f + sizeRad){
			transform.position = new Vector3(transform.position.x,-screenVertical/2f + sizeRad,transform.position.z);
		}
	}

	virtual protected void Die(){
		SoundControl.instance.StopMusic();
		GameObject explosion = ParticleFactory.MakeExplosion(explosionSize);
		explosion.transform.position = transform.position;

		// brings up game over buttons
		GameObject re = GameObject.Find("GameFinishButtons").transform.Find("Retry").gameObject;
		GameObject spawner = GameObject.Find("Spawner");
		if(spawner != null){
			spawner.SetActive(false);
			re.SetActive(true);
		}
		//
		SoundControl.instance.StopWind();
		Destroy(gameObject);
	}

	private IEnumerator StartTrail()
	{
		for(;;){
			Particle();
			yield return new WaitForSeconds(particleRate);
		}
	}

	private void Particle(){
		Vector3 rot = transform.rotation.eulerAngles+new Vector3(0,0,180);
		GameObject particle = Instantiate(emission);
		particle.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z + Random.Range(-5f, 5f));
		particle.transform.parent = transform;
		particle.transform.localPosition = Vector3.zero;
		particle.transform.localPosition += Vector3.forward
										 + Vector3.up*Random.Range(-0.02f, 0.02f)
										 + Vector3.left*0.05f;
		particle.transform.parent = null;
	}

	public float GetTotalDamageTaken(){
		return totalDamageTaken;
	}
}
