using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Rigidbody2D rb;
	private float Flickpower;
	public float maxFlickPower = 50;
	public float friction = 1;
	public float chargeSpeed = 1;
	public float chargeCooldown; //unimplemented
	public int playerNum;
	public GameObject aimer;
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		Flickpower = 0;
		rb.drag = friction;
	}

	// Update is called once per frame
	void Update () {
		Vector2 aim = getAim();

		if (Input.GetButton("Player" + playerNum + "A")){
			ChargeFlick(aim);
		}
		if (Input.GetButtonUp("Player" + playerNum + "A")){
			Flick(aim);
		}
	}

	void ChargeFlick(Vector2 aim) {
		if (Flickpower < maxFlickPower){
			Flickpower+= chargeSpeed;
		}
		var angle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg + 270; //added degrees at end will change depending on which way sprite faces
 		aimer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	void Flick (Vector2 aim) {
		print("aim: " + aim);

		rb.AddForce(aim * Flickpower * -1);
		Flickpower = 0;
	}

	Vector2 getAim () {
		float h = Input.GetAxis("Player" + playerNum + "Horizontal");
		float v = Input.GetAxis("Player" + playerNum + "Vertical");

		Vector2 aim = new Vector2(h, v);
		return aim;
	}
}
