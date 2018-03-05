using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BattleManager : NetworkBehaviour {

	public static BattleManager instance;

	private const int timeLimit = 300;	// 時間制限
	private const int startCountDownTime = 3;	// スタートカウントダウン
	private Hud hud;	//UIのスクリプト
	public GameObject star;
	private int count;

	private static System.DateTime serverStartTime;	// サーバー開始時刻

	void Awake() {
		instance = this;
		count = timeLimit;
	}
		
	public enum STATE {
		NONE,
		START,
		BATTLE,
		FINISH
	};

	// ゲーム状態管理
	public STATE state {
		get;
		set;
	}

	public override void OnStartServer () {
		serverStartTime = System.DateTime.Now;
	}

	public override void OnStartClient () {
		hud = GetComponent<Hud> ();
		hud.Init ();
	}
		
	void Update () {
		if (isServer) {
			Count ();
			Debug.Log ("Server");
		}

		if (isClient) {
			changeState ();
			eventManager ();
			Debug.Log ("Client");
		}

		if (isLocalPlayer) {
			Debug.Log ("LocalPlayer");
		}
	}

	void changeState () {
		if (count == 295) {
			state = STATE.START;
		} else {
			state = STATE.NONE;
		}
	}

	void eventManager () {
		switch (state) {
		case STATE.START:
			CmdSpawnStar (star);
			break;
		default:
			break;
		}
	}

	void Count () {
		if (isServer) {
			count = timeLimit - (int)((System.DateTime.Now - serverStartTime).TotalSeconds);
			RpcSetCount (count);
		}
	}

	[ClientRpc]
	void RpcSetCount (int remainingTime) {
		hud.SetTime (remainingTime);
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