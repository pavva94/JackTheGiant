using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesScript : MonoBehaviour {

	// funzione chiamata quando viene attivato il componente
	void OnEnable() {
		// distruggo il componente dopo x secondi
		Invoke ("DestroyCollectable", 8f);
	}

	void DestroyCollectable() {
		gameObject.SetActive (false);
	}
}
