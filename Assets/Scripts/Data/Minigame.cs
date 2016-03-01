using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Data
{
    public abstract class Minigame : MonoBehaviour
    {
		public bool finished = false;
        private List<PlayerID> winners;
        [SerializeField]
        protected float timer = 0;
		public Sprite instructions;

        public abstract void Init();
        public abstract void Run();
        public abstract void ForceEnd();

        #region C# Properties
        public List<PlayerID> Winners
        {
            get { return winners; }
            set { winners = value; }
        }
        #endregion
    }
}