using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Data
{
    public class CaymanGame : Minigame
    {
        [SerializeField]
        private GameObject cayman;

        private GameObject caymanTemp;

        public override void Init()
        {
			finished = false;

			Winners = new System.Collections.Generic.List<PlayerID>();

			for(int i = 0; i < GameManager.instance.AllPlayers.Count; i++)
			{
				GameManager.instance.AllPlayers[i].LifeComponent.Health = 100;
				GameManager.instance.AllPlayers[i].Anim.SetBool("Stay Dead", false);
				GameManager.instance.AllPlayers[i].Active = true;
			}
        }

        public override void Run()
        {
			if(!caymanTemp) {
				caymanTemp = (GameObject) Instantiate(cayman, new Vector3(0, -7, 0), Quaternion.identity);
			} else {
	            if (caymanTemp.GetComponent<Monster>().winners.Count <= 1)
	            {
	                finished = true;
	                //if (!caymanTemp.GetComponent<Monster>().losers.contains(1))
	                Winners.Add(caymanTemp.GetComponent<Monster>().winners[0]);
	                Destroy(caymanTemp);
	            }
			}
        }
    }
}
