using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour {

	public static Hud instance { get; private set; }

	public Text[] playerNames;
	public Text score;
	public Text gameTime;
	public Text timeUp;
	public Text countDownChar;

	private int totalScore = 0;

	public void Init () {
		instance = this;

		timeUp.gameObject.SetActive (false);
		countDownChar.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("totalScore : " + totalScore);
	}

	public void SetCountDown (float t) {
		var active = (t > 0f);
		if (active != countDownChar.gameObject.activeSelf) {
			countDownChar.gameObject.SetActive (active);
		}
		var cdNum = (int)t;
		countDownChar.text = cdNum > 0 ? cdNum.ToString() : "GO!";
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
		totalScore += n;
		score.text = "Score : " + totalScore;
	}
}
