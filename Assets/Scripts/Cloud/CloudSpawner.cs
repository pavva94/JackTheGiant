using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour {
	[SerializeField]
	private GameObject[] clouds;

	private float distanceBetweenCloud = 3.0f;

	// min e max dove possono spawnare le nuvole, i bordi della telecamera
	private float minX, maxX;

	// la posizione in Y della nuvola più in bassom, dobbiamo saperla per spawnare nuvole sotto questa
	private float lastCloudPositionY;

	private float controlX;

	[SerializeField]
	private GameObject[] collectables;

	private GameObject player;

	// Use this for initialization
	void Awake () {
		controlX = 0f;
		SetMinAndMaxX ();
		CreateClouds ();

		player = GameObject.Find ("Player");

		for (int i = 0; i < collectables.Length; i++) {
			collectables [i].SetActive (false);
		}
	}

	void Start () {
		PositionThePlayer ();
	}
	
	void SetMinAndMaxX () {
		// converte la risolizione dello schermo in coordinate di Unity
		Vector3 bounds = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 0f));

		// prendo il max X in cui istanziare le nuvole, tolgo 0.5 perchè senno metà nuvola potrebbe andare fuori dallo schermo dato che Unity prende il centro del GameObject 
		maxX = bounds.x - 0.5f;
		minX = - bounds.x + 0.5f;
	}

	void Shuffle (GameObject[] arrayToShuffle) {
		for (int i = 0; i < arrayToShuffle.Length; i++) {
			GameObject temp = arrayToShuffle [i];
			int random = Random.Range (i, arrayToShuffle.Length);
			arrayToShuffle [i] = arrayToShuffle [random];
			arrayToShuffle [random] = temp;

		}
		
	}

	void CreateClouds () {
		Shuffle (clouds);

		float posistionY = 0f;

		for (int i = 0; i < clouds.Length; i++) {
			Vector3 temp = clouds [i].transform.position;

			temp.y = posistionY;

			// controllo la logica di spawn della nuvola sulle x per non metterle una sotto l'altra
			// controlX sono più o meno gli step di spawn delle nuvole controllando così lo zig zag delle nuvole
			if (controlX == 0) {
				temp.x = Random.Range (0.0f, maxX);
				controlX = 1;
			} else if (controlX == 1) {
				temp.x = Random.Range (0.0f, minX);
				controlX = 2;
			} else if (controlX == 2) {
				temp.x = Random.Range (1.0f, maxX);
				controlX = 3;
			} else if (controlX == 3) {
				temp.x = Random.Range (-1.0f, minX);
				controlX = 0;
			}
			// mi salvo la posizione Y della nuvola
			lastCloudPositionY = posistionY;

			clouds [i].transform.position = temp;

			posistionY -= distanceBetweenCloud;
		}
	}

	void PositionThePlayer () {
		GameObject[] darkClouds = GameObject.FindGameObjectsWithTag ("Deadly");
		GameObject[] cloudsInGame = GameObject.FindGameObjectsWithTag ("Cloud");

		for (int i = 0; i < darkClouds.Length; i++) {

			// se la prima nuvola è una darkCloud allora la scambio con la prima nuvola chiara
			if (darkClouds [i].transform.position.y == 0f) {
				Vector3 t = darkClouds [i].transform.position;

				darkClouds [i].transform.position = new Vector3 (
					cloudsInGame [0].transform.position.x,
					cloudsInGame [0].transform.position.x,
					cloudsInGame [0].transform.position.y
				);

				cloudsInGame [0].transform.position = t;
			}
		}

		// riposiziono il player alla prima nuvola
		Vector3 temp = cloudsInGame[0].transform.position;

		// trovo la prima nuvola
		for (int i = 1; i < cloudsInGame.Length; i++) {
			if (temp.y < cloudsInGame [i].transform.position.y) {
				temp = cloudsInGame [i].transform.position;
			}
		}

		// metto il player sopra la nuvola
		temp.y += 0.6f;

		player.transform.position = temp;
	}


	void OnTriggerEnter2D (Collider2D target) {
		if (target.tag == "Cloud" || target.tag == "Deadly") {

			// se il target è l'ultima nuvola allora devo ricreare le nuvole
			if (target.transform.position.y == lastCloudPositionY) {
				Shuffle (clouds);
				Shuffle (collectables);

				Vector3 temp = target.transform.position;

				for (int i = 0; i < clouds.Length; i++) {

					// solo se la nuvola non è attiva allora la riposiziono
					if (!clouds [i].activeInHierarchy) {

						// controllo la logica di spawn della nuvola sulle x per non metterle una sotto l'altra
						// controlX sono più o meno gli step di spawn delle nuvole controllando così lo zig zag delle nuvole
						if (controlX == 0) {
							temp.x = Random.Range (0.0f, maxX);
							controlX = 1;
						} else if (controlX == 1) {
							temp.x = Random.Range (0.0f, minX);
							controlX = 2;
						} else if (controlX == 2) {
							temp.x = Random.Range (1.0f, maxX);
							controlX = 3;
						} else if (controlX == 3) {
							temp.x = Random.Range (-1.0f, minX);
							controlX = 0;
						}

						temp.y -= distanceBetweenCloud;

						// mi salvo l'ultima posizione Y della nuvola istanziata
						lastCloudPositionY = temp.y;

						clouds [i].transform.position = temp;
						clouds [i].SetActive (true);

						int random = Random.Range (0, collectables.Length);

						// spawn dei bonus sopra le nuvole non nere
						if (clouds [i].tag != "Deadly") {
							// se il bonus è già attivo lo lascio dov'è
							if (!collectables [random].activeInHierarchy) {
								Vector3 temp2 = clouds [i].transform.position;
								temp2.y += 0.7f;

								// spawn della vita solo se il giocatore ha meno di due vite
								if (collectables [random].tag == "Life") {
									if (PlayerScore.lifeCount < 2) {
										collectables [random].transform.position = temp2;
										collectables [random].SetActive (true);

									}
								} else {
									// metto il bonus sopra la nuvola
									collectables [random].transform.position = temp2;
									collectables [random].SetActive (true);
								}

							}
						}

					}
				}
				// controllo la logica di spawn della nuvola sulle x per non metterle una sotto l'altra
				// controlX sono più o meno gli step di spawn delle nuvole controllando così lo zig zag delle nuvole
				if (controlX == 0) {
					temp.x = Random.Range (0.0f, maxX);
					controlX = 1;
				} else if (controlX == 1) {
					temp.x = Random.Range (0.0f, minX);
					controlX = 2;
				} else if (controlX == 2) {
					temp.x = Random.Range (1.0f, maxX);
					controlX = 3;
				} else if (controlX == 3) {
					temp.x = Random.Range (-1.0f, minX);
					controlX = 0;
				}


			}
		}
	}

}
