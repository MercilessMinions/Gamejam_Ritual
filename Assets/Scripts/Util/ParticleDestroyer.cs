using UnityEngine;

namespace Assets.Scipts.Util
{
    public class ParticleDestroyer : MonoBehaviour
    {
        public float destroyTime = 1f;
        private float timer = 0;

        void Update()
        {
            timer += Scripts.Data.GameManager.instance.DeltaTime;
            if (timer >= destroyTime) Destroy(gameObject);
        }
    } 
}
