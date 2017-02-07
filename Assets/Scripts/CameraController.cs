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
    public bool smoothing = true;
    public float smooth = .05f;

    public float newDistance = -8f;
    public float adjustmentDistance = -8f;
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

  [Serializable]
  public class DebugSettings {
    public bool drawDesiredCollisionLines = true;
    public bool drawAdjustedCollisionLines = true;
  }

  public PositionSettings pos = new PositionSettings();
  public OrbitSettings orbit = new OrbitSettings();
  public InputSettings input = new InputSettings();
  public DebugSettings debugSettings = new DebugSettings();
  public CollisionHandler cHandler = new CollisionHandler();

  Vector3 targetPos = Vector3.zero;
  Vector3 dest = Vector3.zero;
  Vector3 adjustedDest = Vector3.zero;
  Vector3 camVelocity = Vector3.zero;
  CharacterController charController;
  float vertOrbitInput, horOrbitInput, zoomInput, horOrbitSnapInput;

  // Use this for initialization
  void Start() {
    SetCameraTarget(target);
    MoveToTarget();

    cHandler.Init(Camera.main);
    cHandler.UpdateCameraClipPoints(transform.position,transform.rotation,ref cHandler.adjustedCameraClipPoints);
    cHandler.UpdateCameraClipPoints(dest,transform.rotation,ref cHandler.desiredCameraClipPoints);


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

  void FixedUpdate() {
    MoveToTarget();
    LookAtTarget();

    cHandler.UpdateCameraClipPoints(transform.position,transform.rotation,ref cHandler.adjustedCameraClipPoints);
    cHandler.UpdateCameraClipPoints(dest,transform.rotation,ref cHandler.desiredCameraClipPoints);

    //debug
    for (int i = 0; i < 5; i++) {
      if(debugSettings.drawAdjustedCollisionLines) {
        Debug.DrawLine(targetPos,cHandler.adjustedCameraClipPoints[i],Color.red);
      }
      if(debugSettings.drawDesiredCollisionLines) {
        Debug.DrawLine(targetPos,cHandler.desiredCameraClipPoints[i],Color.white);
      }
    }

    cHandler.CheckColliding(targetPos);
    pos.adjustmentDistance = cHandler.GetAdjustedDistanceWithRay(targetPos);
  }

  void MoveToTarget() {
    //targetPos = target.position + pos.targetOffsetPosition;
    targetPos = target.position + Vector3.up * pos.targetOffsetPosition.y + Vector3.forward * pos.targetOffsetPosition.z + transform.TransformDirection(Vector3.right * pos.targetOffsetPosition.x);
    dest = Quaternion.Euler(orbit.xRotation,orbit.yRotation + target.eulerAngles.y,0) * -Vector3.forward * pos.distanceFromTarget;
    dest += targetPos;
    //transform.position = dest;

    if(cHandler.colliding) {
      adjustedDest = Quaternion.Euler(orbit.xRotation,orbit.yRotation + target.eulerAngles.y,0) * Vector3.forward * pos.adjustmentDistance;
      adjustedDest += targetPos;
      if(pos.smoothing) {
        transform.position = Vector3.SmoothDamp(transform.position,adjustedDest,ref camVelocity,pos.smooth);
      } else {
        transform.position = adjustedDest;
      }

    } else {
      if(pos.smoothing) {
        transform.position = Vector3.SmoothDamp(transform.position,dest,ref camVelocity,pos.smooth);
      } else {
        transform.position = dest;
      }
    }
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
    }
    if(orbit.xRotation < orbit.minRotationX) {
      orbit.xRotation = orbit.minRotationX;
    }
  }

  void ZoomOnTarget() {
    pos.distanceFromTarget += zoomInput * pos.zoomSmooth * Time.deltaTime;
    if(pos.distanceFromTarget > pos.maxZoom) {
      pos.distanceFromTarget = pos.maxZoom;
    }
    if(pos.distanceFromTarget < pos.minZoom) {
      pos.distanceFromTarget = pos.minZoom;
    }
  }

  [Serializable]
  public class CollisionHandler {
    public LayerMask collisionLayer;
    public bool colliding = false;
    public Vector3[] adjustedCameraClipPoints;
    public Vector3[] desiredCameraClipPoints;

    Camera cam;
  
    public void Init(Camera camera) {
      cam = camera;
      adjustedCameraClipPoints = new Vector3[5];
      desiredCameraClipPoints = new Vector3[5];
    }

    bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition) {
      for (int i = 0; i < clipPoints.Length; i++) {
        Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
        float distance = Vector3.Distance(clipPoints[i], fromPosition);
        if(Physics.Raycast(ray, distance, collisionLayer)) {
          return true;
        }
      }

      return false;
    }

    public void UpdateCameraClipPoints(Vector3 camPos, Quaternion atRotation, ref Vector3[] intoArray) {
      if(!cam) {
        return;
      }

      intoArray = new Vector3[5];
      float z = cam.nearClipPlane;
      float x = Mathf.Tan(cam.fieldOfView / 2) * z;
      float y = x / cam.aspect;

      intoArray[0] = (atRotation * new Vector3(-x,y,z)) + camPos;

      intoArray[1] = (atRotation * new Vector3(x,y,z)) + camPos;

      intoArray[2] = (atRotation * new Vector3(-x,-y,z)) + camPos;

      intoArray[3] = (atRotation * new Vector3(x,-y,z)) + camPos;

      intoArray[4] = camPos - cam.transform.forward;

    }

    public float GetAdjustedDistanceWithRay(Vector3 from) {
      float distance = -1;

      for (int i = 0; i <desiredCameraClipPoints.Length; i++) {
        Ray ray = new Ray(from, desiredCameraClipPoints[i] - from);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)) {
          if(distance == -1) {
            distance = hit.distance;
          } else {
            if(hit.distance < distance) {
              distance = hit.distance;
            }
          }
        }
      }
      if(distance == -1)
        return 0;
      else
        return distance;
    }

    public void CheckColliding(Vector3 targetPos) {
      if(CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPos)) {
        colliding = true;
      } else {
        colliding = false;
      }
    }


  } // End CollisionHandler Class
}