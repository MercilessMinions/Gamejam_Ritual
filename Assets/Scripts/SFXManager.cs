using UnityEngine;
using System.Collections;

public class SFXManager : MonoBehaviour {

	public static SFXManager instance;

	public AudioSource source;

	public AudioClip menuClick, godTalk1, godTalk2, roll, jab1, jab2, jab3, run1, run2, run3, run4, run5, fireBurst;

	// Use this for initialization
	void Start () {
		instance = this;
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		source.pitch = Random.Range(0.8f,1.2f);
	}

	public AudioClip RandomRun() {
		int rand = Random.Range(0,5);
		if(rand == 0) {
			return run1;
		} else if(rand == 1) {
			return run2;
		} else if(rand == 2) {
			return run3;
		} else if(rand == 3) {
			return run4;
		} else if(rand == 4) {
			return run5;
		}
		return run1;
	}

	public AudioClip RandomGodTalk() {
		if(Random.value < 0.5f) {
			return godTalk1;
		} else {
			return godTalk2;
		}
	}

	public AudioClip RandomJab() {
		return jab1;
	}

	public AudioClip RandomRoll() {
		source.pitch = Random.Range(0.8f,1.2f);
		return roll;
	}
}
