using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		
		Vector3 aim = new Vector3(h, 0.0f, v);

	}

	void Fling () {
		
	}
}
