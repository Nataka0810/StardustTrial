using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMove : MonoBehaviour {

	public int score = 1;	// スコア

	public int power = 10;	// 引力の大きさ

	public void DataAttract (GameObject player) {
		// 引力による引き寄せ (引数 Vector3 v)
		/*
		v = (v - transform.position) * power;
		GetComponent<Rigidbody> ().AddForce (v);
		*/

		// 座標変更による引き寄せ (引数 GameObject player)
		transform.LookAt(player.transform);
		transform.position += transform.forward * Time.deltaTime * 30;
	}
}
