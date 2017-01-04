using UnityEngine;
using System.Collections;

public class GamePadRefresh : MonoBehaviour {


	
	// Update is called once per frame
	void Update () {
    GamePadManager.Instance.Refresh();
	}
}
