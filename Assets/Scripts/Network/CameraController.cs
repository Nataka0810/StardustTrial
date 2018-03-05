using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float smooth = 3f;		// カメラモーションのスムーズ化用変数
	public Transform standardPos;			// the usual position for the camera, specified by a transform in the game
	public GameObject playerCam;

	// スムーズに繋がない時（クイック切り替え）用のブーリアンフラグ
	bool bQuickSwitch = false;	//Change Camera Position Quickly

	void Start ()
	{
		// 各参照の初期化
		//standardPos = transform;

		//カメラをスタートする
		playerCam.transform.position = standardPos.position;	
		playerCam.transform.forward = standardPos.forward;
	}

	void FixedUpdate ()	// このカメラ切り替えはFixedUpdate()内でないと正常に動かない
	{
		setCameraPositionNormalView ();
	}

	void setCameraPositionNormalView ()
	{
		if (bQuickSwitch == false) {
			// the camera to standard position and direction
			playerCam.transform.position = Vector3.Lerp (playerCam.transform.position, standardPos.position, Time.fixedDeltaTime * smooth);	
			playerCam.transform.forward = Vector3.Lerp (playerCam.transform.forward, standardPos.forward, Time.fixedDeltaTime * smooth);
		} else {
			// the camera to standard position and direction / Quick Change
			playerCam.transform.position = standardPos.position;	
			playerCam.transform.forward = standardPos.forward;
			bQuickSwitch = false;
		}
	}
}
