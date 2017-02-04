using UnityEngine;
using System;
using System.Collections;

public class CharacterController : MonoBehaviour {

  [Serializable]
  public class MoveSettings {
    public float forwardVelocity = 12f;
    public float rotateVelocity = 100f;
    public float jumpVelocity = 25f;
    public float distToGround = .1f;
    public LayerMask ground;
  }
  [Serializable]
  public class PhysicsSettings {
    public float downAcceleration = .75f;
  }
  [Serializable]
  public class InputSettings {
    public float inputDelay = .1f;
    public string FORWARD_AXIS = "Vertical";
    public string TURN_AXIS = "Horizontal";
    public string JUMP_AXIS = "Jump";
  }

  public Animator anim;
  

  public MoveSettings moveSetting = new MoveSettings();
  public PhysicsSettings physicsSetting = new PhysicsSettings();
  public InputSettings inputSetting = new InputSettings();
  public Vector3 velocity = Vector3.zero;
  Quaternion targetRotation;
  Rigidbody rbody;
  float forwardInput, turnInput, jumpInput;
  
  public Quaternion TargetRotation {
     get { return targetRotation; }
  }

  bool Grounded() {
    return Physics.Raycast(transform.position, Vector3.down, moveSetting.distToGround, moveSetting.ground);
  }

	// Use this for initialization
	void Start () {
    anim = GetComponent<Animator>();
    targetRotation = transform.rotation;
    if(GetComponent<Rigidbody>())
      rbody = GetComponent<Rigidbody>();
    else
      Debug.Log("Need rbody");

    forwardInput = turnInput = jumpInput = 0;

	}
	
  void GetInput() {
    forwardInput = Input.GetAxis(inputSetting.FORWARD_AXIS);
    turnInput = Input.GetAxis(inputSetting.TURN_AXIS);
    jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS);
  }

	// Update is called once per frame
	void Update () {
    GetInput();
    Turn();
	}

  void FixedUpdate() {
    Run();
    Jump();

    rbody.velocity = transform.TransformDirection(velocity);
  }

  void Run() {
    //if(Mathf.Abs(forwardInput) > inputSetting.inputDelay) {
      //move;
      velocity.z = moveSetting.forwardVelocity * forwardInput;
      anim.SetFloat("Forward",forwardInput);
    //} else {
      //velocity.z = 0;
    //}
  }

  void Turn() {
    if(Mathf.Abs(turnInput) > inputSetting.inputDelay) {
      targetRotation *= Quaternion.AngleAxis(moveSetting.rotateVelocity * turnInput * Time.deltaTime,Vector3.up);
    }
    transform.rotation = targetRotation;
    anim.SetFloat("Turn",turnInput);
  }

  void Jump() {
    if(jumpInput > 0 && Grounded()) {
      velocity.y = moveSetting.jumpVelocity;
      anim.SetBool("OnGround",false);
      anim.SetFloat("Jump",velocity.y);
    } else if(jumpInput == 0 && Grounded()) {
      velocity.y = 0;
      anim.SetBool("OnGround",true);
      anim.SetFloat("Jump",0);
    } else {
      velocity.y -= physicsSetting.downAcceleration;
      anim.SetFloat("Jump",velocity.y);
    }
  }
}
