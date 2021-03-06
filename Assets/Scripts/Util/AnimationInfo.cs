﻿using UnityEngine;
using Assets.Scripts.Data;

namespace Assets.Scripts.Util
{
    /// <summary>
    /// Handles the animation component for events like pausing
    /// </summary>
	public class AnimationInfo : MonoBehaviour
	{
        // Speed of the animator to save
		private float speed = 0f;

        // Reference to the animator
        private Animator anim;

		void OnEnable()
		{
			Data.GameManager.Pause += PauseAnimator;
			Data.GameManager.Unpause += UnpauseAnimator;
		}
		void OnDisable()
		{
			Data.GameManager.Pause -= PauseAnimator;
			Data.GameManager.Unpause -= UnpauseAnimator;
		}

        // Initialize
        void Start()
        {
            anim = GetComponent<Animator>();
        }

		void Update()
		{
			anim.speed = GameManager.instance.DeltaTime/Time.deltaTime;
		}
		
        /// <summary>
        /// Pausing the anumator
        /// </summary>
		public void PauseAnimator()
		{
			speed = anim.speed;
			anim.speed = 0;
		}
		
        /// <summary>
        /// Unpausing the animator
        /// </summary>
		public void UnpauseAnimator()
		{
			anim.speed = speed;
		}
	}
}
