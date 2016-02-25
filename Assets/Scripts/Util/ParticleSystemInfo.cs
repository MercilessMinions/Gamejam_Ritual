using UnityEngine;

namespace Assets.Scripts.Util
{
    /// <summary>
    /// Handles the particle system attached to the game object
    /// </summary>
	public class ParticleSystemInfo : MonoBehaviour
	{
        // Reference to the particle system
        private ParticleSystem system;

		void OnEnable()
		{
			Data.GameManager.Pause += PauseParticles;
			Data.GameManager.Unpause += UnpauseParticles;
		}
		void OnDisable()
		{
			Data.GameManager.Pause -= PauseParticles;
			Data.GameManager.Unpause -= UnpauseParticles;
		}

        // Initialize
        void Start()
        {
            system = GetComponent<ParticleSystem>();
        }

        /// <summary>
        /// Pauses the particle system
        /// </summary>
		public void PauseParticles()
		{
			system.Pause();
		}

        /// <summary>
        /// Unpauses the particle system
        /// </summary>
		public void UnpauseParticles()
		{
            system.Play();
		}
	}
}
