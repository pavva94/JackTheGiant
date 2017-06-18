using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounds : MonoBehaviour {

	// min e max dove possono spawnare le nuvole, i bordi della telecamera
	private float minX, maxX;


	// Use this for initialization
	void Start () {
		SetMinAndMaxX ();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x < minX) {
			Vector3 temp = transform.position;
			temp.x = minX;
			transform.position = temp;
		}
		if (transform.position.x > maxX) {
			Vector3 temp = transform.position;
			temp.x = maxX;
			transform.position = temp;
		}
	}


	void SetMinAndMaxX () {
		// converte la risolizione dello schermo in coordinate di Unity
		Vector3 bounds = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 0f));

		// prendo il max X in cui istanziare le nuvole, tolgo 0.5 perchè senno metà nuvola potrebbe andare fuori dallo schermo dato che Unity prende il centro del GameObject 
		maxX = bounds.x - 0.2f;
		minX = - bounds.x + 0.2f;
	}
}
