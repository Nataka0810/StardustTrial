using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Destroy : NetworkBehaviour {

	// Use this for initialization
	[Command]
	public void CmdDestoryNetworkObject () {
		Debug.Log ("削除");
		NetworkServer.Destroy (gameObject);
	}
}
