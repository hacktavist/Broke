using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {


  public Transform myPlayer;
  public Vector3 myPos = new Vector3(0, 0, -2f);
  public float camOffsetX;
  public float camoffsetY;
	// Update is called once per frame
	void Update () {
    
    transform.position = myPlayer.position + myPos;
	}
}
