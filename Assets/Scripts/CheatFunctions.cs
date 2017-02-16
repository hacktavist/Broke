using UnityEngine;
using System.Collections;

public class CheatFunctions : MonoBehaviour {

  CheatFunctions instance;
  public CharacterController charController;
  public float superHeight = 50f;
  bool BHEnabled = false;
  bool GiantEnabled = false;

  void Awake() {
    if (instance != null && instance != this) {
      Destroy(this);
    } else {
      instance = this;
      DontDestroyOnLoad(instance);
    }
    
  }

  public void SuperJump() {
    charController.velocity.y = superHeight;
  }

  public void BigHead() {

    
    GameObject head = GameObject.FindWithTag("Head");
    float ScaledHead = 1f;
    if (BHEnabled == false) {
      head.GetComponent<SphereCollider>().enabled = true;
      head.transform.localScale += new Vector3(ScaledHead, ScaledHead, ScaledHead);
    } else {
      head.GetComponent<SphereCollider>().enabled = false;
      head.transform.localScale -= new Vector3(ScaledHead, ScaledHead, ScaledHead);
    }

    BHEnabled = !BHEnabled;

  }

  public void GiantChar() {
    GameObject player = GameObject.FindWithTag("Player");
    float ScalePlayer = 4f;
    if (GiantEnabled == false)
      player.transform.localScale += new Vector3(ScalePlayer, ScalePlayer, ScalePlayer);
    else
      player.transform.localScale -= new Vector3(ScalePlayer, ScalePlayer, ScalePlayer);

    GiantEnabled = !GiantEnabled;
  }
}
