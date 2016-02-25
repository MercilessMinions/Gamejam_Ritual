using UnityEngine;
using Assets.Scripts.Level;
using Assets.Scripts.Data;
using Assets.Scripts.Player;

namespace Assets.Scripts.Level
{
    public class Fish : SpriteObject
    {
        private FishMinigame game;
        [SerializeField]
        private GameObject splash;

        void OnEnable()
        {
            FishMinigame.EndGame += EndGame;
        }
        void OnDisable()
        {
            FishMinigame.EndGame -= EndGame;
        }

        protected override void Init()
        {
            base.Init();
            falling = true;
			sprite.localPosition = Vector3.up*20f; 
        }
        protected override void HitGround()
        {
            base.HitGround();
            Destroy(transform.root.gameObject);
        }

        private void EndGame()
        {
            GetComponentInChildren<Collider2D>().enabled = false;
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.transform.root.tag.Equals("Player"))
            {
                if (Mathf.Abs(GetComponent<SpriteRenderer>().sortingOrder - col.GetComponent<SpriteRenderer>().sortingOrder) < 100)
                {
                    Basket b = col.GetComponent<Basket>();
                    if (b == null) return;
                    Util.Enums.Characters character = col.GetComponent<Basket>().Character;
                    game.FishCaught(GameManager.instance.CharacterToPlayer[character]);
                    Instantiate(splash, sprite.position + Vector3.down, Quaternion.Euler(new Vector3(-90, 0, 0)));
                    Destroy(transform.root.gameObject);
                }
            }
        }

        public FishMinigame Game
        {
            set { game = value; }
        }
    }
}