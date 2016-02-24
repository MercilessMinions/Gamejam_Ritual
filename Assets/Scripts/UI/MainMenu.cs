using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Scripts.Data;

using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	private GameObject player1JoiningBox, player2JoiningBox, player3JoiningBox, player4JoiningBox;
	private Image player1CharacterImage, player2CharacterImage, player3CharacterImage, player4CharacterImage;
	private Text player1CharacterName, player2CharacterName, player3CharacterName, player4CharacterName;
	private Text player1ReadyText, player2ReadyText, player3ReadyText, player4ReadyText;
	private bool player1Joined, player2Joined, player3Joined, player4Joined;
	private bool player1Ready, player2Ready, player3Ready, player4Ready;
	private int player1SelectedCharacter = 0, player2SelectedCharacter = 0, player3SelectedCharacter = 0, player4SelectedCharacter = 0;
	private GameObject playerJoiningBackButton;

	public Sprite[] charSprites;
	public string[] charNames;

	private string[] countDown = {"I", "II", "III", "IV", "V"};
	private float gameCountDownTimer = 5f;

	public static GameObject player1Char, player2Char, player3Char, player4Char;
	public GameObject char1Prefab, char2Prefab, char3Prefab, char4Prefab;

	private float navTimer;

	// Use this for initialization
	void Start () {
		player1JoiningBox = GameObject.Find("Player1Box");
		player2JoiningBox = GameObject.Find("Player2Box");
		player3JoiningBox = GameObject.Find("Player3Box");
		player4JoiningBox = GameObject.Find("Player4Box");

		player1CharacterImage = GameObject.Find("Player1CharImage").GetComponent<Image>();
		player2CharacterImage = GameObject.Find("Player2CharImage").GetComponent<Image>();
		player3CharacterImage = GameObject.Find("Player3CharImage").GetComponent<Image>();
		player4CharacterImage = GameObject.Find("Player4CharImage").GetComponent<Image>();

		player1CharacterName = GameObject.Find("Player1CharacterName").GetComponent<Text>();
		player2CharacterName = GameObject.Find("Player2CharacterName").GetComponent<Text>();
		player3CharacterName = GameObject.Find("Player3CharacterName").GetComponent<Text>();
		player4CharacterName = GameObject.Find("Player4CharacterName").GetComponent<Text>();

		player1ReadyText = GameObject.Find("Player1ReadyText").GetComponent<Text>();
		player2ReadyText = GameObject.Find("Player2ReadyText").GetComponent<Text>();
		player3ReadyText = GameObject.Find("Player3ReadyText").GetComponent<Text>();
		player4ReadyText = GameObject.Find("Player4ReadyText").GetComponent<Text>();

		playerJoiningBackButton = GameObject.Find("PlayerJoiningBackButton");
	}

	private bool IsCharacterAvailable(PlayerID pid) {
		switch(pid) {
		case PlayerID.One:
			if (player2Joined && (Mathf.Abs(player2SelectedCharacter%4) == Mathf.Abs(player1SelectedCharacter%4))) return false;
			else if (player3Joined && (Mathf.Abs(player3SelectedCharacter%4) == Mathf.Abs(player1SelectedCharacter%4))) return false;
			else if (player4Joined && (Mathf.Abs(player4SelectedCharacter%4) == Mathf.Abs(player1SelectedCharacter%4))) return false;
			else return true;
			break;
		case PlayerID.Two:
			if (player1Joined && (Mathf.Abs(player1SelectedCharacter%4) == Mathf.Abs(player2SelectedCharacter%4))) return false;
			else if (player3Joined && (Mathf.Abs(player3SelectedCharacter%4) == Mathf.Abs(player2SelectedCharacter%4))) return false;
			else if (player4Joined && (Mathf.Abs(player4SelectedCharacter%4) == Mathf.Abs(player2SelectedCharacter%4))) return false;
			else return true;
			break;
		case PlayerID.Three:
			if (player2Joined && (Mathf.Abs(player2SelectedCharacter%4) == Mathf.Abs(player3SelectedCharacter%4))) return false;
			else if (player1Joined && (Mathf.Abs(player1SelectedCharacter%4) == Mathf.Abs(player3SelectedCharacter%4))) return false;
			else if (player4Joined && (Mathf.Abs(player4SelectedCharacter%4) == Mathf.Abs(player3SelectedCharacter%4))) return false;
			else return true;
			break;
		case PlayerID.Four:
			if (player2Joined && (Mathf.Abs(player2SelectedCharacter%4) == Mathf.Abs(player4SelectedCharacter%4))) return false;
			else if (player3Joined && (Mathf.Abs(player3SelectedCharacter%4) == Mathf.Abs(player4SelectedCharacter%4))) return false;
			else if (player1Joined && (Mathf.Abs(player1SelectedCharacter%4) == Mathf.Abs(player4SelectedCharacter%4))) return false;
			else return true;
			break;
		}
		return false;
	}
	
	// Update is called once per frame
	void Update () {
		if(((player1Ready == player1Joined) && (player2Joined == player2Ready) && (player3Joined == player3Ready) && (player4Joined == player4Ready))
			&& player2Ready) {
			transform.GetChild(3).GetChild(6).GetComponent<CanvasGroup>().alpha += Time.deltaTime;
			if(gameCountDownTimer <= 0) {

                //TODO: GO TO GAME
                if(player1Ready)
                {
                    GameManager.instance.InitializePlayer((Assets.Scripts.Util.Enums.Characters)Mathf.Abs(player1SelectedCharacter % 4), PlayerID.One);
                }
                if (player2Ready)
                {
                    GameManager.instance.InitializePlayer((Assets.Scripts.Util.Enums.Characters)Mathf.Abs(player2SelectedCharacter % 4), PlayerID.Two);
                }
                if (player3Ready)
                {
                    GameManager.instance.InitializePlayer((Assets.Scripts.Util.Enums.Characters)Mathf.Abs(player3SelectedCharacter % 4), PlayerID.Three);
                }
                if (player4Ready)
                {
                    GameManager.instance.InitializePlayer((Assets.Scripts.Util.Enums.Characters)Mathf.Abs(player4SelectedCharacter % 4), PlayerID.Four);
                }
                SceneManager.LoadScene("Eric Test2");
            } else {
				gameCountDownTimer -= Time.deltaTime;
				transform.GetChild(3).GetChild(6).localScale = Vector3.one * (5-gameCountDownTimer)/2f;
				transform.GetChild(3).GetChild(6).GetChild(1).GetComponent<Text>().text = countDown[(int)gameCountDownTimer];
			}
		} else {
			gameCountDownTimer = 5;
			transform.GetChild(3).GetChild(6).localScale = Vector3.one;
			transform.GetChild(3).GetChild(6).GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
		}
		navTimer -= Time.deltaTime;
		if(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SplashLoop")) {
			ControllerManager.instance.AddPlayer(ControllerInputWrapper.Buttons.Start);
			if(ControllerManager.instance.NumPlayers > 0) {
				GetComponent<Animator>().SetTrigger("SplashToMainMenu");
			}
		}

		//Player joining screen
		if(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("MainMenuToPlayerJoin")
			&& !GetComponent<Animator>().IsInTransition(0)) {
			ControllerManager.instance.AddPlayer(ControllerInputWrapper.Buttons.A);
//			ControllerManager.instance.AllowPlayerRemoval(ControllerInputWrapper.Buttons.B);

			//Player 1 CharacterSelection + Readying
			if(player1Joined) {
				player1JoiningBox.transform.FindChild("P1_Press_A_Text").GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
				player1JoiningBox.transform.GetChild(1).GetComponent<CanvasGroup>().alpha += Time.deltaTime;
				if(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.A, PlayerID.One) && !player1Ready && IsCharacterAvailable(PlayerID.One)) {
					player1Ready = true;
				} else if(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.B, PlayerID.One) && player1Ready) {
					player1Ready = false;
				}
				if(!player1Ready) {
					if(ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.DPadX, PlayerID.One) > 0 && navTimer < 0) {
						SFXManager.instance.source.PlayOneShot(SFXManager.instance.menuClick);
						navTimer = 0.2f;
						player1CharacterImage.sprite = charSprites[Mathf.Abs(++player1SelectedCharacter%4)];
						player1CharacterName.text = charNames[Mathf.Abs(player1SelectedCharacter%4)];
					} else if(ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.DPadX, PlayerID.One) < 0 && navTimer < 0) {
						SFXManager.instance.source.PlayOneShot(SFXManager.instance.menuClick);
						navTimer = 0.2f;
						player1CharacterImage.sprite = charSprites[Mathf.Abs(--player1SelectedCharacter%4)];
						player1CharacterName.text = charNames[Mathf.Abs(player1SelectedCharacter%4)];
					}
				}
			} else {
				player1JoiningBox.transform.FindChild("P1_Press_A_Text").GetComponent<CanvasGroup>().alpha += Time.deltaTime;
				player1JoiningBox.transform.GetChild(1).GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
			}
			if(player1Ready) {
				player1ReadyText.GetComponent<CanvasGroup>().alpha += Time.deltaTime;
				player1JoiningBox.transform.GetChild(1).GetChild(0).GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
			} else {
				player1ReadyText.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
				player1JoiningBox.transform.GetChild(1).GetChild(0).GetComponent<CanvasGroup>().alpha += Time.deltaTime;
			}

			//Player 1 joining
			if(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.A, PlayerID.One) && !player1Joined) {
				player1Joined = true;
			}
			if (ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.B, PlayerID.One) && player1Joined) {
				player1Joined = false;
			} else if (ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.B, PlayerID.One)) { //Only Player 1 is allowed to go back
				ExecuteEvents.Execute(playerJoiningBackButton, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
			}

			//Player 2 CharacterSelection + Readying
			if(player2Joined) {
				player2JoiningBox.transform.FindChild("P2_Press_A_Text").GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
				player2JoiningBox.transform.GetChild(1).GetComponent<CanvasGroup>().alpha += Time.deltaTime;
				if(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.A, PlayerID.Two) && !player2Ready && IsCharacterAvailable(PlayerID.Two)) {
					player2Ready = true;
				} else if(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.B, PlayerID.Two) && player2Ready) {
					player2Ready = false;
				}
				if(!player2Ready) {
					if((ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.DPadX, PlayerID.Two) > 0) && navTimer < 0) {
						SFXManager.instance.source.PlayOneShot(SFXManager.instance.menuClick);
						navTimer = 0.2f;
						player2CharacterImage.sprite = charSprites[Mathf.Abs(++player2SelectedCharacter%4)];
						player2CharacterName.text = charNames[Mathf.Abs(player2SelectedCharacter%4)];
					} else if((ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.DPadX, PlayerID.Two) < 0) && navTimer < 0) {
						SFXManager.instance.source.PlayOneShot(SFXManager.instance.menuClick);
						navTimer = 0.2f;
						player2CharacterImage.sprite = charSprites[Mathf.Abs(--player2SelectedCharacter%4)];
						player2CharacterName.text = charNames[Mathf.Abs(player2SelectedCharacter%4)];
					}
				}
			} else {
				player2JoiningBox.transform.FindChild("P2_Press_A_Text").GetComponent<CanvasGroup>().alpha += Time.deltaTime;
				player2JoiningBox.transform.GetChild(1).GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
			}
			if(player2Ready) {
				player2ReadyText.GetComponent<CanvasGroup>().alpha += Time.deltaTime;
				player2JoiningBox.transform.GetChild(1).GetChild(0).GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
			} else {
				player2ReadyText.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
				player2JoiningBox.transform.GetChild(1).GetChild(0).GetComponent<CanvasGroup>().alpha += Time.deltaTime;
			}

			//Player 2 joining
			if(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.A, PlayerID.Two) && !player2Joined) {
				player2Joined = true;
			}
			if (ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.B, PlayerID.Two) && player2Joined) {
				player2Joined = false;
			}

			//Player 3 CharacterSelection + Readying
			if(player3Joined) {
				player3JoiningBox.transform.FindChild("P3_Press_A_Text").GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
				player3JoiningBox.transform.GetChild(1).GetComponent<CanvasGroup>().alpha += Time.deltaTime;
				if(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.A, PlayerID.Three) && !player3Ready && IsCharacterAvailable(PlayerID.Three)) {
					player3Ready = true;
				} else if(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.B, PlayerID.Three) && player3Ready) {
					player3Ready = false;
				}
				if(!player3Ready) {
					if((ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.DPadX, PlayerID.Three) > 0) && navTimer < 0) {
						SFXManager.instance.source.PlayOneShot(SFXManager.instance.menuClick);
						navTimer = 0.2f;
						player3CharacterImage.sprite = charSprites[Mathf.Abs(++player3SelectedCharacter%4)];
						player3CharacterName.text = charNames[Mathf.Abs(player3SelectedCharacter%4)];
					} else if((ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.DPadX, PlayerID.Three) < 0) && navTimer < 0) {
						SFXManager.instance.source.PlayOneShot(SFXManager.instance.menuClick);
						navTimer = 0.2f;
						player3CharacterImage.sprite = charSprites[Mathf.Abs(--player3SelectedCharacter%4)];
						player3CharacterName.text = charNames[Mathf.Abs(player3SelectedCharacter%4)];
					}
				}
			} else {
				player3JoiningBox.transform.FindChild("P3_Press_A_Text").GetComponent<CanvasGroup>().alpha += Time.deltaTime;
				player3JoiningBox.transform.GetChild(1).GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
			}
			if(player3Ready) {
				player3ReadyText.GetComponent<CanvasGroup>().alpha += Time.deltaTime;
				player3JoiningBox.transform.GetChild(1).GetChild(0).GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
			} else {
				player3ReadyText.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
				player3JoiningBox.transform.GetChild(1).GetChild(0).GetComponent<CanvasGroup>().alpha += Time.deltaTime;
			}

			//Player 3 joining
			if(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.A, PlayerID.Three) && !player3Joined) {
				player3Joined = true;
			}
			if (ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.B, PlayerID.Three) && player3Joined) {
				player3Joined = false;
			}

			//Player 4 CharacterSelection + Readying
			if(player4Joined) {
				player4JoiningBox.transform.FindChild("P4_Press_A_Text").GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
				player4JoiningBox.transform.GetChild(1).GetComponent<CanvasGroup>().alpha += Time.deltaTime;
				if(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.A, PlayerID.Four) && !player4Ready && IsCharacterAvailable(PlayerID.Four)) {
					player4Ready = true;
				} else if(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.B, PlayerID.Four) && player4Ready) {
					player4Ready = false;
				}
				if(!player4Ready) {
					if((ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.DPadX, PlayerID.Four) > 0) && navTimer < 0) {
						SFXManager.instance.source.PlayOneShot(SFXManager.instance.menuClick);
						navTimer = 0.2f;
						player4CharacterImage.sprite = charSprites[Mathf.Abs(++player4SelectedCharacter%4)];
						player4CharacterName.text = charNames[Mathf.Abs(player4SelectedCharacter%4)];
					} else if((ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.DPadX, PlayerID.Four) < 0) && navTimer < 0) {
						SFXManager.instance.source.PlayOneShot(SFXManager.instance.menuClick);
						navTimer = 0.2f;
						player4CharacterImage.sprite = charSprites[Mathf.Abs(--player4SelectedCharacter%4)];
						player4CharacterName.text = charNames[Mathf.Abs(player4SelectedCharacter%4)];
					}
				}
			} else {
				player4JoiningBox.transform.FindChild("P4_Press_A_Text").GetComponent<CanvasGroup>().alpha += Time.deltaTime;
				player4JoiningBox.transform.GetChild(1).GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
			}
			if(player4Ready) {
				player4ReadyText.GetComponent<CanvasGroup>().alpha += Time.deltaTime;
				player4JoiningBox.transform.GetChild(1).GetChild(0).GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
			} else {
				player4ReadyText.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
				player4JoiningBox.transform.GetChild(1).GetChild(0).GetComponent<CanvasGroup>().alpha += Time.deltaTime;
			}

			//Player 4 joining
			if(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.A, PlayerID.Four) && !player4Joined) {
				player4Joined = true;
			}
			if (ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.B, PlayerID.Four) && player4Joined) {
				player4Joined = false;
			}
		} else {
			EventSystem cur = EventSystem.current;
			GameObject curSelectedGameObject = cur.currentSelectedGameObject;
			if(curSelectedGameObject == null) {
				curSelectedGameObject = cur.firstSelectedGameObject;
				cur.SetSelectedGameObject(curSelectedGameObject);
			}
			if(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.A, PlayerID.One)) {
				ExecuteEvents.Execute(curSelectedGameObject, new PointerEventData(cur), ExecuteEvents.submitHandler);
			}
			if((ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.DPadY, PlayerID.One) > 0f) && navTimer < 0) {
				SFXManager.instance.source.PlayOneShot(SFXManager.instance.menuClick);
				navTimer = 0.1f;
				Selectable sel = curSelectedGameObject.GetComponent<Selectable>();
				Selectable up = sel.FindSelectableOnUp();
				if(up) {
					cur.SetSelectedGameObject(up.gameObject);
				}
			} else if((ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.DPadY, PlayerID.One) < 0) && navTimer < 0) {
				SFXManager.instance.source.PlayOneShot(SFXManager.instance.menuClick);
				navTimer = 0.1f;
				Selectable sel = curSelectedGameObject.GetComponent<Selectable>();
				Selectable down = sel.FindSelectableOnDown();
				if(down) {
					cur.SetSelectedGameObject(down.gameObject);
				}
			}
		}
	}

	public void GoToPlayerJoin() {
		GetComponent<Animator>().SetTrigger("MainMenuToPlayerJoin");
	}

	public void PlayerJoinToMainMenu() {
		GetComponent<Animator>().SetTrigger("PlayerJoinToMainMenu");
		player1Joined = false;
		player2Joined = false;
		player3Joined = false;
		player4Joined = false;
		player1Ready = false;
		player2Ready = false;
		player3Ready = false;
		player4Ready = false;
	}
}
