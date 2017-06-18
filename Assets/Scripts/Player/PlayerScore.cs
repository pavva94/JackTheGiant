using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour {

	[SerializeField]
	private AudioClip coinClip, lifeClip;

	private CameraScript cameraScript;

	private Vector3 previusPosition;
	private bool countScore;

	public static int scoreCount;
	public static int lifeCount;
	public static int coinCount;

	void Awake() {
		cameraScript = Camera.main.GetComponent<CameraScript> ();
	}
  
	// Use this for initialization
	void Start () {
		previusPosition = transform.position;
		countScore = true;
	}
	
	// Update is called once per frame
	void Update () {
		CountScore ();
	}

	void CountScore() {
		if (countScore) {
			// aumento lo score di uno ogni volta che la Y è minore di prima
			// quindi mentre scendo aumenta il punteggio, se mi fermo no
			if (transform.position.y < previusPosition.y) {
				scoreCount++;
			}

			previusPosition = transform.position;
			GameplayController.instance.SetScore (scoreCount);
		}
	}

	void OnTriggerEnter2D (Collider2D target) {

		if (target.tag == "Coin") {
			coinCount++;
			scoreCount += 200;

			GameplayController.instance.SetScore (scoreCount);
			GameplayController.instance.SetCoinScore (coinCount);

			AudioSource.PlayClipAtPoint (coinClip, transform.position);
			target.gameObject.SetActive (false);
		}

		if (target.tag == "Life") {
			lifeCount++;
			scoreCount += 200;

			GameplayController.instance.SetScore (scoreCount);
			GameplayController.instance.SetLifeScore (lifeCount);

			AudioSource.PlayClipAtPoint (lifeClip, transform.position);
			target.gameObject.SetActive (false);
		}

		if (target.tag == "Bounds" || target.tag == "Deadly") {
			countScore = false;
			// così fermo il movimento automatico della telecamera
			cameraScript.moveCamera = false;
			lifeCount--;

			// porto fuori dallo schermo il player per far vedere che è morto
			transform.position = new Vector3 (500, 500, 0);

			GameManager.instance.CheckGameStatus (scoreCount, coinCount, lifeCount);
		}


	}
}
