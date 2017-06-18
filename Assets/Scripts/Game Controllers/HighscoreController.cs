using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighscoreController : MonoBehaviour {

	[SerializeField]
	private Text scoreText, coinText;

	// Use this for initialization
	void Start () {
		// faccio vedere lo score corretto in base alla difficoltà
		SetTheScoreBasedOnDifficulty ();
	}

	void SetScore(int score, int coinScore) {
		scoreText.text = score.ToString ();
		coinText.text = coinScore.ToString ();
	}

	void SetTheScoreBasedOnDifficulty() {
		if (GamePreferences.GetEasyDifficulty () == 1) {
			SetScore (GamePreferences.GetEasyDifficultyHighScore (), GamePreferences.GetEasyDifficultyCoinScore ());
		}

		if (GamePreferences.GetMediumDifficulty () == 1) {
			SetScore (GamePreferences.GetMediumDifficultyHighScore (), GamePreferences.GetMediumDifficultyCoinScore ());
		}

		if (GamePreferences.GetHardDifficulty () == 1) {
			SetScore (GamePreferences.GetHardDifficultyHighScore (), GamePreferences.GetHardDifficultyCoinScore ());
		}
	}
	
	public void GoBackToMenu() {
		SceneManager.LoadScene ("MainMenu");
	}
}
