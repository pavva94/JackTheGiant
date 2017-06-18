using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float speed = 8f, maxVelocity = 4f;

	private Rigidbody2D myBody;
	private Animator anim;

	void Awake () {
		myBody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	

	void FixedUpdate () {
		PlayerMoveKeyboard ();
	}

	void PlayerMoveKeyboard () {
		float forceX = 0f;
		float vel = Mathf.Abs (myBody.velocity.x);
		// -1 0 1 in base alle freccette destra sinistra
		float h = Input.GetAxisRaw ("Horizontal");

		if (h > 0) {
			if (vel < maxVelocity)
				forceX = speed;

			// faccio partire l'animazione 
			anim.SetBool ("Walk", true);

			// giro il player a destra
			Vector3 temp = transform.localScale;
			temp.x = Mathf.Abs (temp.x);
			transform.localScale = temp;
		
		} else if (h < 0) {
			if (vel < maxVelocity)
				forceX = -speed;

			// faccio partire l'animazione
			anim.SetBool ("Walk", true);

			// giro il player a sinistra
			Vector3 temp = transform.localScale;
			temp.x = - Mathf.Abs (temp.x);
			transform.localScale = temp;

		} else {
			// stopppo l'animazione
			anim.SetBool ("Walk", false);
		}

		myBody.AddForce (new Vector2 (forceX, 0));
	}
}
