using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {

	Rigidbody2D rb;

	private float FlickPower;
	private float remainingFlickCooldown;
	private bool alive;

	public float maxFlickPower = 500;
	public float friction = 1;
	public float chargeSpeed = 10;
	public float flickCooldown = 50;
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
		FlickPower = 0;
		remainingFlickCooldown = 0;
		rb.drag = friction;
		alive = true;
	}

	// Update is called once per frame
	void Update () {
		Vector2 aim = getAim();

		if (alive){
			if (remainingFlickCooldown <= 0){
				if (Input.GetButton("Player" + playerNum + "A")){
					ChargeFlick();
				}
				if (Input.GetButtonUp("Player" + playerNum + "A")){
					Flick(aim);
				}
			}
		}

		ShowFlickAim(aim);
		Cooldown();
	}

	void Cooldown(){
		if (remainingFlickCooldown >= 0){
			remainingFlickCooldown--;
		}
	}

	void ShowFlickAim(Vector2 aim){
		var angle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg + 270; //added degrees at end will change depending on which way sprite faces
 		aimer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	void ChargeFlick() {
		if (FlickPower < maxFlickPower){
			FlickPower+= chargeSpeed;
		}
		AnimateCharge();
	}

	void Flick (Vector2 aim) {
		print("aim: " + aim);
		print("power: " + FlickPower);

		rb.AddForce(aim * FlickPower * -1);
		FlickPower = 0;
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
        }
    }

	void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Floor")
        {
            StartCoroutine(AnimateFall());
        }
    }

    IEnumerator AnimateFall(){
    	for (int i = 0; i < 10; i++)
    	{
    		yield return new WaitForEndOfFrame();
    		transform.localScale = transform.localScale - new Vector3(0.1f,0.1f,0.0f);
    	}
    	Die();
    }

    void Die(){
    	lives--;
    	alive = false;
    	StartCoroutine(Respawn());
    }

    IEnumerator Respawn(){
    	transform.position = map.transform.Find("Spawnpoints").GetChild(playerNum).position;
    	rb.velocity = new Vector2(0,0);
		yield return new WaitForSeconds(5);
    	transform.localScale = new Vector3(1.0f,1.0f,0.0f);
    	alive = true;
    }
}
