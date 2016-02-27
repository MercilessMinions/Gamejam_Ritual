using UnityEngine;

public class InputTester : MonoBehaviour {

    private static InputTester instance;

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
