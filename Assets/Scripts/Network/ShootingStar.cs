using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStar : MonoBehaviour {

	private GameObject target; //ターゲットを取得

	//public GameObject explosion;	// 爆発エフェクト

	// Use this for initialization
	void Start () {

		target = null;
	}
	
	// Update is called once per frame
	void Update () {

		target = FindClosestOtherPlayer ();

		if (target != null) {
			// スムーズにターゲットの方向を向く
			Quaternion targetRotation = Quaternion.LookRotation (target.transform.position - transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * 5);

			// フレーム毎に向く
			//transform.LookAt (target.transform);
		}

		// 前進させる
		transform.position += transform.forward * Time.deltaTime * 100;
	}

	GameObject FindClosestOtherPlayer () {

		GameObject[] players;
		players = GameObject.FindGameObjectsWithTag ("OtherPlayer");
		GameObject closest = null;
		float distance = Mathf.Infinity;

		foreach (GameObject player in players) {
			// ターゲットとの距離を取得
			Vector3 diff = player.transform.position - transform.position;
			float curDistance = diff.sqrMagnitude;

			if (curDistance < distance) {
				closest = player;
				distance = curDistance;
			}
		}

		if (closest != null) {
			if (Vector3.Distance (closest.transform.position, transform.position) > 100)
				closest = null;
		}

		return closest;
	}
}
