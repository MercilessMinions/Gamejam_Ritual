using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ControllerManager test = new ControllerManager();
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.A,PlayerID.One));
	}
}
