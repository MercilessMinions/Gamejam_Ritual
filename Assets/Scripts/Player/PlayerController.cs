using UnityEngine;
using Assets.Scripts.Level;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Class that handles player specific components of the controller
    /// Uses input
    /// </summary>
    public class PlayerController : Controller
    {
		private const float MAX_HOLD_TIME = 3f;

        private bool pickedUpThisTurn;
		private float walkSoundTimer;
		private float holdTimer;

		[SerializeField]
		private Transform selectorUI;

		[SerializeField]
		private Canvas holdingUI;

		protected override void Update()
        {
            base.Update();

			if(playerColor == Color.clear) playerColor = holdingUI.transform.GetChild(1).GetComponent<Image>().color;

			if (fallingOffEdge) {
				if(transform.localScale.x <= 0) {
					life.Deactivate();
					transform.localScale = Vector3.one*0.75f;
					transform.eulerAngles = Vector3.zero;
					sprite.localEulerAngles = Vector3.zero;
					GetComponent<SpriteRenderer>().enabled = true;
					fallingOffEdge = false;
				}
			}
            if (heldObject != null)
            {
                heldObject.transform.position = transform.position + new Vector3(0, 0.1f, 0);
                heldObject.Sprite.transform.position = holdPoint.position;
                heldObject.UpdateSortingLayer();
                anim.SetBool("Carry", true);
            }
			if (Active && Data.GameManager.instance.DeltaTime > 0)
            {
				float hor = ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.LeftStickX, this.id);
				float vert = ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.LeftStickY, this.id);

				if (heldObject != null && heldObject.GetComponent<PlayerController>()) {
					hor += 0.5f*ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.LeftStickX, heldObject.GetComponent<PlayerController>().id);
					vert += 0.5f*ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.LeftStickY, heldObject.GetComponent<PlayerController>().id);
				}

                if (hor > ControllerManager.CUSTOM_DEADZONE)
                {
                    movement.MoveHorizontal(1, Mathf.Abs(hor));
                    anim.SetFloat("Speed", 1f);
                    sprite.GetComponent<SpriteRenderer>().flipX = false;

					walkSoundTimer -= Data.GameManager.instance.DeltaTime;
					if(walkSoundTimer < 0) {
						walkSoundTimer = 0.5f;
						SFXManager.instance.source.PlayOneShot(SFXManager.instance.RandomRun());
					}
                }
                else if (hor < -ControllerManager.CUSTOM_DEADZONE)
                {
                    movement.MoveHorizontal(-1, Mathf.Abs(hor));
                    anim.SetFloat("Speed", 1f);
                    sprite.GetComponent<SpriteRenderer>().flipX = true;

					walkSoundTimer -= Data.GameManager.instance.DeltaTime;
					if(walkSoundTimer < 0) {
						walkSoundTimer = 0.5f;
						SFXManager.instance.source.PlayOneShot(SFXManager.instance.RandomRun());
					}
                }
                if (vert > ControllerManager.CUSTOM_DEADZONE)
                {
                    movement.MoveVertical(1, Mathf.Abs(vert));
                    anim.SetFloat("Speed", 1f);

					walkSoundTimer -= Data.GameManager.instance.DeltaTime;
					if(walkSoundTimer < 0) {
						walkSoundTimer = 0.5f;
						SFXManager.instance.source.PlayOneShot(SFXManager.instance.RandomRun());
					}
                }
                else if (vert < -ControllerManager.CUSTOM_DEADZONE)
                {
                    movement.MoveVertical(-1, Mathf.Abs(vert));
                    anim.SetFloat("Speed", 1f);

					walkSoundTimer -= Data.GameManager.instance.DeltaTime;
					if(walkSoundTimer < 0) {
						walkSoundTimer = 0.5f;
						SFXManager.instance.source.PlayOneShot(SFXManager.instance.RandomRun());
					}
                }

                if (hor == 0 && vert == 0) anim.SetFloat("Speed", 0);

				if (ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.LeftBumper, id) && !heldObject)
                {
                    movement.InitRoll(-1);
                    anim.SetBool("Rolling", true);
					SFXManager.instance.source.PlayOneShot(SFXManager.instance.roll);
                }
				if (ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.RightBumper, id) && !heldObject)
                {
                    movement.InitRoll(1);
                    anim.SetBool("Rolling", true);
					SFXManager.instance.source.PlayOneShot(SFXManager.instance.roll);
                }
				if (ControllerManager.instance.GetTrigger(ControllerInputWrapper.Triggers.RightTrigger,this.id) <= 0)
                {
                    if (pickedUpThisTurn)
                        pickedUpThisTurn = false;
                    else
					{
                        ThrowObject();
					}
                }

				if (ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.A, id))
                {
                    if(!fallingOffEdge)
                        anim.SetBool("Attack", true);
                }
                else
                {
                    anim.SetBool("Attack", false);
                }

				if (holdCooldown >= 0)
				{
					holdingUI.transform.GetChild(1).GetComponent<Image>().fillAmount = (1-holdCooldown)*0.8f + 0.1f;
					if (holdingUI.transform.GetChild(1).GetComponent<Image>().color != Color.white)
						holdingUI.transform.GetChild(1).GetComponent<Image>().color = Color.white;
					holdCooldown -= Time.deltaTime;
				}
				else if(heldObject && holdTimer > 0)
				{
					holdTimer -= Time.deltaTime;
					holdingUI.transform.GetChild(1).GetComponent<Image>().fillAmount = (holdTimer/MAX_HOLD_TIME)*0.8f + 0.1f;
					if (holdingUI.transform.GetChild(1).GetComponent<Image>().color != playerColor)
						holdingUI.transform.GetChild(1).GetComponent<Image>().color = playerColor;
					if(holdTimer <= 0) {
						ThrowObject();
					}
				} else {
					if (holdingUI.transform.GetChild(1).GetComponent<Image>().color != playerColor)
						holdingUI.transform.GetChild(1).GetComponent<Image>().color = playerColor;
					holdingUI.transform.GetChild(1).GetComponent<Image>().fillAmount = 1;
				}
            }
        }

        protected override void HitGround()
        {
            base.HitGround();
            anim.SetBool("Picked Up", false);
            if (life.Health <= 0)
            {
                anim.SetTrigger("Dead");
                anim.SetBool("Stay Dead", true);
                active = false;
            }
        }



        void OnTriggerStay2D(Collider2D col)
        {
            if (heldObject || movement.Rolling || !active) return;
			if (holdCooldown > 0) return;
			if (ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.A, id)) return;
			SpriteObject tempObj = col.transform.root.GetComponent<SpriteObject>();
			if(tempObj)
			{
//				selectorUI.position = tempObj.transform.position;
//				selectorUI.GetComponent<SpriteRenderer>().enabled = true;
				if (ControllerManager.instance.GetTrigger(ControllerInputWrapper.Triggers.RightTrigger,this.id) > 0 && !Data.GameManager.instance.paused)
            	{
					heldObject = tempObj;
					holdTimer = MAX_HOLD_TIME;
					if(heldObject.GetComponent<PlayerController>()) 
					{
                    	heldObject.GetComponentInChildren<Animator>().SetBool("Picked Up", true);
					}
                    pickedUpThisTurn = true;
                    heldObject.Active = false;
                    heldObject.Falling = false;
                    heldObject.transform.parent = transform;
                    heldObject.Sprite.position = holdPoint.position;
				}
			}
			else
			{
//				selectorUI.GetComponent<SpriteRenderer>().enabled = false;
			}
        }

		void OnTriggerExit2D(Collider2D col) {
//			selectorUI.GetComponent<SpriteRenderer>().enabled = false;
		}

        public SpriteObject HeldObject
        {
            get { return heldObject; }
        }
    }
}
