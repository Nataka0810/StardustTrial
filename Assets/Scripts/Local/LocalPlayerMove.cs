using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerMove : MonoBehaviour {

	public float speed = 10;
	public float smooth = 1;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		MovePlayer (h, v);
	}

	public void MovePlayer (float h, float v) {
		Vector3 velocity = new Vector3 (0, 0, v);
		velocity = transform.TransformDirection (velocity) * speed;
		rb.AddForce (velocity);
		transform.Rotate (0, h * smooth, 0);
	}
}
