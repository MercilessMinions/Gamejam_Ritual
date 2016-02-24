using UnityEngine;
using System.Collections;
using Assets.Scripts.Data;
using Assets.Scripts.Player;
using Assets.Scripts.Level;

public class Monster : MonoBehaviour {

    public float speed = .7f;
    public float minLookForNewPlayer = 2;
    public float maxLookForNewPlayer = 4;
    public float flyAwayForceX = 200f;
    public float flyAwayForceY = 0f;

    private Controller playerChased;
    public System.Collections.Generic.List<Controller> players;
    public System.Collections.Generic.List<PlayerID> winners;
    public System.Collections.Generic.List<PlayerID> losers;
    // Use this for initialization
    void Start () {
//		winners = new System.Collections.Generic.List<PlayerID>();
//		losers = new System.Collections.Generic.List<PlayerID>();
        players = GameManager.instance.AllPlayers;
        findNearestPlayerAndFace();
        for (int x = 0; x < players.Count; x++)
            winners.Add(players[x].ID);
	}
	
	// Update is called once per frame
	void Update () {
        //find the nearest player and face them
        findNearestPlayerAndFace();
        transform.position = Vector2.MoveTowards(transform.position, playerChased.transform.position, Time.deltaTime * speed);
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        //throw them in the opposite direction
        //Debug.Log(col.GetComponent<Controller>().ID);
       // Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
        //rb.isKinematic = false;
       // rb.AddForce(new Vector2(flyAwayForceX, flyAwayForceY));
        Controller deadPlayer = players.Find(x => x.ID.Equals(col.GetComponent<Controller>().ID));
		if(!deadPlayer.LifeComponent.IsInvincible) {
	        winners.Remove(deadPlayer.ID);
	        losers.Add(deadPlayer.ID);
	        deadPlayer.GetComponent<Life>().ModifyHealth(-100);
		}

    }

    private void findNearestPlayerAndFace()
    {
        //System.Collections.Generic.List<Controller> players = GameManager.instance.AllPlayers;
        //Debug.Log(players[1].ID);

        float tempDistance;
        float minDistance = 1000f;
        
        for (int x = 0; x < players.Count; x++)
        {
            if (!losers.Contains(players[x].ID)) {
                Vector2 playerTemp = new Vector2(players[x].transform.position.x, players[x].transform.position.y);
                Vector2 crocTemp = new Vector2(transform.position.x, transform.position.y);
                tempDistance = Vector2.Distance(playerTemp, crocTemp);
                if (tempDistance < minDistance) {
                    minDistance = tempDistance;
                    playerChased = players[x];
                }
            }
        }

        facePlayer(playerChased.transform.position);
    }

    private void facePlayer(Vector3 playerPos)
    {
        if (playerPos.x > transform.position.x)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        else
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
    }

}
