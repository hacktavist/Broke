using UnityEngine;
using System;
using System.Collections;

public class CameraController : MonoBehaviour {

  public Transform target;
  [Serializable]
  public class PositionSettings {
    public Vector3 targetOffsetPosition = new Vector3(0,3.4f,0);
    public float lookSmooth = 100f;
    public float distanceFromTarget = -8f;
    public float zoomSmooth = 10f;
    public float maxZoom = -2f;
    public float minZoom = -15f;
  }

  [Serializable]
  public class OrbitSettings {
    public float xRotation = -20f;
    public float yRotation = -180f;
    public float maxRotationX = 25f;
    public float minRotationX = 85f;
    public float vertOrbitSmooth = 150f;
    public float horOrbitSmooth = 150f;
  }
  [Serializable]
  public class InputSettings {
    public string ORBIT_H_SNAP = "OrbitHSnap";
    public string ORBIT_H = "RightStickY";
    public string ORBIT_VERTICAL = "RightStickX";
    public string ZOOM = "Mouse ScrollWheel";
  }

  public PositionSettings pos = new PositionSettings();
  public OrbitSettings orbit = new OrbitSettings();
  public InputSettings input = new InputSettings();

  Vector3 targetPos = Vector3.zero;
  Vector3 dest = Vector3.zero;
  CharacterController charController;
  float vertOrbitInput, horOrbitInput, zoomInput, horOrbitSnapInput;

  // Use this for initialization
  void Start() {
    SetCameraTarget(target);
    MoveToTarget();
  }

  void SetCameraTarget(Transform t) {
    target = t;

    if(target != null) {
      if(target.GetComponent<CharacterController>()) {
        charController = target.GetComponent<CharacterController>();
      } else {
        Debug.Log("Camera Target Needs A Character Controller");
      }
    } else {
      Debug.Log("Camera Needs A Target");
    }
  }

  void GetInput() {
    vertOrbitInput = Input.GetAxisRaw(input.ORBIT_VERTICAL);
    horOrbitInput = Input.GetAxisRaw(input.ORBIT_H);
    horOrbitSnapInput = Input.GetAxisRaw(input.ORBIT_H_SNAP);
    zoomInput = Input.GetAxisRaw(input.ZOOM);
  }

  void Update() {
    GetInput();
    OrbitTarget();
    ZoomOnTarget();
  }

  void LateUpdate() {
    MoveToTarget();
    LookAtTarget();
  }

  void MoveToTarget() {
    targetPos = target.position + pos.targetOffsetPosition;
    dest = Quaternion.Euler(orbit.xRotation,orbit.yRotation + target.eulerAngles.y,0) * -Vector3.forward * pos.distanceFromTarget;
    dest += targetPos;
    transform.position = dest;
  }

  void LookAtTarget() {
    Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
    transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,pos.lookSmooth * Time.deltaTime);
  }



  void OrbitTarget() {
    if(horOrbitSnapInput > 0) {
      orbit.yRotation = -180f;
    }
    orbit.xRotation += vertOrbitInput * orbit.vertOrbitSmooth * Time.deltaTime;
    orbit.yRotation += horOrbitInput * orbit.horOrbitSmooth * Time.deltaTime;
    if(orbit.xRotation > orbit.maxRotationX) {
      orbit.xRotation = orbit.maxRotationX;
      Debug.Log("Fuck, I'm in here");
    }
    if(orbit.xRotation < orbit.minRotationX) {
      orbit.xRotation = orbit.minRotationX;
    }
  }

  void ZoomOnTarget() {
    pos.distanceFromTarget += zoomInput * pos.zoomSmooth;
    if(pos.distanceFromTarget > pos.maxZoom) {
      pos.distanceFromTarget = pos.maxZoom;
    }
    if(pos.distanceFromTarget < pos.minZoom) {
                                             pos.distanceFromTarget = pos.minZoom;
    }
  }
}