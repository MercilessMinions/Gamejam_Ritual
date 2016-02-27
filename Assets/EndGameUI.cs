using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Assets.Scripts.Data;

public class EndGameUI : MonoBehaviour {
    private float navTimer = 0;
    void Update()
    {
        navTimer -= Time.deltaTime;
        EventSystem cur = EventSystem.current;
        GameObject curSelectedGameObject = cur.currentSelectedGameObject;
        if (curSelectedGameObject == null)
        {
            curSelectedGameObject = cur.firstSelectedGameObject;
            cur.SetSelectedGameObject(curSelectedGameObject);
        }
        if (ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.A, PlayerID.One))
        {
            ExecuteEvents.Execute(curSelectedGameObject, new PointerEventData(cur), ExecuteEvents.submitHandler);
        }
        if ((ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.DPadX, PlayerID.One) > ControllerManager.CUSTOM_DEADZONE
            || ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.LeftStickX, PlayerID.One) > ControllerManager.CUSTOM_DEADZONE) && navTimer < 0)
        {
            SFXManager.instance.source.PlayOneShot(SFXManager.instance.menuClick);
            navTimer = 0.3f;
            Selectable sel = curSelectedGameObject.GetComponent<Selectable>();
            Selectable right = sel.FindSelectableOnRight();
            if (right)
            {
                cur.SetSelectedGameObject(right.gameObject);
            }
        }
        else if ((ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.DPadX, PlayerID.One) < -ControllerManager.CUSTOM_DEADZONE
          || ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.LeftStickX, PlayerID.One) < -ControllerManager.CUSTOM_DEADZONE) && navTimer < 0)
        {
            SFXManager.instance.source.PlayOneShot(SFXManager.instance.menuClick);
            navTimer = 0.3f;
            Selectable sel = curSelectedGameObject.GetComponent<Selectable>();
            Selectable left = sel.FindSelectableOnLeft();
            if (left)
            {
                cur.SetSelectedGameObject(left.gameObject);
            }
        }
        else if (ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.DPadX, PlayerID.One) == 0f
          && ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.LeftStickX, PlayerID.One) == 0f)
        {
            navTimer = 0f;
        }
    }

	public void PlayAgainPressed() {
		GameManager.instance.ResetGame();
        enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
	}

	public void QuitToMenuPressed() {
		GameManager.instance.RemoveAllPlayers();
		SceneManager.LoadScene(0);
        enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
    }
}
