using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemHolder : NetworkBehaviour {

	public bool flag = false;	// アイテムを所持しているかどうか

	[SerializeField] GameObject[] items;	// アイテム格納
	[SerializeField] GameObject hitEffect;	// ヒット時のエフェクト

	private int itemNo;	// どのアイテムを所持しているか

	private float time;	// タイマー
	private float limitTime; // アイテムの制限時間

	public enum STATE {
		NONE,	// アイテム無し
		FIREBALL,	// ファイアボール
	};

	// ゲーム状態管理
	public STATE state {
		get;
		set;
	}

	// Use this for initialization
	void Start () {
		flag = false;	// アイテム所持フラグを初期化
		itemNo = 0;		// アイテムの種類
		time = 0;		// タイマー初期化
		state = STATE.NONE;	// アイテム使用フラグを初期化
	}
	
	// Update is called once per frame
	void Update () {
		UseItem ();	// アイテム使用関数
		limitItemTime ();	// アイテム制限時間関数
	}

	// アイテム使用時
	private void UseItem () {
		if (flag == true) {	// アイテムを所持しているか
			if (Input.GetKey (KeyCode.Space)) {
				switch (itemNo) {
				case 0:
					CmdFireBall ();
					state = STATE.FIREBALL;	// ファイアボール使用中
					limitTime = 5.0f;	// 使用時間10秒
					Debug.Log ("アイテム0を使用");
					break;
				case 1:
					Debug.Log ("アイテム1を使用");
					break;
				default:
					Debug.Log ("itemNo変数のエラー");
					break;
				}
			}
		}
	}

	// アイテム制限時間管理
	private void limitItemTime () {
		if (state != STATE.NONE) {	// アイテム使用中かどうか
			time += Time.deltaTime;	// タイマー更新
			if (time > limitTime) {	// 制限時間を超えたら
				flag = false;		// アイテム所持フラグをfalseにする
				state = STATE.NONE;	// アイテム使用フラグをfalseにする
				time = 0;			// タイマーを0に初期化
			}
		}
	}


	// アイテムを入手時
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
		
	// アイテム0:ファイアボールを使用時
	[Command]
	public void CmdFireBall () {
		GameObject spawnObject = Instantiate<GameObject> (
			items[0],
			transform.position + transform.forward * 5.0f,
			transform.rotation
		);
		NetworkServer.Spawn (spawnObject);

		Destroy (spawnObject, 3.0f);
	}

	// 攻撃がヒット時
	[Command]
	public void CmdHit () {
		GameObject spawnObject = Instantiate<GameObject> (
			hitEffect,
			new Vector3(transform.position.x, transform.position.y, transform.position.z),
			transform.rotation
		);
		NetworkServer.Spawn (spawnObject);

		Destroy (spawnObject, 1.0f);
	}
}
