﻿using UnityEngine;

namespace Assets.Scripts.Player
{
	public class Movement : ControllerObject {

		private bool facingRight = true;
        private bool rolling = false;
        private float rollTimer = 0, rollTime = 0.3f;
        private float moveSpeed = 5f;
        private float rollSpeed = 20f, rollSlerp = 20f, targetSlerp = 5f, slerpSpeed = 0.1f;
        private float rotationSpeed = 0.1f, rotationVel = 0f, targetZRotation = 360, currentZRotation = 0f;
        private float rollVel = 0f;
        private int rollDir = 1;
        private bool meleeEnabled = false;

        void OnDisable()
        {
            Reset();
        }

        void Update()
        {
            if (Data.GameManager.instance.inGame)
                {
                controller.Anim.SetBool("Club", meleeEnabled);
                if (rolling)
                {
                    rollTimer += Time.deltaTime;
                    rollSlerp = Mathf.SmoothDamp(rollSlerp, targetSlerp, ref rollVel, slerpSpeed);
                    if (rollTimer >= rollTime)
                    {
                        Reset();
                    }
                    else
                    {
                        Roll(rollDir);
                    }
                }
            }
        }

        public void MoveHorizontal(int dir, float ratio = 1f)
        {
            if (rolling) return;
            if (dir < 0)
            {
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime * ratio, Space.World);
                facingRight = false;
            }
            else
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime * ratio, Space.World);
                facingRight = true;
            }
            controller.UpdateSortingLayer();
        }
        public void MoveVertical(int dir, float ratio = 1f)
        {
            if (rolling) return;
            if (dir < 0)
            {
                transform.Translate(Vector3.down * moveSpeed * Time.deltaTime * ratio, Space.World);
            }
            else
            {
                transform.Translate(Vector3.up * moveSpeed * Time.deltaTime * ratio, Space.World);
            }
            controller.UpdateSortingLayer();
        }

        public void InitRoll(int dir)
        {
            rolling = true;
            targetZRotation = -dir * 360;
            rollDir = dir;
        }
        private void Roll(int dir)
        {
            transform.Translate(dir * Vector3.right * rollSlerp * Time.deltaTime, Space.World);

            currentZRotation = Mathf.SmoothDamp(currentZRotation, targetZRotation, ref rotationVel, rotationSpeed);
            controller.Sprite.transform.rotation = Quaternion.Euler(0, 0, currentZRotation);
        }

        public void Reset()
        {
            if (!Data.GameManager.instance.inGame) return;
            rollTimer = 0;
            rolling = false;
            rollSlerp = rollSpeed;
            currentZRotation = 0f;
            controller.Sprite.transform.rotation = Quaternion.Euler(0, 0, currentZRotation);
            controller.Anim.SetBool("Rolling", false);
            controller.Anim.SetBool("Carry", false);
        }

        public float MoveSpeed
        {
            get { return moveSpeed; }
            set { moveSpeed = value; }
        }

        public bool Rolling
        {
            get { return rolling; }
        }

        public bool FacingRight
        {
			get { return facingRight; }
		}

        public bool MeleeEnabled
        {
            get { return meleeEnabled; }
            set
            { meleeEnabled = value; }
        }
	}
}