using UnityEngine;
using System.Collections;

public class AutoKeyUI : MonoBehaviour {

	public PlayerID id;

	private enum Type {Button, Axis, Trigger};

	[SerializeField]
	private Type type;
	public ControllerInputWrapper.Buttons button;
	public ControllerInputWrapper.Axis axis;
	public ControllerInputWrapper.Triggers trigger;

	// Use this for initialization
	void Start () {
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

					break;
				case ControllerManager.ControlType.Xbox:

					break;
				case ControllerManager.ControlType.PS4:

					break;
				}
				break;
			case ControllerInputWrapper.Buttons.B:

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
	
	// Update is called once per frame
	void Update () {
		
	}


}
