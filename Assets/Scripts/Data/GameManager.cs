using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Level;
using Assets.Scripts.Player;
using Assets.Scripts.Timers;
using Assets.Scripts.Util;
using Assets.Scripts.Level;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Data
{
    /// <summary>
    /// Manager to control everything as the top of the pyramid
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Delegate event
        /// </summary>
        /// <param name="t"></param>
        public delegate void PausingEvent();
        /// <summary>
        /// The event for pausing
        /// </summary>
        public static event PausingEvent Pause;
        /// <summary>
        /// The event for unpausing
        /// </summary>
        public static event PausingEvent Unpause;

        /// <summary>
        /// Use a singleton instance to make sure there is only one
        /// </summary>
        public static GameManager instance;

        [SerializeField]
        private GameObject[] players;

        // List of all the controllers of the players
        private List<Controller> controllers;
        private List<RespawnNode> respawnNodes;
        private List<Goblet> goblets;

        private Minigame currentGame;
        [SerializeField]
        private List<Minigame> games;
        private List<Minigame> gamesQueue;

        private int[] playerScores = new int[4];

        public const int MAX_SCORE = 100;
        private int numGames = 5;
        private int pointStep = 20;
		private bool transitionStarted = false;
		private bool scoreAdded = false;
		private float demandingTimer = 1f, transitionTimer2 = 0f;

        public float DeltaTime = 0;

        public bool inGame = false;
        public bool paused = false;

        public GameObject field;

		private GameObject dialogHolder;

		private Enums.Characters winningCharacter;
		private bool gameWon;
		private float winTimer;
		private GameObject winningCharacterSprite, winText;

		private float veryFirstTimer = 4f;
		
        private Dictionary<Enums.Characters, PlayerID> characterToPlayer;

        // Sets up singleton instance. Will remain if one does not already exist in scene
        void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            controllers = new List<Controller>();
            respawnNodes = new List<RespawnNode>();
            characterToPlayer = new Dictionary<Enums.Characters, PlayerID>();
            goblets = new List<Goblet>();
            gamesQueue = new List<Minigame>();
        }

        void OnLevelWasLoaded(int i)
        {
            if (SceneManager.GetActiveScene().name == "Eric Test2") StartGame();
        }

        public void StartGame()
        {
            inGame = true;

            RespawnNode[] findNodes = FindObjectsOfType<RespawnNode>();
            for (int i = 0; i < findNodes.Length; i++)
            {
                respawnNodes.Add(findNodes[i]);
            }

            Goblet[] findGoblets = FindObjectsOfType<Goblet>();
            for (int i = 0; i < findGoblets.Length; i++)
            {
                goblets.Add(findGoblets[i]);
            }

			dialogHolder = GameObject.Find("DialogHolder");

			field = GameObject.Find("Field");

            RefillGames();
            currentGame = gamesQueue[0];
            gamesQueue.RemoveAt(0);

            for (int i = 0; i < controllers.Count; i++)
            {
                controllers[i].Enable();
                RespawnNode playerNode = respawnNodes.Find(x => x.ID.Equals(controllers[i].ID));
                controllers[i].transform.position = playerNode.transform.position;
				controllers[i].SetInitAttributes();
            }
                
			currentGame.Init();
			veryFirstTimer = 4f;
			Camera.main.GetComponent<Animator>().SetTrigger("GodDemands");
			dialogHolder.GetComponent<SpriteRenderer>().sprite = currentGame.instructions;

			winningCharacterSprite = GameObject.Find("WinningCharacterSprite");
			winText = GameObject.Find("WinText");

        }

		public void ResetGame() {
			Camera.main.GetComponent<Animator>().SetTrigger("RestartGame");

			for(int i = 0; i < 4; i++) {
				playerScores[i] = 0;
			}
			foreach(PlayerID id in characterToPlayer.Values) {
				Enums.Characters c = characterToPlayer.FirstOrDefault(x => x.Value == id).Key;
				Goblet g = goblets.Find(x => x.character.Equals(c));
				g.UpdateScale(Mathf.Clamp01(((float)playerScores[((int)id) - 1]) / (float)MAX_SCORE));
			}

			inGame = true;

			for (int i = 0; i < controllers.Count; i++)
			{
				controllers[i].Enable();
				RespawnNode playerNode = respawnNodes.Find(x => x.ID.Equals(controllers[i].ID));
				controllers[i].transform.position = playerNode.transform.position;
			}

		}

        void Update()
        {
            if (inGame)
            {
                if(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.Start, PlayerID.One))
                {
                    if (!paused)
                    {
                        paused = true;
                        if (Pause != null) Pause();
                    }
                    else
                    {
                        paused = false;
                        if (Unpause != null) Unpause();
                    }
                }

                if (paused) DeltaTime = 0;
                else DeltaTime = Time.deltaTime;

                if (!currentGame.finished && !transitionStarted)
                {
					veryFirstTimer -= DeltaTime;
					if(veryFirstTimer <= 0) {
                    	currentGame.Run();
					}

                }
                else
                {

					if(!transitionStarted) {
						transitionStarted = true;
						if(IncrementPlayerScore(currentGame.Winners)) {
							inGame = false;
							gameWon = true;
							winTimer = 3f;
						}
						demandingTimer = 5f;
						transitionTimer2 = 15f;
                        if (gamesQueue.Count == 0) RefillGames();
						currentGame = gamesQueue[0];
                        gamesQueue.RemoveAt(0);
					} else {
						demandingTimer -= DeltaTime;

						if(demandingTimer <= 0) {
							transitionTimer2 = 5f;
							demandingTimer = 10f;
							Camera.main.GetComponent<Animator>().SetTrigger("GodDemands");
							SFXManager.instance.source.PlayOneShot(SFXManager.instance.RandomGodTalk());
							dialogHolder.GetComponent<SpriteRenderer>().sprite = currentGame.instructions;
						}

						transitionTimer2 -= DeltaTime;

						if(transitionTimer2 <= 0) {
							currentGame.Init();
							transitionStarted = false;
						}
					}
                }
				dialogHolder.transform.localScale = Vector3.one*Mathf.Max(1.5f,(Mathf.Sin(Time.time*8f)+1));
			} else if(gameWon) {
				winTimer -= DeltaTime;
				if(winTimer <= 0) {
					gameWon = false;
					Camera.main.GetComponent<Animator>().SetTrigger("GameEnd");
					for(int i = 0; i < controllers.Count; i++) {
						if(controllers[i].Character == winningCharacter) {
							winningCharacterSprite.GetComponent<Image>().sprite = controllers[i].Sprite.GetComponent<SpriteRenderer>().sprite;
							winText.GetComponent<Text>().text = "Congratulations! " + controllers[i].Character + " Was Sacrificed!";
							break;
						}
					}
				}
			}
        }

        private bool IncrementPlayerScore(List<PlayerID> winners)
        {
            bool scoreReached = false;
            foreach(PlayerID id in winners)
            {
                playerScores[((int)id)-1] += pointStep;
                Enums.Characters c = characterToPlayer.FirstOrDefault(x => x.Value == id).Key;
                Goblet g = goblets.Find(x => x.character.Equals(c));
				g.UpdateScale(Mathf.Clamp01(((float)playerScores[((int)id) - 1]) / (float)MAX_SCORE));
				if (playerScores[((int)id)-1] >= MAX_SCORE) {
					scoreReached = true;
					winningCharacter = c;
				}
            }
            return scoreReached;
        }

        /// <summary>
        /// Sets up the player so it can respawn.
        /// </summary>
        /// <param name="id">The ID of the player that died</param>
        public void Respawn(PlayerID id)
        {
            // Find the dead player
            Controller deadPlayer = controllers.Find(x => x.ID.Equals(id));
            if (deadPlayer != null)
            {
                // Initialize the respawn timer
                // Initialize the respawn timer
                CountdownTimer t = gameObject.AddComponent<CountdownTimer>();
                t.Initialize(3f, deadPlayer.ID.ToString());
                t.TimeOut += new CountdownTimer.TimerEvent(ResawnHelper);

            }
        }

        private void ResawnHelper(CountdownTimer t)
        {
            // Find the dead player again
            Controller deadPlayer = controllers.Find(x => x.ID.Equals(System.Enum.Parse(typeof(PlayerID), t.ID)));
            //RespawnNode playerNode = respawnNodes.Find(x => x.ID.Equals(System.Enum.Parse(typeof(PlayerID), t.ID)));
            RespawnNode playerNode = respawnNodes[Random.Range(0, respawnNodes.Count)];
            if (deadPlayer != null)
            {
                deadPlayer.transform.position = playerNode.transform.position;
                // Let the player revive itself
                if (currentGame.GetType().Equals(typeof(MeleeMinigame))) deadPlayer.LifeComponent.Respawn(false);
				else if (currentGame.GetType().Equals(typeof(CaymanGame))) deadPlayer.LifeComponent.Respawn(false);
                else deadPlayer.LifeComponent.Respawn();
            }
        }


        public void InitializePlayer(Enums.Characters character, PlayerID id)
        {
            GameObject newPlayer = Instantiate(players[(int)character]);
            DontDestroyOnLoad(newPlayer);
            Controller controller = newPlayer.GetComponent<Controller>();
            controller.ID = id;
            controller.Character = character;
            controllers.Add(controller);
            controller.Disable();
            characterToPlayer.Add(character, id);
            
        }

        public void RemovePlayer(Enums.Characters character)
        {
            Controller removePlayer = controllers.Find(x => x.ID.Equals(characterToPlayer[character]));
            controllers.Remove(removePlayer);
            characterToPlayer.Remove(character);
        }

		public void RemoveAllPlayers() {
			for(int i = 0; i < controllers.Count; i++) {
				Destroy(controllers[i].gameObject);
			}
			controllers = new List<Controller>();
			characterToPlayer = new Dictionary<Enums.Characters, PlayerID>();

		}

        private void RefillGames()
        {
            gamesQueue = games.ToList();
            for (int i = gamesQueue.Count - 1; i >= 0; i--)
            {
                int j = Random.Range(0, gamesQueue.Count);
                Minigame m = gamesQueue[i];
                gamesQueue[i] = gamesQueue[j];
                gamesQueue[j] = m;
            }
        }

#region C# Properties
        public List<Controller> AllPlayers
        {
            get { return controllers; }
        }
        public Dictionary<Enums.Characters, PlayerID> CharacterToPlayer
        {
            get { return characterToPlayer; }
        }
        #endregion
    }
}
