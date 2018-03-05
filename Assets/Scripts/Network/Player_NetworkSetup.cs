using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour {

	[SerializeField] Camera playerCam;
	[SerializeField] AudioListener audioListener;
	[SerializeField] GameObject camPos;
	[SerializeField] GameObject gravity;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			GameObject.Find ("Scene Camera").SetActive (false);

			GetComponent<PlayerMove> ().enabled = true;
			GetComponent<CameraController> ().enabled = true;

			playerCam.enabled = true;
			audioListener.enabled = true;
		}
	}
}
