using UnityEngine;
using Assets.Scripts.Player;

namespace Assets.Scripts.Level
{
    public class Field : MonoBehaviour
    {

        void OnTriggerExit2D(Collider2D col)
        {
            Debug.Log(col.transform.name);
			if(col.GetComponent<SpriteObject>()) {
				col.GetComponent<SpriteObject>().FellOffEdge();
			}
            else if (col.GetComponentInParent<SpriteObject>())
            {
                col.GetComponentInParent<SpriteObject>().FellOffEdge();
            }
        }
    } 
}
