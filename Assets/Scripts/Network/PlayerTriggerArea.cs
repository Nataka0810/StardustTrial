using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーが他オブジェクトを取得する際のスクリプト
public class PlayerTriggerArea : MonoBehaviour {
	
	ItemHolder itemHolder = null;
	Hud hud = null;

	void Start () {
		itemHolder = gameObject.transform.parent.GetComponent<ItemHolder> ();	// ItemHolderスクリプトを取得
	}

	void Update () {
		if (hud == null) {
			hud = GameObject.Find ("BattleManager").GetComponent<Hud> ();
		}
	}

	// PlayerTriggerAreaにデータが触れたとき
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Data") {
			Debug.Log ("count" + this.transform.parent.name);
			//Hud.instance.CountScore (1);
			hud.CountScore(1);
			other.gameObject.GetComponent<Destroy> ().CmdDestoryNetworkObject ();
		}

		if (other.tag == "ItemBox") {
			if (itemHolder.flag == false) {	// アイテムを所持してない場合
				itemHolder.GetItem ();	// アイテムを取得
				//Debug.Log ("アイテムを取得");
			}
		}

		if (other.tag == "Fire") {
			itemHolder.CmdHit ();
			hud.ReduceScore (5);
			//Debug.Log ("ダメージ発生(火の玉)");
			Destroy (other.gameObject);
		}
	}
}
