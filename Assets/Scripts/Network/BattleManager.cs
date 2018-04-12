using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BattleManager : NetworkBehaviour {

	public static BattleManager instance;

	private const int timeLimit = 300;	// 時間制限
	private const int startCountDownTime = 3;	// スタートカウントダウン
	private Hud hud;	// UIのスクリプト管理
	public GameObject star;	// オブジェクト
	private int count;	// タイマー
	private static System.DateTime serverStartTime;	// サーバー開始時刻

	void Awake() {
		instance = this;	// ゲームマネージャのインスタンス生成
		count = timeLimit;	// カウントダウンを初期化
	}
		
	public enum STATE {
		NONE,	// 状態無し
		START,	// ゲーム開始
		BATTLE,	// 対戦中
		FINISH	// 対戦終了
	};

	// ゲーム状態管理
	public STATE state {
		get;
		set;
	}

	// サーバ起動時
	public override void OnStartServer () {
		serverStartTime = System.DateTime.Now;	// 現在の時刻を格納
	}

	// クライアント起動時
	public override void OnStartClient () {
		hud = GetComponent<Hud> ();	// GUIスクリプトを格納
		hud.Init ();	// GUIスクリプトを初期化
	}
		
	void Update () {
		// サーバーが起動しているとき
		if (isServer) {
			Count ();	// タイマー起動
			//Debug.Log ("Server");
		}

		// クライアントが起動しているとき
		if (isClient) {
			changeState ();	// ゲーム状態遷移
			eventManager ();	// イベント実行
			//Debug.Log ("Client");
		}

		// ローカルプレイヤーのみ
		if (isLocalPlayer) {
			//Debug.Log ("LocalPlayer");
		}
	}

	// ゲーム状態を遷移する関数
	void changeState () {
		// 時間毎にイベントが発生する
		if (count == 295) {
			state = STATE.START;	// ゲーム状態をゲーム開始にする
		} else {
			state = STATE.NONE;
		}
	}

	// イベントを管理する関数
	void eventManager () {
		// ゲーム状態により発生するイベントを管理
		switch (state) {
		case STATE.START:	// ゲーム開始状態の時
			CmdSpawnStar (star);	// オブジェクトを生成
			break;
		default:
			break;
		}
	}

	// タイマーのカウントを進める
	void Count () {
		if (isServer) {
			count = timeLimit + startCountDownTime - (int)((System.DateTime.Now - serverStartTime).TotalSeconds);
			RpcSetCount (count);
		}
	}

	[ClientRpc]
	void RpcSetCount (int remainingTime) {
		hud.SetTime (remainingTime);
	}

	[ClientRpc]
	void RpcSetCountDown (int remaingTime) {
		hud.SetCountDown (remaingTime - timeLimit);
	}

	[Command]
	void CmdSpawnStar (GameObject star) {
		GameObject obj = Instantiate<GameObject> (
			star,
			new Vector3 (Random.Range(-50, 50), 10, Random.Range(-50, 50)),
			Quaternion.Euler(new Vector3 (0, 0, 0))
		);
		NetworkServer.Spawn (obj);
	}
}