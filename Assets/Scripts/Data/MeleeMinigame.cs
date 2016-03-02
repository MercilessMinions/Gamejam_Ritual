using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Data
{
    public class MeleeMinigame : Minigame
    {
        private int numAlive;
        public override void Init()
        {
			finished = false;
            numAlive = GameManager.instance.AllPlayers.Count;
            Winners = new List<PlayerID>();
            for(int i = 0; i < GameManager.instance.AllPlayers.Count; i++)
            {
                GameManager.instance.AllPlayers[i].MovementComponent.MeleeEnabled = true;
				GameManager.instance.AllPlayers[i].LifeComponent.Health = 100;
				GameManager.instance.AllPlayers[i].LifeComponent.DisplayHealth();
				GameManager.instance.AllPlayers[i].Anim.SetBool("Stay Dead", false);
				GameManager.instance.AllPlayers[i].Active = true;
            }
        }
        public override void Run()
        {
			if (numAlive == 1)
            {
                for (int i = 0; i < GameManager.instance.AllPlayers.Count; i++)
                {
                    if (GameManager.instance.AllPlayers[i].LifeComponent.Health > 0) Winners.Add(GameManager.instance.AllPlayers[i].ID);
                    finished = true;
					GameManager.instance.AllPlayers[i].MovementComponent.MeleeEnabled = false;
					GameManager.instance.AllPlayers[i].LifeComponent.HideHealth();
                }
            }
			if (numAlive == 0) {
				finished = true;
				for (int i = 0; i < GameManager.instance.AllPlayers.Count; i++)
				{
					GameManager.instance.AllPlayers[i].MovementComponent.MeleeEnabled = false;
					GameManager.instance.AllPlayers[i].LifeComponent.HideHealth();
				}
			}
            else
            {
                int temp = 0;
                for (int i = 0; i < GameManager.instance.AllPlayers.Count; i++)
                {
                    if (GameManager.instance.AllPlayers[i].LifeComponent.Health > 0) temp++;
                }
                numAlive = temp;
            }
        }

        public override void ForceEnd()
        {
            for (int i = 0; i < GameManager.instance.AllPlayers.Count; i++)
            {
                GameManager.instance.AllPlayers[i].LifeComponent.Health = 100f;
                GameManager.instance.AllPlayers[i].MovementComponent.MeleeEnabled = false;
				GameManager.instance.AllPlayers[i].LifeComponent.HideHealth();
            }
        }
    }
}
