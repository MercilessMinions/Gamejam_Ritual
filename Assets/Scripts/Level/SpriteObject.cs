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

        private Vector3 initPosition;
		private Vector3 initScale;
		private Vector3 respawnPosition;

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
            initPosition = sprite.localPosition;
			initScale = transform.localScale;
        }

        public void Update()
        {
			if(fallingOffEdge) {
				GetComponent<SpriteRenderer>().enabled = false;
				if(transform.localScale.x > 0) {
					transform.localScale -= Vector3.one*Time.deltaTime;
					transform.Rotate(new Vector3(0,0,Time.deltaTime*100f));
				} else if (respawnPosition != null) {
					transform.position = respawnPosition;
					transform.localScale = initScale;
					fallingOffEdge = false;
					active = true;
					transform.rotation = Quaternion.identity;
					force = 0;
					vertForce = 0;
					GetComponent<SpriteRenderer>().enabled = true;
				}
			} else {
	            heightOffGround = sprite.position.y - transform.position.y;
				if (heightOffGround > 0.6f && falling)
	            {
	                active = false;
	                Fall();
	            }
				else if(falling && !active && heightOffGround <= 0.6f)
	            {
	                HitGround();
	            }
			}
        }

        protected void Fall()
        {
            sprite.transform.Translate(-Vector2.up * 5f * Time.deltaTime, Space.World);
                //vertForce = Mathf.SmoothDamp(vertForce, 5f, ref vertVel, 0.1f);
                //sprite.transform.Translate(-Vector2.up * vertForce * Time.deltaTime, Space.World);
            force = Mathf.SmoothDamp(force, 0f, ref throwVel, 0.1f);
            transform.Translate(Vector2.right * force * Time.deltaTime, Space.World);
        }

		public virtual void FellOffEdge() {
			fallingOffEdge = true;
			active = false;
		}

		public bool HasFallenOffEdge() {
			if(gameObject.name.Contains("Blue Basket"))
				Debug.Log(transform.localScale.x);
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
