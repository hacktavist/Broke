using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePadManager : MonoBehaviour {
  public int numberOfPads = 2;
  List<GamePadInput> pads;
  static GamePadManager manager;

	// Use this for initialization
	void Awake () {
    if (manager != null && manager != this) {
      Destroy(this.gameObject);
      return;
    } else {
      manager = this;
      DontDestroyOnLoad(this.gameObject);
      numberOfPads = Mathf.Clamp(numberOfPads, 1, 4);
      pads = new List<GamePadInput>();

      for (int i = 0; i < numberOfPads; ++i) {
        pads.Add(new GamePadInput(i + 1));
      }
    }
    
	}

  public static GamePadManager Instance {
    get {
      if (manager == null) {
        Debug.Log("Manager doesn't exist");
        return null;
      }
      return manager;
    }
  }

	// Update is called once per frame
	void Update () {
    for (int i = 0; i < pads.Count; ++i) {
      pads[i].Update();
    }
	}

  public void Refresh() {
    for (int i = 0; i < pads.Count; ++i) {
      pads[i].Refresh();
    }
  }

  public GamePadInput GetPad(int index) {
    for (int i = 0; i < pads.Count; ) {
      if (pads[i].Index == (index - 1)) {
        return pads[i];
      } else
        ++i;
    } //End for loop
    Debug.LogError("Index " + index + " is not valid");
    return null;
  }
}
