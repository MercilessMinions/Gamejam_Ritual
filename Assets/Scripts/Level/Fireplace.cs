using UnityEngine;
using System.Collections;
using Assets.Scripts.Player;
using Assets.Scripts.Util;

namespace Assets.Scripts.Level
{
	public class Fireplace : MonoBehaviour {

		private float fuel = 10f;
		public Enums.Characters linkedCharacter;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			fuel -= Data.GameManager.instance.DeltaTime;
			if(fuel > 8f) {
				GetComponent<Animator>().SetInteger("FireLevel", 3);
			} else if (fuel > 6f) {
				GetComponent<Animator>().SetInteger("FireLevel", 2);
			} else if (fuel > 4f) {
				GetComponent<Animator>().SetInteger("FireLevel", 1);
			} else if (fuel <= 0f) {
				transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
				GetComponent<Animator>().SetInteger("FireLevel", 0);
			} 
		}

		void OnTriggerEnter2D(Collider2D col) {
			if(fuel > 0 && col.GetComponent<Tinder>()) {
				Destroy(col.gameObject);
				transform.GetChild(0).GetComponent<ParticleSystem>().Emit(200);
				transform.GetChild(0).GetComponent<ParticleSystem>().Play();
				fuel += 5f;
				SFXManager.instance.source.PlayOneShot(SFXManager.instance.fireBurst);
			}
			if(fuel > 0 && col.GetComponent<PlayerController>()) {
				col.GetComponent<PlayerController>().LifeComponent.Deactivate();
				transform.GetChild(0).GetComponent<ParticleSystem>().Emit(400);
				transform.GetChild(0).GetComponent<ParticleSystem>().Play();
				fuel += 2f;
				SFXManager.instance.source.PlayOneShot(SFXManager.instance.fireBurst);
			}
			
		}

		public bool Dead {
			get {
				return fuel <= 0;
			}
		}
	}
}
