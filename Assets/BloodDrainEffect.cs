using UnityEngine;
using System.Collections;

public class BloodDrainEffect : MonoBehaviour {

	public Vector3 start, end;

	// Use this for initialization
	void Start () {
		transform.position = start;
		transform.GetChild(0).localScale = Vector3.one*0.01f;
		for(int i =0; i < 10; i++) {
			GameObject g = (GameObject)GameObject.Instantiate(transform.GetChild(0).gameObject);
			g.transform.parent = this.transform;
			g.transform.position = this.transform.position + new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f));
			g.transform.localScale = Vector3.one*0.1f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < transform.childCount; i++) {
			if(transform.GetChild(i).localScale.x < 0.5f) transform.GetChild(i).localScale = Vector3.MoveTowards(transform.GetChild(i).localScale, Vector3.one*0.5f, Time.deltaTime);
			transform.GetChild(i).position = Vector3.MoveTowards(transform.GetChild(i).position, end, Time.deltaTime*5*(2f-(Vector3.Distance(transform.GetChild(0).position, end)/Vector3.Distance(start,end))));
//			transform.GetChild(i).position = Vector3.MoveTowards(transform.GetChild(i).position, end, Time.deltaTime);
		}
		if(Vector3.Distance(transform.GetChild(0).position,end) < 0.01f) {
			Destroy(this.gameObject);
		}
	}
}
