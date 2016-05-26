using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AutoKeyUI : MonoBehaviour {

	public PlayerID id;

	private enum Type {Button, Axis, Trigger};

	[SerializeField]
	private Type type;
	public ControllerInputWrapper.Buttons button;
	public ControllerInputWrapper.Axis axis;
	public ControllerInputWrapper.Triggers trigger;

	private bool uiImage = false;

	// Use this for initialization
	void OnEnable () {
		if(GetComponent<Image>() != null) uiImage = true;
		if(uiImage) GetComponent<Image>().sprite = null;
		else GetComponent<SpriteRenderer>().sprite = null;

	}
	
	// Update is called once per frame
	void Update () {
		
		if(ControllerManager.instance.NumPlayers < (int)id) return;
		if(id == PlayerID.One) GetComponent<Image>();
		if(uiImage && GetComponent<Image>().sprite != null) return;
		else if(!uiImage && GetComponent<SpriteRenderer>().sprite != null) return;


		switch(type) {
		case Type.Axis:
			switch(axis) {
			case ControllerInputWrapper.Axis.DPadX:

				break;
			case ControllerInputWrapper.Axis.DPadY:

				break;
			case ControllerInputWrapper.Axis.LeftStickX:

				break;
			case ControllerInputWrapper.Axis.LeftStickY:
				switch(ControllerManager.instance.PlayerControlType(id)) {
				case ControllerManager.ControlType.Keyboard:
					if(uiImage) {
						GetComponent<Image>().sprite = InputTester.instance.KEY_UPDOWNLEFTRIGHT;
					} else {
						GetComponent<SpriteRenderer>().sprite = InputTester.instance.KEY_UPDOWNLEFTRIGHT;
					}
					break;
				case ControllerManager.ControlType.Xbox:
				case ControllerManager.ControlType.PS4:
					if(uiImage) {
						GetComponent<Image>().sprite = InputTester.instance.JOYLEFT;
					} else {
						GetComponent<SpriteRenderer>().sprite = InputTester.instance.JOYLEFT;
					}
					break;
				}
				break;
			case ControllerInputWrapper.Axis.RightStickX:

				break;
			case ControllerInputWrapper.Axis.RightStickY:

				break;
			}
			break;
		case Type.Button:
			switch(button) {
			case ControllerInputWrapper.Buttons.A:
				switch(ControllerManager.instance.PlayerControlType(id)) {
				case ControllerManager.ControlType.Keyboard:
					if(uiImage) {
						GetComponent<Image>().sprite = InputTester.instance.KEY_A;
					} else {
						GetComponent<SpriteRenderer>().sprite = InputTester.instance.KEY_A;
					}
					break;
				case ControllerManager.ControlType.Xbox:
					if(uiImage) {
						GetComponent<Image>().sprite = InputTester.instance.XBOX_A;
					} else {
						GetComponent<SpriteRenderer>().sprite = InputTester.instance.XBOX_A;
					}
					break;
				case ControllerManager.ControlType.PS4:
					if(uiImage) {
						GetComponent<Image>().sprite = InputTester.instance.PS4_A;
					} else {
						GetComponent<SpriteRenderer>().sprite = InputTester.instance.PS4_A;
					}
					break;
				}
				break;
			case ControllerInputWrapper.Buttons.B:
				switch(ControllerManager.instance.PlayerControlType(id)) {
				case ControllerManager.ControlType.Keyboard:
					if(uiImage) {
						GetComponent<Image>().sprite = InputTester.instance.KEY_B;
					} else {
						GetComponent<SpriteRenderer>().sprite = InputTester.instance.KEY_B;
					}
					break;
				case ControllerManager.ControlType.Xbox:
					if(uiImage) {
						GetComponent<Image>().sprite = InputTester.instance.XBOX_B;
					} else {
						GetComponent<SpriteRenderer>().sprite = InputTester.instance.XBOX_B;
					}
					break;
				case ControllerManager.ControlType.PS4:
					if(uiImage) {
						GetComponent<Image>().sprite = InputTester.instance.PS4_B;
					} else {
						GetComponent<SpriteRenderer>().sprite = InputTester.instance.PS4_B;
					}
					break;
				}
				break;
			case ControllerInputWrapper.Buttons.LeftBumper:

				break;
			case ControllerInputWrapper.Buttons.LeftStickClick:

				break;
			case ControllerInputWrapper.Buttons.RightBumper:

				break;
			case ControllerInputWrapper.Buttons.RightStickClick:

				break;
			case ControllerInputWrapper.Buttons.Start:

				break;
			case ControllerInputWrapper.Buttons.X:

				break;
			case ControllerInputWrapper.Buttons.Y:

				break;
			}
			break;
		case Type.Trigger:

			break;
		}
	}


}
