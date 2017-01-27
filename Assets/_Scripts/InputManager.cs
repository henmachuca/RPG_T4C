using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static InputManager instance;

    public delegate void InputEvent();      // Created since there will be a couple of events! This one is called INPUTEVENT
    public static event InputEvent OnPressUp;
    public static event InputEvent KeyPressedDown;

    // Use this for initialization
    void Start () {
        if (instance == null) instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonUp(0)) {
            if (OnPressUp != null) OnPressUp();
        }
        if (Input.anyKeyDown) {
            if (KeyPressedDown != null) KeyPressedDown();
            //print("Input Key: " + Input.inputString);
        }
	}
}
