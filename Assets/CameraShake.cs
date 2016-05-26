using UnityEngine;
using System.Collections;
using Assets.Scripts.Data;

public class CameraShake : MonoBehaviour {

	public void ScreenShake(float amount, bool right) {
		StopAllCoroutines();
		StartCoroutine(ShakeCam(amount, right));
	}

	private IEnumerator ShakeCam(float amount, bool right) {
		GetComponent<Animator>().enabled = false;
		if(right) {
			transform.position -= Vector3.right*amount*0.5f;
		} else {
			transform.position += Vector3.right*amount*0.5f;
		}
		GetComponent<Camera>().orthographicSize = 5 - amount/5f;
		float timer = 0;
		while (timer < 1) {
			transform.position = Vector3.Slerp(transform.position, new Vector3(0,-1,-10),timer);
			GetComponent<Camera>().orthographicSize = Mathf.Lerp(5 - amount/5f,5,timer);
			timer += GameManager.instance.DeltaTime*2f;
			yield return new WaitForEndOfFrame();
		}
		GetComponent<Animator>().enabled = true;
		yield return null;
	}
}
