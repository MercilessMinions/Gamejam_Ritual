using UnityEngine;
using System.Collections;
using Assets.Scripts.Data;
using UnityEngine.UI;
using Assets.Scripts.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour {

	public GameObject defaultSelected;

	void OnEnable() {
		GameManager.Pause += Pause;
		GameManager.Unpause += UnPause;
	}

	void OnDisable() {
		GameManager.Pause -= Pause;
		GameManager.Unpause -= UnPause;
	}


	void Pause() {
		transform.root.GetComponent<Animator>().SetTrigger("Pause");
		Navigator.defaultGameObject = defaultSelected;
	}

	void UnPause() {
		transform.root.GetComponent<Animator>().SetTrigger("Pause");
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.DPadY, PlayerID.One) > 0
			|| ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.LeftStickY, PlayerID.One) > ControllerManager.CUSTOM_DEADZONE) {
			Navigator.Navigate(Assets.Scripts.Util.Enums.MenuDirections.Up);
		}
		if(ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.DPadY, PlayerID.One) < 0
			|| ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.LeftStickY, PlayerID.One) < -ControllerManager.CUSTOM_DEADZONE) {
			Navigator.Navigate(Assets.Scripts.Util.Enums.MenuDirections.Down);
		}
		if(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.A, PlayerID.One)) {
			Navigator.CallSubmit();
		}
	}

	public void UnPauseGame() {
		GameManager.instance.UnPauseGame();
	}

	public void UnPauseAndQuit() {
		GameManager.instance.UnPauseGame();
		GameManager.instance.inGame = false;
		GameManager.instance.RemoveAllPlayers();
		SceneManager.LoadScene(0);
		enabled = false;
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void UnPauseAndRestart() {
		EventSystem.current.SetSelectedGameObject(null);
		GameManager.instance.UnPauseGame();
		SceneManager.LoadScene(1);
	}
}
