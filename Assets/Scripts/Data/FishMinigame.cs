using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Level;
using Assets.Scripts.Timers;

namespace Assets.Scripts.Data
{
    public class FishMinigame : Minigame
    {
        [SerializeField]
        private GameObject fishPrefab;
        RepetitionTimer spawnTimer;

        private int[] fishCaught;
        private int targetFish = 5;

        [SerializeField]
        private GameObject[] baskets;
        private List<Basket> inGameBaskets;

        public delegate void EndGameEvent();
        public static event EndGameEvent EndGame;
        
        public override void Init()
        {

            inGameBaskets = new List<Basket>();
            finished = false;
            spawnTimer = gameObject.AddComponent<RepetitionTimer>();
            spawnTimer.Initialize(0.5f, "Fish Spawner");
            spawnTimer.TimeOut += new RepetitionTimer.TimerEvent(SpawnFish);
            spawnTimer.FinalTick += new RepetitionTimer.TimerEvent(SpawnFish);
            fishCaught = new int[4];
            Winners = new System.Collections.Generic.List<PlayerID>();

			for(int i = 0; i < GameManager.instance.AllPlayers.Count; i++)
			{
				GameManager.instance.AllPlayers[i].LifeComponent.Health = 100;
				GameManager.instance.AllPlayers[i].Anim.SetBool("Stay Dead", false);
				GameManager.instance.AllPlayers[i].Active = true;
			}

            if (GameManager.instance.CharacterToPlayer.ContainsKey(Util.Enums.Characters.Opochtli))
            {
                Basket b = ((GameObject)Instantiate(baskets[0], new Vector2(-5, 1), Quaternion.identity)).GetComponent<Basket>();
				b.RespawnAt(new Vector2(-5,1));
                b.Character = Util.Enums.Characters.Opochtli;
                inGameBaskets.Add(b);
            }
            if (GameManager.instance.CharacterToPlayer.ContainsKey(Util.Enums.Characters.Zolin))
            {
                Basket b = ((GameObject)Instantiate(baskets[1], new Vector2(-5, -3), Quaternion.identity)).GetComponent<Basket>();
				b.RespawnAt(new Vector2(-5,-3));
                b.Character = Util.Enums.Characters.Zolin;
                inGameBaskets.Add(b);
            }
            if (GameManager.instance.CharacterToPlayer.ContainsKey(Util.Enums.Characters.Yaotl))
            {
                Basket b = ((GameObject)Instantiate(baskets[2], new Vector2(5, 1), Quaternion.identity)).GetComponent<Basket>();
				b.RespawnAt(new Vector2(5,1));
                b.Character = Util.Enums.Characters.Yaotl;
                inGameBaskets.Add(b);
            }
            if (GameManager.instance.CharacterToPlayer.ContainsKey(Util.Enums.Characters.Coatl))
            {
                Basket b = ((GameObject)Instantiate(baskets[3], new Vector2(5, -3), Quaternion.identity)).GetComponent<Basket>();
				b.RespawnAt(new Vector2(5,-3));
                b.Character = Util.Enums.Characters.Coatl;
                inGameBaskets.Add(b);
            }
        }

        private void SpawnFish(RepetitionTimer t)
        {
            Bounds b = GameManager.instance.field.GetComponent<Collider2D>().bounds;
            Vector2 pos = new Vector2(Random.Range(b.min.x, b.max.x), Random.Range(b.min.y, b.max.y));
            GameObject fish = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            fish.GetComponent<Fish>().Game = this;
        }

        public override void Run(){}

        public void FishCaught(PlayerID id)
        {
            fishCaught[((int)id)-1]++;
            if(fishCaught[((int)id)-1] >= targetFish)
            {
                finished = true;
                Destroy(spawnTimer);
                Fish[] allFish = FindObjectsOfType<Fish>();
                for(int i = 0; i < allFish.Length; i++)
                {
                    Destroy(allFish[i].transform.root.gameObject);
                }
                Winners.Add(id);
                if (EndGame != null) EndGame();
                for(int i = 0; i < inGameBaskets.Count; i++)
                {
                    GameManager.instance.AllPlayers.Find(x => x.Character.Equals(inGameBaskets[i].Character)).ThrowObject();
					inGameBaskets[i].transform.parent = null;
                    Destroy(inGameBaskets[i].gameObject);
                }
            }
        }

        public override void ForceEnd()
        {
            Destroy(spawnTimer);
            if (EndGame != null) EndGame();
        }
    }
}
