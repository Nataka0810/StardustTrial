using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーが他オブジェクトを取得する際のスクリプト
public class PlayerTriggerArea : MonoBehaviour {

	// PlayerTriggerAreaにオブジェクトが触れたとき
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Data") {
			//Debug.Log ("count");
			Hud.instance.CountStar (1);
			Destroy (other.gameObject);
		}
	}
}
