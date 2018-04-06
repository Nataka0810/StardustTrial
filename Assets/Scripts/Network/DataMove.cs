using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataMove : NetworkBehaviour {

	public int score = 1;

	public int power = 10;

	void Update () {
		if (transform.position.y < -30) {
			Destroy (gameObject);
		}
	}	

	[ClientCallback]
	public void DataAttract (Vector3 v) {
		v = (v - transform.position) * power;
		CmdDataAttract (v);
	}

	[ClientCallback]
	public void GetData() {
		CmdDestroy ();
	}

	[Command]
	public void CmdDataAttract (Vector3 v) {
		GetComponent<Rigidbody> ().AddForce (v);
	}

	[Command]
	public void CmdDestroy () {
		Destroy (gameObject);
	}
}
