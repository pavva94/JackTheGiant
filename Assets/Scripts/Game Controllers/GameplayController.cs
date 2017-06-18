using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour {

	public static GameplayController instance;

	[SerializeField]
	private Text scoreText, coinText, lifeText, gameOverScoreText, gameOverCoinText;

	[SerializeField]
	private GameObject pausePanel, gameOverPanel;

	[SerializeField]
	private GameObject readyButton;

	// Use this for initialization
	void Awake () {
		MakeInstance ();
	}

	void Start () {
		Time.timeScale = 0f;
	}

	public void GameOverShowPanel(int finalScore, int finalCoinScore) {
		gameOverPanel.SetActive (true);
		gameOverCoinText.text = finalCoinScore.ToString ();
		gameOverScoreText.text = finalScore.ToString ();
		// faccio andare dopo tre secondi nel menu principale
		StartCoroutine (GameOverLoadMainMenu ());
	}

	IEnumerator GameOverLoadMainMenu() {
		yield return new WaitForSeconds (3f);
		SceneManager.LoadScene ("MainMenu");
	}

	public void PlayerDiedRestartTheGame() {
		StartCoroutine (PlayerDiedRestart ());
	} 

	IEnumerator PlayerDiedRestart() {
		yield return new WaitForSeconds (1f);
		// SceneManager.LoadScene ("Gameplay");
		SceneFader.instance.LoadLevel("Gameplay");
	}
	
	public void SetScore(int score) {
		scoreText.text = "x" + score;
	}

	public void SetCoinScore(int cointScore) {
		coinText.text = "x" + cointScore;
	}

	public void SetLifeScore(int lifeScore) {
		lifeText.text = "x" + lifeScore;
	}


	void MakeInstance () {
		if (instance == null)
			instance = this;
	}

	public void PauseTheGame() {
		Time.timeScale = 0f;
		pausePanel.SetActive (true);
	}

	public void ResumeGame() {
		Time.timeScale = 1f;
		pausePanel.SetActive (false);
	}

	public void QuitGame() {
		Time.timeScale = 1f;
		// SceneManager.LoadScene ("MainMenu");
		SceneFader.instance.LoadLevel("MainMenu");
	}

	public void StartTheGame () {
		Time.timeScale = 1f;
		readyButton.SetActive (false);
	}
}
