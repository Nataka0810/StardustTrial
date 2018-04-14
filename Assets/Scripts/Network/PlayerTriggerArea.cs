using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーが他オブジェクトを取得する際のスクリプト
public class PlayerTriggerArea : MonoBehaviour {

	ItemHolder itemHolder = null;

	void Start () {
		itemHolder = gameObject.transform.parent.GetComponent<ItemHolder> ();	// ItemHolderスクリプトを取得
	}

	// PlayerTriggerAreaにデータが触れたとき
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Data") {
			Debug.Log ("count" + this.transform.parent.name);
			Hud.instance.CountStar (1);
			Destroy (other.gameObject);
		}
			
		if (other.tag == "ItemBox") {
			if (itemHolder.flag == false) {	// アイテムを所持してない場合
				itemHolder.GetItem ();	// アイテムを取得
				Debug.Log ("アイテムを取得");
			}
		}
	}
}
