using UnityEngine;
using System.Collections;
using Assets.Scripts.Player;
using Assets.Scripts.Data;

namespace Assets.Scripts.Level
{
    public class MeleeHitbox : MonoBehaviour
    {
        private int damage = -10;
        void OnTriggerEnter2D(Collider2D col)
        {
            if(col.tag.Equals("Player"))
            {
				SFXManager.instance.source.PlayOneShot(SFXManager.instance.RandomJab());
                Controller controller = col.GetComponent<Controller>();
				GameManager.instance.SleepGame(0.05f);
                controller.LifeComponent.ModifyHealth(damage, true);
				if (transform.root.GetComponent<Controller>().MovementComponent.FacingRight) controller.MovementComponent.InitRoll(1);
				else controller.MovementComponent.InitRoll(-1);
				Camera.main.GetComponent<CameraShake>().ScreenShake(1f,transform.root.GetComponent<Controller>().MovementComponent.FacingRight);
            }
        }
    } 
}
