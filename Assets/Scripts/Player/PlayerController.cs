using UnityEngine;
using Assets.Scripts.Level;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Class that handles player specific components of the controller
    /// Uses input
    /// </summary>
    public class PlayerController : Controller
    {
        private bool pickedUpThisTurn;
		private float walkSoundTimer;

		protected override void Update()
        {
            base.Update();
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
            if (Active && !Data.GameManager.instance.paused)
            {
				float hor = ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.LeftStickX, this.id);
				float vert = ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.LeftStickY, this.id);

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
                        ThrowObject();
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
			if (ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.A, id)) return;
			if (ControllerManager.instance.GetTrigger(ControllerInputWrapper.Triggers.RightTrigger,this.id) > 0 && !Data.GameManager.instance.paused)
            {
                heldObject = col.transform.root.GetComponent<SpriteObject>();
                if (heldObject)
                {
					if(heldObject.GetComponent<PlayerController>()) {
                    	heldObject.GetComponentInChildren<Animator>().SetBool("Picked Up", true);
					}
                    pickedUpThisTurn = true;
                    heldObject.Active = false;
                    heldObject.Falling = false;
                    heldObject.transform.parent = transform;
                    heldObject.Sprite.position = holdPoint.position;
                }
            }
        }

        public SpriteObject HeldObject
        {
            get { return heldObject; }
        }
    }
}
