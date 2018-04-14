using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemHolder : NetworkBehaviour {

	public bool flag;	// アイテムを所持しているかどうか

	[SerializeField] GameObject[] items;	// アイテム格納

	private int itemNo;	// どのアイテムを所持しているか

	// Use this for initialization
	void Start () {
		flag = false;	// フラグを初期化
		itemNo = 0;		// アイテムの種類
	}
	
	// Update is called once per frame
	void Update () {
		if (flag == true) {
			if (Input.GetKey (KeyCode.Space)) {
				switch (itemNo) {
				case 0:
					CmdShootingStar ();
					Debug.Log ("アイテム0を使用");
					break;
				case 1:
					Debug.Log ("アイテム1を使用");
					break;
				default:
					Debug.Log ("itemNo変数のエラー");
					break;
				}

				flag = false;	// アイテム無し
			}
		}
	}

	public void GetItem () {
		flag = true;	// アイテム有り
		itemNo = Random.Range(0, items.Length);	// アイテムの種類をランダムで取得
		switch (itemNo) {
		case 0:
			Debug.Log ("アイテム0を取得");
			break;
		default:
			break;
		}
	}

	[Command]
	public void CmdShootingStar () {
		GameObject spawnObject = Instantiate<GameObject> (
			items[0],
			transform.position,
			transform.rotation
		);
		NetworkServer.Spawn (spawnObject);

		Destroy (spawnObject, 3.0f);
	}

	public void Barrier () {
		/*
		GameObject spawnObject = Instantiate<GameObject> (
			items[0],
			transform.position,
			Quaternion.Euler(new Vector3 (0, 0, 0))
		);
		*/
	}
}
