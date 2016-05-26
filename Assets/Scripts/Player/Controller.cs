using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.Level;
using System;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// This class wil manage all the player's (and enemy's) components,
    /// such as movement, data , etc
    /// Will also allow the different components to talk to one another
    /// </summary>
    public class Controller : SpriteObject
    {
        // ID for identifying which player is accepting input
        [SerializeField]
        protected PlayerID id;
        [SerializeField]
        protected Util.Enums.Characters character;

        // Componenets to manage
        protected Movement movement;
        protected Life life;

        [SerializeField]
        protected Transform holdPoint;

		public SpriteObject heldObject;

		protected float holdCooldown;

        protected Animator anim;

		protected Color playerColor;

        void Awake()
        {
            // Init all componenets
            InitializePlayerComponents();
            anim = GetComponentInChildren<Animator>();
        }

        void Start()
        {
            base.Init();
        }

		protected override void Update() {
			base.Update();
			if(life.IsInvincible) {
				base.sprite.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.5f);
			} else if(life.IsHit) {
				base.sprite.GetComponent<SpriteRenderer>().color = new Color(1,0,0,1);
			} else {
				base.sprite.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
			}
		}

        /// <summary>
        /// Assigning references
        /// </summary>
        protected virtual void InitializePlayerComponents()
        {
            // Add all components to manage
            life = GetComponent<Life>();
            movement = GetComponent<Movement>();

            // Tell all components this is their controller
            life.Controller = this;
            movement.Controller = this;

            active = true;
        }

        public void ThrowObject()
        {
            if (heldObject)
            {
                if (movement.FacingRight)
                    heldObject.Force = 30f;
                else
                    heldObject.Force = -30f;
                heldObject.Falling = true;
                heldObject.transform.parent = null;
                heldObject = null;
                anim.SetBool("Carry", false);
				holdCooldown = 1f;
            }
        }

        /// <summary>
        /// Disables the player
        /// </summary>
        public void Disable(bool goInactive = true)
        {
           active = false;
           if(goInactive) gameObject.SetActive(false);
        }

		public override void FellOffEdge ()
		{
			base.FellOffEdge ();
			ThrowObject();
		}


        /// <summary>
        /// Reenables the player
        /// </summary>
        public void Enable()
        {
           	active = true;
           	gameObject.SetActive(true);
			base.RespawnReset();
			anim.SetBool("Club", movement.MeleeEnabled);
			if (life.Health <= 0)
			{
			    anim.SetTrigger("Dead");
			    anim.SetBool("Stay Dead", true);
			    active = false;
			}
        }

		public void SlowlyResetSpriteLocation() {
			sprite.localPosition = Vector3.MoveTowards(sprite.localPosition,base.initPosition,Data.GameManager.instance.DeltaTime*5f);
		}

        #region C# Properties

        public override bool Active
        {
            get { return active; }
            set
            {
                active = value;
                if (!active) ThrowObject();
            }
        }

        /// <summary>
        /// Life component of the player
        /// </summary>
        public Life LifeComponent
        {
            get { return life; }
        }
        /// <summary>
        /// Parkour component of the player
        /// </summary>
        public Movement MovementComponent
        {
            get { return movement; }
        }
        
        /// <summary>
        /// ID of the player
        /// </summary>
        public PlayerID ID
        {
            get { return id; }
            set { id = value; }
        }

        public Util.Enums.Characters Character
        {
            get { return character; }
            set { character = value; }
        }

        public Animator Anim
        {
            get { return anim; }
        }
        #endregion
    }
}
