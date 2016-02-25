using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.Scripts.Data;

public class EndGameUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayAgainPressed() {
		GameManager.instance.ResetGame();
	}

	public void QuitToMenuPressed() {
		GameManager.instance.RemoveAllPlayers();
		SceneManager.LoadScene(0);
	}
}
