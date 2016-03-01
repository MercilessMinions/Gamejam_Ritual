using UnityEngine;

public class InputTester : MonoBehaviour {

	public static InputTester instance;

	public Sprite XBOX_A, PS4_A, KEY_GEN, XBOX_START, PS4_START, KEY_START;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            ControllerManager test = new ControllerManager();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
