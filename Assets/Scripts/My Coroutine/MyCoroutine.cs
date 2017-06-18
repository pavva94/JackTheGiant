using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyCoroutine {

	public static IEnumerator WaitForRealSecond (float time) {
		float start = Time.realtimeSinceStartup;

		// aspetto time secondi reali
		while (Time.realtimeSinceStartup < (start + time)) {
			yield return null;
		}

	}
}
