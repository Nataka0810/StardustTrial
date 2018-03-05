using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_SyncRotation : NetworkBehaviour {

	// SyncVar: ホストサーバーからクライアントへ送られる
	// Quaternion型 からfloat型に修正
	// hook: SyncVar変数が変更された時に、SyncVar変数を引数にして指定したメソッドを実行
	// プレイヤーの角度
	[SyncVar (hook = "OnPlayerRotSynced")] private float syncPlayerRotation;
	// FirstPersonCharacterのカメラの角度
	[SyncVar (hook = "OnCamRotSynced")] private float syncCamRotation;

	[SerializeField] private Transform playerTransform;
	[SerializeField] private Transform camTransform;

	// Serialize Fieldを削除
	private float lerpRate = 17;
	// 前フレームの最終角度
	// Quaternion型からfloat型(オイラー角。360度のやつ。)に変更
	private float lastPlayerRot;
	private float lastCamRot;
	// 閾値は5。5度以上動いた時のみメソッドを実行
	private float threshold = 1;
	// 角度保存用のList
	private List<float> syncPlayerRotList = new List<float> ();
	private List<float> syncCamRotList = new List<float> ();
	// HistoricalInterpolationで角度の判定
	private float closeEnough = 0.4f;
	// 前時代の角度同期メソッドを使うか
	[SerializeField] private bool useHistoricalInterpolation;


	void Update () {
		// 現在角度と取得した角度を補間する
		LerpRotations ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// クライアント側のPlayerの角度を取得
		TransmitRotations ();
	}

	// 角度を補間するメソッド
	void LerpRotations () {
		// 自プレイヤー以外のPlayerの時
		if (!isLocalPlayer) {
			// プレイヤーの角度とカメラの角度を補間
			//playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, 
			//	syncPlayerRotation, Time.deltaTime * lerpRate);
			//camTransform.rotation = Quaternion.Lerp (camTransform.rotation, 
			//	syncCamRotation, Time.deltaTime * lerpRate);

			if (useHistoricalInterpolation) {
				// 前時代の角度同期判定
				HistoricalInterpolation ();
			} else {
				// UNETを使用した角度同期判定
				OrdinaryLerping ();
			}
		}
	}

	// 前時代の角度同期判定
	void HistoricalInterpolation () {
		// Listが1つでもあったら
		if (syncPlayerRotList.Count > 0) {
			LerpPlayerRotation (syncPlayerRotList [0]);

			if (Mathf.Abs (playerTransform.localEulerAngles.y - syncPlayerRotList [0]) < closeEnough) {
				syncPlayerRotList.RemoveAt (0);
			}
			Debug.Log (syncPlayerRotList.Count.ToString () + "syncPlayerRotList Count");
		}

		if (syncCamRotList.Count > 0) {
			LerpCamRotation (syncCamRotList [0]);

			if (Mathf.Abs (camTransform.localEulerAngles.x - syncCamRotList [0]) < closeEnough) {
				syncCamRotList.RemoveAt (0);
			}
			Debug.Log (syncCamRotList.Count.ToString () + "syncCamRotList Count");
		}
	}

	void OrdinaryLerping () {
		LerpPlayerRotation (syncPlayerRotation);
		LerpCamRotation (syncCamRotation);
	}

	// プレイヤーの現在角度を補間
	void LerpPlayerRotation (float rotAngle) {
		// 引数のオイラー角を、Vector3の形で保存
		Vector3 playerNewRot = new Vector3 (0, rotAngle, 0);
		// Lerp: 現在の角度と受け取った角度の補間値を求める
		// Euler: オイラー角をQuaternion角に変換してくれる
		playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, Quaternion.Euler(playerNewRot), lerpRate * Time.deltaTime);
	}

	// カメラの現在角度を補間
	void LerpCamRotation (float rotAngle) {
		Vector3 camNewRot = new Vector3 (rotAngle, 0, 0);
		// カメラの子オブジェクトのため、親オブジェクトの向きを継承するlocalRotationを使う
		camTransform.localRotation = Quaternion.Lerp (camTransform.localRotation, Quaternion.Euler (camNewRot), lerpRate * Time.deltaTime);
	}

	// クライアントからホストへ送られる
	// 引数をfloat型(オイラー角)に修正
	[Command]
	void CmdProvideRotationsToServer (float playerRot, float camRot) {
		syncPlayerRotation = playerRot;
		syncCamRotation = camRot;

		Debug.Log("Command for angle");
	}

	// クライアント側だけが実行できるメソッド
	[Client]
	void TransmitRotations () {
		if (isLocalPlayer) {
			//if (Quaternion.Angle (playerTransform.rotation, lastPlayerRot) > threshold ||
			//    Quaternion.Angle (camTransform.rotation, lastCamRot) > threshold) {
			//}

			// localEulerAngles: Quaternion角をオイラー角(普通の360度)で回転量を表す
			if (CheckIfBeyondThreshold (playerTransform.localEulerAngles.y, lastPlayerRot) ||
			    CheckIfBeyondThreshold (camTransform.localEulerAngles.x, lastCamRot)) {
				// lastPlayerRotとlastCamRotを現在角度に更新
				lastPlayerRot = playerTransform.localEulerAngles.y;
				lastCamRot = camTransform.localEulerAngles.x;
				// 現在の角度に更新したlastPlayerRotとlastCamRotでメソッドを実行
				CmdProvideRotationsToServer (lastPlayerRot, lastCamRot);
			}
		}
	}

	// 現在角度と前フレームのオイラー角を比較し、threshold(1度)以上開きがあったらtrueを返す
	bool CheckIfBeyondThreshold (float rot1, float rot2) {
		// Mathf.Abs: 絶対値取得
		if (Mathf.Abs (rot1 - rot2) > threshold) {
			return true;
		} else {
			return false;
		}
	}

	// syncPlayerRotation変数が変更された時に実行(hook)
	// Clientのみ実行
	[Client]
	void OnPlayerRotSynced (float latestPlayerRot) {
		// hookは自分で同期する必要がある
		syncPlayerRotation = latestPlayerRot;
		// Listに登録
		syncPlayerRotList.Add (syncPlayerRotation);
	}

	// syncCamRotation変数が変更された時に実行(hook)
	// Clientのみ実行
	[Client]
	void OnCamRotSynced (float latestCamRot) {
		// hookは自分で同期する必要がある
		syncCamRotation = latestCamRot;
		// Listに登録
		syncCamRotList.Add (syncCamRotation);
	}
}
