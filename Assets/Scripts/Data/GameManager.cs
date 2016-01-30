﻿using UnityEngine;
using System.Collections.Generic;
//using Assets.Scripts.Level;
using Assets.Scripts.Player;
//using Assets.Scripts.Timers;
//using Assets.Scripts.Util;
using TeamUtility.IO;

namespace Assets.Scripts.Data
{
    /// <summary>
    /// Manager to control everything as the top of the pyramid
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Use a singleton instance to make sure there is only one
        /// </summary>
        public static GameManager instance;

        // List of all the controllers of the players
        private List<Controller> controllers;

        [SerializeField]
        private GameObject playerPrefab;

        private Minigame currentGame;
        [SerializeField]
        private List<Minigame> games;

        private int[] playerScores = new int[4];

        public const int MAX_SCORE = 100;
        private int numGames = 5;

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
            //games = new List<Minigame>();
            controllers = new List<Controller>();
        }

        void Start()
        {
            Controller[] findControllers = FindObjectsOfType<Controller>();
            for (int i = 0; i < findControllers.Length; i++)
            {
                controllers.Add(findControllers[i]);
            }
            currentGame = games[Random.Range(0, games.Count)];
        }

        void Update()
        {
            if(!currentGame.finished)
            {
                currentGame.Run();
            }
            else
            {
                if(!IncrementPlayerScore(currentGame.Winners))
                {
                    // pick a new game
                    currentGame = games[Random.Range(0, games.Count)];
                }

                //startInBetweenAnimation
            }
        }

        private bool IncrementPlayerScore(List<PlayerID> winners)
        {
            bool scoreReached = false;
            foreach(PlayerID id in winners)
            {
                playerScores[(int)id] += MAX_SCORE / numGames;
                if (playerScores[(int)id] >= MAX_SCORE) scoreReached = true;
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
                // Find an appropriate spawning pod (set to default for now)
                deadPlayer.transform.position = Vector3.zero;
                // Let the player revive itself
                deadPlayer.LifeComponent.Respawn();
            }
        }


        public void InitializePlayer(PlayerID id)
        {
            GameObject newPlayer = Instantiate(playerPrefab);
            Controller controller = newPlayer.GetComponent<Controller>();
            controller.ID = id;
            controllers.Add(controller);
            controller.Disable();
        }

        public void RemovePlayer(PlayerID id)
        {
            Controller removePlayer = controllers.Find(x => x.ID.Equals(id));
            controllers.Remove(removePlayer);
        }

        #region C# Properties

        #endregion
    }
}
