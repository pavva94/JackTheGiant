using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	[HideInInspector]
	public bool gameStartedFromMainMenu, gameRestartedAfterPlayerDied;

	[HideInInspector]
	public int score, coinScore, lifeScore;

	// Use this for initialization
	void Awake () {
		MakeSingleton ();
	}

	void Start() {
		InitializeVariables ();
	}

	// utilizzo OnEnable e OnDisable per delegare alla fine del loading della scena la funzione
	// in questo modo quando la scena e caricata eseguo la funzione OnSceneLoaded()
	void OnEnable() {
		//Tell our 'OnSceneLoaded' function to start listening for a scene change as soon as this script is enabled.
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable() {
		//Tell our 'OnSceneLoaded' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (SceneManager.GetActiveScene ().name == "Gameplay") {
			if (gameRestartedAfterPlayerDied) {

				GameplayController.instance.SetScore (score);
				GameplayController.instance.SetCoinScore (coinScore);
				GameplayController.instance.SetLifeScore (lifeScore);

				PlayerScore.scoreCount = score;
				PlayerScore.coinCount = coinScore;
				PlayerScore.lifeCount = lifeScore;
			} else if (gameStartedFromMainMenu) {
				GameplayController.instance.SetScore (0);
				GameplayController.instance.SetCoinScore (0);
				GameplayController.instance.SetLifeScore (2);

				PlayerScore.scoreCount = 0;
				PlayerScore.coinCount = 0;
				PlayerScore.lifeCount = 2;
			}
		}
	}

	void InitializeVariables() {
		// se è la prima volta che apriamo il gioco 
		// PlayerPref non ha nessuna chiave
		if (!PlayerPrefs.HasKey("Game Initialized")) {
			GamePreferences.SetEasyDifficulty(0);
			GamePreferences.SetEasyDifficultyHighScore (0);
			GamePreferences.SetEasyDifficultyCoinScore (0);

			GamePreferences.SetMediumDifficulty(1); // difficoltà base
			GamePreferences.SetMediumDifficultyHighScore (0);
			GamePreferences.SetMediumDifficultyCoinScore (0);

			GamePreferences.SetHardDifficulty(0);
			GamePreferences.SetHardDifficultyHighScore (0);
			GamePreferences.SetHardDifficultyCoinScore (0);

			GamePreferences.SetMusicState (0);

			// salvo questa chiave così non ci torno più qui
			PlayerPrefs.SetInt ("Game Initialized", 123);
		}
	}
	
	void MakeSingleton () {
		// così si crea un SINGLETON
		// questo gameObject viene chiamato ogni volta che si carica questa scne
		// quindi creerebbe ogni volta un nuovo gamemanager
		// la prima volta va bene e lo crea mettendolo anche come DontDestroy
		// la seconda volta crea un nuovo gameObject ma noi abbiamo già il primo 
		// quindi questo secondo oggetto viene distrutto
		// e così ogni volta che si carica la scena. 
		// in questo modo abbiamo sempre e solo il primo creato 
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			// viene passato ovunque nelle varie scene
			DontDestroyOnLoad (gameObject);
		}
	}

	public void CheckGameStatus(int score, int coinScore, int lifeScore) {

		// controlliamo se non ho più vite, allora gameover
		if (lifeScore < 0) {

			if (GamePreferences.GetEasyDifficulty () == 1) {
				int highScore = GamePreferences.GetEasyDifficultyHighScore ();
				int coinHighScore = GamePreferences.GetEasyDifficultyCoinScore ();

				if (highScore < score)
					GamePreferences.SetEasyDifficultyHighScore (score);
				
				if (coinHighScore < coinScore) {
					GamePreferences.SetEasyDifficultyCoinScore (coinScore);
				}
			}

			if (GamePreferences.GetMediumDifficulty () == 1) {
				int highScore = GamePreferences.GetMediumDifficultyHighScore ();
				int coinHighScore = GamePreferences.GetMediumDifficultyCoinScore ();

				if (highScore < score)
					GamePreferences.SetMediumDifficultyHighScore (score);

				if (coinHighScore < coinScore) {
					GamePreferences.SetMediumDifficultyCoinScore (coinScore);
				}
			}

			if (GamePreferences.GetHardDifficulty () == 1) {
				int highScore = GamePreferences.GetHardDifficultyHighScore ();
				int coinHighScore = GamePreferences.GetHardDifficultyCoinScore ();

				if (highScore < score)
					GamePreferences.SetHardDifficultyHighScore (score);

				if (coinHighScore < coinScore) {
					GamePreferences.SetHardDifficultyCoinScore (coinScore);
				}
			}

			gameStartedFromMainMenu = false;
			gameRestartedAfterPlayerDied = false;

			GameplayController.instance.GameOverShowPanel (score, coinScore);

		} else {
			// se ho ancora delle vite allora mi salvo il punteggio perchè devo ricominciare!
			this.score = score;
			this.coinScore = coinScore;
			this.lifeScore = lifeScore;

			gameRestartedAfterPlayerDied = true;
			gameStartedFromMainMenu = false;

			GameplayController.instance.PlayerDiedRestartTheGame ();

		}

	}
}
