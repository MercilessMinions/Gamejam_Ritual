using UnityEngine;

namespace Assets.Scripts.Level
{
    public abstract class SpriteObject : MonoBehaviour
    {
        protected float heightOffGround = 0f;
        protected bool falling = false;
        protected bool active = true;
        protected float force = 0f;
        protected float vertForce = -20f;

        private float throwVel = 0f;
        private float vertVel = 0f;

        [SerializeField]
        protected Transform sprite;

		protected Vector3 initPosition;
		protected float initialHeightOffGround;
		private Vector3 initScale;
		private Vector3 respawnPosition;
		private bool setInitVals = true;

		protected bool fallingOffEdge;

        void OnEnable()
        {
            UpdateSortingLayer();
            Init();

        }

        void OnDisable()
        {
            sprite.localPosition = initPosition;
            force = 0f;
            falling = false;
        }

        void Start()
        {
            Init();
        }

        protected virtual void Init()
        {
            UpdateSortingLayer();
			if(setInitVals) {
				setInitVals = false;
				initPosition = sprite.localPosition;
				initialHeightOffGround = sprite.position.y - transform.position.y;
				initScale = transform.localScale;
			}
        }

		public void SetInitAttributes() {
			setInitVals = true;
		}

		protected virtual void Update()
        {
			if(fallingOffEdge) {
				GetComponent<SpriteRenderer>().enabled = false;
				if(transform.localScale.x > 0) {
					transform.localScale -= Vector3.one*Data.GameManager.instance.DeltaTime;
					transform.Rotate(new Vector3(0,0,Data.GameManager.instance.DeltaTime*100f));
				} else if (respawnPosition != null) {
					RespawnReset();
				}
			} else {
	            heightOffGround = sprite.position.y - transform.position.y;
				if (heightOffGround > initialHeightOffGround && falling)
	            {
	                active = false;
	                Fall();
	            }
				else if(falling && !active && heightOffGround <= initialHeightOffGround)
	            {
	                HitGround();
	            }
			}
        }

		protected void RespawnReset() {
			if(respawnPosition != null) transform.position = respawnPosition;
			UpdateSortingLayer();
			sprite.localPosition = initPosition;
			transform.localScale = initScale;
			fallingOffEdge = false;
			active = true;
			falling = true;
			transform.rotation = Quaternion.identity;
			GetComponent<SpriteRenderer>().enabled = true;
		}

        protected void Fall()
        {
            sprite.transform.Translate(-Vector2.up * 5f * Data.GameManager.instance.DeltaTime, Space.World);
            if(!Data.GameManager.instance.paused) force = Mathf.SmoothDamp(force, 0f, ref throwVel, 0.1f);
            transform.Translate(Vector2.right * force * Data.GameManager.instance.DeltaTime, Space.World);
        }

		public virtual void FellOffEdge() {
			fallingOffEdge = true;
			active = false;
		}

		public bool HasFallenOffEdge() {
			return transform.localScale.x <= 0;
		}

		public void RespawnAt(Vector3 pos) {
			respawnPosition = pos;
		}

        protected virtual void HitGround()
        {
            sprite.transform.localPosition = initPosition;
            falling = false;
            active = true;
            force = 0f;
            vertForce = 0f;
        }

        public void UpdateSortingLayer()
        {
            sprite.GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * 100);
            GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * 100 + 1);
        }

        public virtual bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public bool Falling
        {
            get { return falling; }
            set { falling = value; }
        }

        public bool FallingOffEdge
        {
            get { return fallingOffEdge; }
            set { fallingOffEdge = value; }
        }

        public float Force
        {
            get { return force; }
            set { force = value; }
        }

        public Transform Sprite
        {
            get { return sprite; }
        }
    } 
}
