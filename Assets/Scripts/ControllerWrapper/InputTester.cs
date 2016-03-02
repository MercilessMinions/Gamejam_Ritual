using UnityEngine;

public class InputTester : MonoBehaviour {

	public static InputTester instance;

	public Sprite XBOX_A, PS4_A, KEY_A, XBOX_B, PS4_B, KEY_B, XBOX_START, PS4_START, KEY_START, JOYLEFT, KEY_UPDOWNLEFTRIGHT;

	private ControllerManager cm;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            cm = new ControllerManager();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

	void OnLevelWasLoaded(int level) {
		if(level == 0 && cm != null) {
			cm.ClearPlayers();
		}
	}

    void Update()
    {
        //Debug.Log("J1A3: " + Input.GetAxis("j1_Axis3"));
        //Debug.Log("J2A3: " + Input.GetAxis("j2_Axis3"));
        //Debug.Log("===============");
    }
}
