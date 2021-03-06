﻿using UnityEngine;
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
            Destroy(gameObject);
        }

        private void EndGame()
        {
            GetComponentInChildren<Collider2D>().enabled = false;
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.transform.root.tag.Equals("Player") || col.transform.root.tag.Equals("Baskets"))
            {
                if (Mathf.Abs(GetComponent<SpriteRenderer>().sortingOrder - col.GetComponent<SpriteRenderer>().sortingOrder) < 100)
                {
                    Basket b = col.GetComponentInParent<Basket>();
                    if (b == null) return;
                    Util.Enums.Characters character = b.Character;
                    game.FishCaught(GameManager.instance.CharacterToPlayer[character]);
                    Instantiate(splash, sprite.position + Vector3.down, Quaternion.Euler(new Vector3(-90, 0, 0)));
                    Destroy(gameObject);
                }
            }
        }

        public FishMinigame Game
        {
            set { game = value; }
        }
    }
}