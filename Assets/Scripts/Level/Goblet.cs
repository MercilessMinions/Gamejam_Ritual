using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Level
{
    public class Goblet : MonoBehaviour
    {
        private float maxHeight;

        private float vel = 0;
        private float currentHeight, targetHeight;

        public Enums.Characters character;

        [SerializeField]
        private Transform liquid;

        void Awake()
        {
            maxHeight = liquid.localScale.y;
            liquid.localScale = new Vector3(liquid.localScale.x, 0f, 1f);
        }

        void Update()
        {
            currentHeight = Mathf.SmoothDamp(currentHeight, targetHeight, ref vel, 2f);
            liquid.localScale = new Vector3(liquid.localScale.x, currentHeight, 1f);
        }

        public void UpdateScale(float ratio)
        {
            targetHeight = maxHeight * ratio;
        }
    } 
}
