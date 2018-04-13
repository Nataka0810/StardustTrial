using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMove : MonoBehaviour {

	public int score = 1;	// スコア

	public int power = 10;	// 引力の大きさ

	public void DataAttract (Vector3 v) {
		v = (v - transform.position) * power;
		GetComponent<Rigidbody> ().AddForce (v);
	}
}
