using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StarMove : NetworkBehaviour {

	public int score = 1;

	public int power = 10;

	void Update () {
		if (transform.position.y < -30) {
			Destroy (gameObject);
		}
	}	

	[ClientCallback]
	public void StarAttract (Vector3 v) {
		v = (v - transform.position) * power;
		CmdStarAttract (v);
	}

	[ClientCallback]
	public void GetStar() {
		CmdDestroy ();
	}

	[Command]
	public void CmdStarAttract (Vector3 v) {
		GetComponent<Rigidbody> ().AddForce (v);
	}

	[Command]
	public void CmdDestroy () {
		Destroy (gameObject);
	}
}
