using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	void Start () {
	
	}

	void Update () {
		controllerPlayer ();
	}
		
	private void controllerPlayer() {
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		MovePlayer (h, v); // 力学による移動
	}

	public void MovePlayer (float h, float v) {
		Vector3 velocity = new Vector3 (h, 0, v);
		velocity = transform.TransformDirection (velocity) * 20f;
		GetComponent<Rigidbody> ().AddForce (velocity);
		transform.Rotate (0, h * 2.0f, 0);
	}
				
	void OnCollisionEnter (Collision collision) {
			if (collision.gameObject.tag == "Star") {
			var script = collision.gameObject.GetComponent<StarMove> ();
			if (script != null) {
				//Debug.Log ("count");
				Hud.instance.CountStar (1);
				script.GetStar ();
			}
		}
	}
}
