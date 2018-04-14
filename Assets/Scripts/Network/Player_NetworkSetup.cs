using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour {

	[SerializeField] Camera playerCam;
	[SerializeField] AudioListener audioListener;
	[SerializeField] GameObject camPos;
	[SerializeField] GameObject gravity;
	[SerializeField] GameObject playerTriggerArea;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			GameObject.Find ("Scene Camera").SetActive (false);

			GetComponent<PlayerMove> ().enabled = true;
			GetComponent<CameraController> ().enabled = true;
			GetComponent<ItemHolder> ().enabled = true;
			playerTriggerArea.GetComponent<PlayerTriggerArea> ().enabled = true;

			playerCam.enabled = true;
			audioListener.enabled = true;

			this.tag = "Player";
		} else {
			this.tag = "OtherPlayer";
		}
	}
}
