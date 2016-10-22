using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Rigidbody2D rb;

	private float FlickPower;
	private float remainingFlickCooldown;

	public float maxFlickPower = 500;
	public float friction = 1;
	public float chargeSpeed = 10;
	public float flickCooldown = 50;
	public int playerNum;
	public GameObject aimer;
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		FlickPower = 0;
		remainingFlickCooldown = 0;
		rb.drag = friction;
	}

	// Update is called once per frame
	void Update () {
		Vector2 aim = getAim();

		if (remainingFlickCooldown <= 0){
			if (Input.GetButton("Player" + playerNum + "A")){
				ChargeFlick();
			}
			if (Input.GetButtonUp("Player" + playerNum + "A")){
				Flick(aim);
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
	}

	void Flick (Vector2 aim) {
		print("aim: " + aim);

		rb.AddForce(aim * FlickPower * -1);
		FlickPower = 0;
		remainingFlickCooldown = flickCooldown;
	}

	Vector2 getAim () {
		float h = Input.GetAxis("Player" + playerNum + "Horizontal");
		float v = Input.GetAxis("Player" + playerNum + "Vertical");

		Vector2 aim = new Vector2(h, v);
		return aim;
	}
}
