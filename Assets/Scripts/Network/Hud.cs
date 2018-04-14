using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour {

	public static Hud instance { get; private set; }

	public Text[] playerNames;
	public Text score;
	public Text gameTime;
	public Image timeUp;
	public Image countDownImage;
	public Sprite[] countDownSprite;

	private int totalScore = 0;

	public void Init () {
		instance = this;

		timeUp.gameObject.SetActive (false);
		countDownImage.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("totalScore : " + totalScore);
	}

	public void SetCountDown (int remainingTime) {
		int cdNum = remainingTime - 300;	// ゲーム開始前のカウントダウン
		countDownImage.sprite = countDownSprite [cdNum < 0 ? 0 : cdNum];
		if (cdNum > 0) {
			if (countDownImage.gameObject.activeSelf == false)
				countDownImage.gameObject.SetActive (true);
		} else if (cdNum == 0) {
			countDownImage.rectTransform.sizeDelta = new Vector2 (300, 100);
		} else {
			if (countDownImage.gameObject.activeSelf == true)
				countDownImage.gameObject.SetActive (false);
		}
	}

	public void SetTime (int remainingTime) {
		if (remainingTime <= 0f) {
			timeUp.gameObject.SetActive (true);
			gameTime.text = "0";
		} else {
			gameTime.text = ((int)remainingTime).ToString ();
		}
	}

	public void CountStar (int n) {
		Debug.Log (n);
		totalScore += n;
		score.text = "Score : " + totalScore;
	}
}
