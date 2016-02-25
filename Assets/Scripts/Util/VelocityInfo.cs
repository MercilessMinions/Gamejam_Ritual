using UnityEngine;
using System.Collections;

/*
 * Information about an object's rigidbody
 * Used for pausing and unpausing
 */
namespace Assets.Scripts.Util
{
    public class VelocityInfo : MonoBehaviour
    {
        //is rigidbody kinematic?
        private bool kinematic = false;

        //is rigidbody fixed angle?
        private bool fixedAngle = false;

        //is rigidbody paused?
        private bool paused = false;

        //reference to old velocity
        private Vector2 vel = Vector2.zero;
        //reference to spinning velocity
        private float angVel = 0f;

        //caching the rigidbody
        private Rigidbody2D body;

        void OnEnable()
        {
            Data.GameManager.Pause += PauseMotion;
            Data.GameManager.Unpause += UnpauseMotion;
        }
        void OnDisable()
        {
            Data.GameManager.Pause -= PauseMotion;
            Data.GameManager.Unpause -= UnpauseMotion;
        }

        void Start()
        {
            body = GetComponent<Rigidbody2D>();
        }

        //pause rigidbody
        public void PauseMotion()
        {
            kinematic = body.isKinematic;
            fixedAngle = body.constraints.Equals(RigidbodyConstraints2D.FreezeRotation);

            if (!kinematic && !fixedAngle)
            {
                //save velocity
                vel = body.velocity;
                //save angular velocity
                angVel = body.angularVelocity;

                //set fixed angle
                body.constraints = RigidbodyConstraints2D.FreezeRotation;
                //set to kinematic to pause
                body.isKinematic = true;
            }
            else if (!kinematic)
            {
                //save velocity
                vel = body.velocity;
                //set to kinematic to pause
                body.isKinematic = true;
            }

            //pause
            paused = true;
        }

        public void UnpauseMotion()
        {
            if (!kinematic && !fixedAngle)
            {
                //set to not kinematic to unpause
                body.isKinematic = false;

                //set to not fixed angle
                body.constraints = RigidbodyConstraints2D.None;
                //reapply angular velocity
                body.angularVelocity = angVel;
                //reapply velocity
                body.velocity = vel;

                //reset reference
                angVel = 0f;
                //reset reference
                vel = Vector2.zero;
            }
            else if (!kinematic)
            {
                //set to not kinematic to unpause
                body.isKinematic = false;
                //reapply velocity
                body.velocity = vel;
                //reset reference
                vel = Vector2.zero;
            }

            //unpause
            paused = false;
        }

        public bool Paused
        {
            get { return paused; }
        }

        public Vector2 Vel
        {
            get { return vel; }
        }

        public float AngVel
        {
            get { return angVel; }
        }
    }
}