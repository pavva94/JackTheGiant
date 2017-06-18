using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScaler : MonoBehaviour {

	// Use this for initialization
	void Start () {

		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		Vector3 tempScale = transform.localScale;

		// mi da la larghezza dell'immagine a cui è collegato questo script
		float width = sr.sprite.bounds.size.x;

		// l'altezza dello schermo
		float worldHeight = Camera.main.orthographicSize * 2f;

		// largezza dello schermo
		float worldWidth = worldHeight / Screen.height * Screen.width;

		// così calcolo lo Scale che devo applicare all'immagine per adattarsi allo schermo
		tempScale.x = worldWidth / width;

		transform.localScale = tempScale;

	}

}
