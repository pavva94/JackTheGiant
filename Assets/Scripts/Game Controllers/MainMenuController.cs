using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	[SerializeField]
	private Button musicBtn;
	[SerializeField]
	private Sprite[] musicIcons;

	// Use this for initialization
	void Start () {
		CheckToPlayTheMusic ();
	}

	void CheckToPlayTheMusic() {
		if (GamePreferences.GetMusicState () == 1) {
			MusicController.instance.PlayMusic (true);
			musicBtn.image.sprite = musicIcons [1];
		} else {
			MusicController.instance.PlayMusic (false);
			musicBtn.image.sprite = musicIcons [0];
		}
	}
	
	public void StartGame() {
		GameManager.instance.gameStartedFromMainMenu = true;
		// SceneManager.LoadScene ("Gameplay");
		SceneFader.instance.LoadLevel("Gameplay");
	}

	public void HighscoreMenu() {
		SceneManager.LoadScene ("Highscore");
	}

	public void OptionsMenu() {
		SceneManager.LoadScene ("Options");
	}

	public void QuitGame() {
		Application.Quit ();
	}

	public void MusicButton() {
		if (GamePreferences.GetMusicState () == 0) {
			GamePreferences.SetMusicState (1);
		} else if (GamePreferences.GetMusicState () == 1) {
			GamePreferences.SetMusicState (0);
		}
		CheckToPlayTheMusic ();
	}
}
