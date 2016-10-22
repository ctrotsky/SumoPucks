using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Rigidbody2D rb;
	private float Flickpower;
	public float maxFlickPower = 50;
	public float friction = 1;
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		Flickpower = 0;
		rb.drag = friction;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire1")){
			ChargeFlick();
		}
		if (Input.GetButtonUp("Fire1")){
			Flick();
		}
	}

	void ChargeFlick() {
		Mathf.Clamp(Flickpower++, 0, maxFlickPower);
	}

	void Flick () {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		Vector2 aim = new Vector2(h, v).normalized * Flickpower;

		rb.AddForce(aim);
		Flickpower = 0;
	}
}
