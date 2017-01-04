using UnityEngine;
using System.Collections;

public class Destructable : MonoBehaviour {

  public GameObject destroyed;

  string goingUp = "null";
  float timer = 0;
	// Use this for initialization
	void Start () {
	  
	}
  void OnTriggerEnter(Collider c) {
    if(c.tag == "Head")
      destruction();
  }

  void destruction() {
    Instantiate(destroyed, transform.position, transform.rotation);
    Destroy(gameObject);
  }

}
