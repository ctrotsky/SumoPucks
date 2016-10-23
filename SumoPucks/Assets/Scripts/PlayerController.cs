using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {

	Rigidbody2D rb;
	Animator anim;
	Animator charAnim;
	HUDController hudController;

	private float flickPower;
	private float remainingFlickCooldown;
	private bool alive;
	private bool stunned;

	public float maxFlickPower = 500;
	public float friction = 1;
	public float chargeSpeed = 10;
	public float flickCooldown = 50;
	public float respawnDelay = 3;
	public int playerNum;

	public GameObject aimer;
	public GameObject flickCharge;

    public Powerups powerUps;
    public Stats stats;
    public GameObject map;

    public int lives = 3;

    public PowerType powerType;

    // Use this for initialization
    void Start () {
        powerUps = GetComponent<Powerups>();
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		charAnim = transform.Find("Character").GetComponent<Animator>();
		//hudController = GameObject.FindGameObjectWithTag("HUDCanvas").GetComponent<HUDController>();
		flickPower = 0;
		remainingFlickCooldown = 0;
		rb.drag = friction;
		alive = true;
	}

	void UpdateStats(){
		rb.drag = friction;
	}

	// Update is called once per frame
	void Update () {
		Vector2 aim = getAim();

		if (alive && !stunned){
			if (remainingFlickCooldown <= 0){
				if (Input.GetButton("Player" + playerNum + "A")){
					ChargeFlick();
				}
				if (Input.GetButtonUp("Player" + playerNum + "A")){
					Flick(aim);
				}
			}
			if (Input.GetButtonDown("Player" + playerNum + "X")){
				powerUps.UseHammer();
			}
			if (Input.GetButtonDown("Player" + playerNum + "B")){
				powerUps.UseSpikes();
			}
		}

		ShowFlickAim(aim);
		Cooldown();
		UpdateStats();
	}

	void Cooldown(){
		if (remainingFlickCooldown >= 0){
			remainingFlickCooldown--;
		}
	}

	void ShowFlickAim(Vector2 aim){
		var angle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg + 90; //added degrees at end will change depending on which way sprite faces
 		aimer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
 		aimer.transform.localPosition = (aim.normalized * -0.6f) - new Vector2(-0.01f, 0.25f);
	}

	void ChargeFlick() {
		if (flickPower < maxFlickPower){
			flickPower+= chargeSpeed;
		}
		AnimateCharge();
	}

	void Flick (Vector2 aim) {
		rb.AddForce(aim * flickPower * -1);
		flickPower = 0;
		remainingFlickCooldown = flickCooldown;
		flickCharge.transform.localScale = new Vector3(0.1f,0.1f,0f);
	}

	Vector2 getAim () {
		float h = Input.GetAxis("Player" + playerNum + "Horizontal");
		float v = Input.GetAxis("Player" + playerNum + "Vertical");

		Vector2 aim = new Vector2(h, v);
		return aim;
	}

	void AnimateCharge(){
		float increment = transform.localScale.x / (maxFlickPower/chargeSpeed);
		if (flickCharge.transform.localScale.x < transform.localScale.x){
			flickCharge.transform.localScale += new Vector3(increment, increment, 0);
		}
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Item")
        {
            switch (col.gameObject.GetComponent<Pickup>().getPower())
            {
                case PowerType.SPIKE:
                    powerUps.addSpike();
                    print("gained spike");
                    break;
                case PowerType.JUMP:
                    powerUps.addJump();
                    print("gained jump");
                    break;
                case PowerType.HAMMER:
                    powerUps.addHammer();
                    print("gained hammer");
                    break;
                case PowerType.SAVE:
                    powerUps.addSave();
                    print("gained save");
                    break;
            }
            Destroy(col.gameObject);
        } else if (col.gameObject.tag == "Stun"){
        	StartCoroutine(Stun(2));

        }
    }

	void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Floor" && alive)
        {
			alive = false;
            StartCoroutine(AnimateFall());
        }
    }

    IEnumerator Stun(float time){
    	stunned = true;
    	charAnim.SetBool("Stunned", true);
    	flickPower = 0;
    	yield return new WaitForSeconds(time);
    	stunned = false;
		charAnim.SetBool("Stunned", false);
    }

    IEnumerator AnimateFall(){
    	while (transform.localScale.x > 0){
    		yield return new WaitForEndOfFrame();
    		transform.localScale = transform.localScale - new Vector3(0.1f,0.1f,0.0f);
    	}
    	Die();
    }

    void Die(){
    	Debug.Log("Died, lives: " + lives);
    	if (lives >= 1){
			lives--;
			hudController.Lives(playerNum, lives);
    		StartCoroutine(Respawn());
    	}
    	else {
    		Debug.Log("Player " + playerNum + " is out of lives.");
    		Destroy(this);
    	}
    }

    IEnumerator Respawn(){
		//should make sure players don't spawn on top of each other
    	transform.position = map.transform.Find("Spawnpoints").GetChild(playerNum).position;
    	rb.velocity = new Vector2(0,0);
		yield return new WaitForSeconds(respawnDelay);
    	transform.localScale = new Vector3(1.0f,1.0f,0.0f);
    	alive = true;
    }
}
