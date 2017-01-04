using UnityEngine;
using System.Collections;

public class TestPad : MonoBehaviour {
  public int gamepadNumber = 1;

  GamePadInput pad;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    pad = GamePadManager.Instance.GetPad(gamepadNumber);
    if (pad.GetButtonDown("A")) {
      Debug.Log("A button pressed");
      pad.AddRumble(2.5f, new Vector2(0.5f, 0.5f), 0.2f);
    }
	}
}
