using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Level
{
    public class RespawnNode : MonoBehaviour
    {
        [SerializeField]
        private PlayerID id;

        public PlayerID ID
        {
            get { return id; }
        }
    }
}
