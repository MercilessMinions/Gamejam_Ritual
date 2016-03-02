using UnityEngine;
using Assets.Scripts.Level;
using System.Collections.Generic;

namespace Assets.Scripts.Data
{
	public class FireMinigame : Minigame {

		[SerializeField]
		private GameObject logs;

		[SerializeField]
		private GameObject[] firePlaces;

		private List<Fireplace> inGameFirePlaces;

		private List<GameObject> inGameLogs;

		private float winnerFound = 2f;


		public override void Init ()
		{
			finished = false;
			Winners = new System.Collections.Generic.List<PlayerID>();

			inGameFirePlaces = new List<Fireplace>();
			inGameLogs = new List<GameObject>();

			int numPlayers = GameManager.instance.AllPlayers.Count;

			if(GameManager.instance.CharacterToPlayer.ContainsKey(Assets.Scripts.Util.Enums.Characters.Opochtli)) {
				inGameFirePlaces.Add(((GameObject)GameObject.Instantiate(firePlaces[0], new Vector2(-5,1), Quaternion.identity)).GetComponent<Fireplace>());
			}
			if(GameManager.instance.CharacterToPlayer.ContainsKey(Assets.Scripts.Util.Enums.Characters.Zolin)) {
				inGameFirePlaces.Add(((GameObject)GameObject.Instantiate(firePlaces[1], new Vector2(-5,-3), Quaternion.identity)).GetComponent<Fireplace>());
			}
			if(GameManager.instance.CharacterToPlayer.ContainsKey(Assets.Scripts.Util.Enums.Characters.Yaotl)) {
				inGameFirePlaces.Add(((GameObject)GameObject.Instantiate(firePlaces[2], new Vector2(5,1), Quaternion.identity)).GetComponent<Fireplace>());
			}
			if(GameManager.instance.CharacterToPlayer.ContainsKey(Assets.Scripts.Util.Enums.Characters.Coatl)) {
				inGameFirePlaces.Add(((GameObject)GameObject.Instantiate(firePlaces[3], new Vector2(5,-3), Quaternion.identity)).GetComponent<Fireplace>());
			}

			for(int i = 0; i < 20; i++) {
				inGameLogs.Add((GameObject)GameObject.Instantiate(logs, new Vector2(Random.Range(-3,3), Random.Range(1f,-4f)), Quaternion.identity));
			}

			for(int i = 0; i < GameManager.instance.AllPlayers.Count; i++)
			{
				GameManager.instance.AllPlayers[i].LifeComponent.Health = 100;
				GameManager.instance.AllPlayers[i].Anim.SetBool("Stay Dead", false);
				GameManager.instance.AllPlayers[i].Active = true;
			}
		}

		public override void Run ()
		{
			int numDead = 0;
			Fireplace survivor = null;
			foreach(Fireplace f in inGameFirePlaces) {
				if(f.Dead) numDead++;
				else survivor = f;
			}

			if(numDead >= GameManager.instance.AllPlayers.Count - 1) {
				winnerFound -= Data.GameManager.instance.DeltaTime;
				if(winnerFound < 0) {
					for(int i = 0; i < GameManager.instance.AllPlayers.Count; i++) {
						GameManager.instance.AllPlayers[i].ThrowObject();
					}

					for(int i = 0; i < inGameLogs.Count; i++) {
						Destroy(inGameLogs[i].gameObject);
					}
					for(int i = 0; i < inGameFirePlaces.Count; i++) {
						if(inGameFirePlaces[i] != null) Destroy(inGameFirePlaces[i].gameObject);
					}
					if(survivor)
                        Winners.Add(GameManager.instance.CharacterToPlayer[survivor.linkedCharacter]);
					finished = true;
					winnerFound = 2f;
				}
			}
		}

        public override void ForceEnd()
        {
            for (int i = 0; i < inGameLogs.Count; i++)
            {
                Destroy(inGameLogs[i].gameObject);
            }
            for (int i = 0; i < inGameFirePlaces.Count; i++)
            {
				if(inGameFirePlaces[i] != null) Destroy(inGameFirePlaces[i].gameObject);
            }
            winnerFound = 2f;
        }
    }
}