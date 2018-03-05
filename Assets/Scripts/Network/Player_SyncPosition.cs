using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(channel=0, sendInterval=0.033f)]

public class Player_SyncPosition : NetworkBehaviour {

	// hook: SyncVar変数が変更された時、指定メソッドを実行するようサーバーから全クライアントへ命令を出す
	[SyncVar (hook = "SyncPositionValues")] // ホストから全クライアントに送られる
	private Vector3 syncPos;
	// Playerの現在位置
	[SerializeField] Transform myTransform;
	// Lerp: 2ベクトル間を補間する
	//[SerializeField] float lerpRate = 15;

	float lerpRate;
	float normalLerpRate = 15;
	float fasterLerpRate = 25;

	// 前フレームの最終位置
	private Vector3 lastPos;
	// threshold: 閾値、境目となる値
	// 0.5unitを越えなければ移動していないこととする
	private float threshold = 0.5f;

	// Position同期用のList
	private List<Vector3> syncPosList = new List<Vector3> ();

	// HistoricalLerpingメソッドを使う時はtrueにする
	// SerializeFieldなのでInspectorビューから変更可能
	[SerializeField] private bool useHistoricalLerping = false;

	// 2点間の距離を判定する時に使う
	private float closeEnough = 0.1f;

	void Start () {
		lerpRate = normalLerpRate;
	}

	void Update () {
		LerpPosition ();	// 2点間を補間する
	}

	void FixedUpdate () {
		TransmitPosition ();
	}

	//ポジション補間用メソッド
	void LerpPosition () {
		// 補間対象は相手プレイヤーのみ
		if (!isLocalPlayer) {
			// Lerp(from, to, 割合) from~toのベクトル間を補間する
			//myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);

			if (useHistoricalLerping) {
				// 前時代の補間メソッド
				HistoricalLerping ();
			} else {
				// 通常の補間メソッド
				OrdinaryLerping ();
			}

			//Debug.Log (Time.deltaTime.ToString());
		}
	}

	// クライアントからホストへ、Position情報を送る
	[Command]
	void CmdProvidePositionToServer (Vector3 pos) {
		// サーバー側が受け取る値
		syncPos = pos;

		// 自分がホストの時に毎フレーム呼ばれる
		// クライアントの数だけ倍増していく
		// 自分がクライアントの時は全く呼ばれない
		//Debug.Log("Command");
	}

	// クライアントのみ実行される
	[ClientCallback]
	// 位置情報を送るメソッド
	void TransmitPosition () {
		// 自プレイヤー且つ、現在位置と前フレームの最終位置との距離がthresholdより大きい時
		if (isLocalPlayer && Vector3.Distance (myTransform.position, lastPos) > threshold) {
			CmdProvidePositionToServer (myTransform.position);

			// 現在位置を最終位置として保存
			lastPos = myTransform.position;
		}
	}

	// クライアントのみ有効
	[Client]
	// hookで指定されたメソッド 全クライアントが実行
	void SyncPositionValues (Vector3 latestPos) {
		syncPos = latestPos;
		// ListにPosition追加
		syncPosList.Add (syncPos);
	}

	// 通常使われる補間メソッド
	void OrdinaryLerping () {
		// Lerp(from, to, 割合) from~toのベクトル間を補間する
		myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
	}

	// 過去使用されていた補間メソッド
	void HistoricalLerping () {
		// Listが1以上あったら
		if (syncPosList.Count > 0) {
			// 現在位置とListの0番目の位置との中間値を補間
			myTransform.position = Vector3.Lerp(myTransform.position, syncPosList[0], Time.deltaTime * lerpRate);
		
			// 2点間がcloseEnoughより小さくなった時
			if (Vector3.Distance (myTransform.position, syncPosList[0]) < closeEnough) {
				// Listの0番目を削除
				syncPosList.RemoveAt (0);
			}

			if (syncPosList.Count > 10) {
				lerpRate = fasterLerpRate;
			} else {
				lerpRate = normalLerpRate;
			}

			// syncPosList.Countが0に戻った時、同期が追いついたことを意味する
			Debug.Log (syncPosList.Count.ToString());
		}
	}
}
