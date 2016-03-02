using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Handles all the components related to in game Life
    /// such as lives, health, respawning, etc
    /// </summary>
    public class Life : ControllerObject
	{
        /// <summary>
        /// The max health a player can have
        /// </summary>
        public const float MAX_HEALTH = 100f;

        private float health = MAX_HEALTH;

		private GameObject healthTransform;

		private float respawnInvincibility;

        [SerializeField]
        private GameObject hitEffect;

        /// <summary>
        /// Modifies a player's heath
        /// </summary>
        /// <param name="delta">The amound to change (should be negative for damage)</param>
        public void ModifyHealth(float delta, bool effects = false)
        {
            health = Mathf.Clamp((health + delta), 0, MAX_HEALTH);
            if (health <= 0) Die();

            if (effects) Instantiate(hitEffect, transform.position + Vector3.up, Quaternion.identity);
        }

        // Handles when players die
        public void Deactivate()
        {
            controller.ThrowObject();
            controller.Disable();
            GameManager.instance.Respawn(controller.ID);
        }

        private void Die()
        {
            controller.ThrowObject();
            controller.Anim.SetTrigger("Dead");
            controller.Anim.SetBool("Stay Dead", true);
            controller.Disable(false);
        }

		public bool IsInvincible {
			get {
				return respawnInvincibility > 0;
			}
		}

        /// <summary>
        /// Respawns the player and clears all previous effects
        /// </summary>
        public void Respawn(bool restoreHealth = true)
        {
            //if(restoreHealth) health = MAX_HEALTH;
            controller.Enable();
			respawnInvincibility = 1f;
        }

		public void DisplayHealth() {
			if(healthTransform == null) healthTransform = transform.FindChild("HealthBG").gameObject;
			healthTransform.SetActive(true);
		}

		public void HideHealth() {
			if(healthTransform == null) healthTransform = transform.FindChild("HealthBG").gameObject;
			healthTransform.SetActive(false);
		}

		void Update() {
			if(respawnInvincibility > 0) {
				respawnInvincibility -= Data.GameManager.instance.DeltaTime;
			}
			if(healthTransform.activeSelf) {
				healthTransform.transform.GetChild(0).localScale = new Vector3((health/MAX_HEALTH),1,1);
			}
			
		}

		void Start() {
			healthTransform = transform.FindChild("HealthBG").gameObject;
		}


        #region C# Properties
        /// <summary>
        /// Health of the player
        /// </summary>
        public float Health
        {
            get { return health; }
            set { health = value; }
        }

		/// <summary>
		/// Health divided by max health of the player
		/// </summary>
		public float HealthPercentage
		{
			get { return health/MAX_HEALTH; }
		}
        #endregion
    }
}
