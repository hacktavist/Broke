using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CheatTest : MonoBehaviour {
  public CheatSetup[] cheats;
  Event cEvent;
  GamePadInput controller;
  float timeLastButtonPressed;
  int currentPosition;
  string tempKey;
  bool sequenceMatched = false;
  System.Array values = System.Enum.GetValues(typeof(KeyCode));
  static CheatTest instance;
  int cheatIndex;

  void Awake() {
    if (GamePadManager.Instance.isActiveAndEnabled)
      controller = GamePadManager.Instance.GetPad(1);
    if (instance != null && instance != this) {
      Destroy(this);
    } else {
      instance = this;

      DontDestroyOnLoad(instance);
    }
  }

  // Update is called once per frame
  void Update() {
    #region
    if (controller.isConnected == false) {
      if (Input.GetKeyDown(KeyCode.LeftArrow)) {
        tempKey = "left";
      } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
        tempKey = "right";
      } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
        tempKey = "up";
      } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
        tempKey = "down";
      } else if (Input.GetKeyDown(KeyCode.Space)) {
        tempKey = "jump";
      } else if (Input.GetKeyDown(KeyCode.Space)) {
        tempKey = "jump";
      } else if (Input.GetKeyDown(KeyCode.Mouse0)) {
        tempKey = "melee";
      } else if (Input.GetKeyDown(KeyCode.E)) {
        tempKey = "switch";
      }

    } else {

      /* This is the work around for controller cheats */
      //  if(Input.GetButtonDown("Jump")) {
      //  tempKey = "jump";
      //}


      if (controller.GetButtonDown("dLeft"))
      {
        tempKey = "left";
      }
      else if (controller.GetButtonDown("dRight"))
      {
        tempKey = "right";
      }
      else if (controller.GetButtonDown("dUp"))
      {
        tempKey = "up";
      }
      else if (controller.GetButtonDown("dDown"))
      {
        tempKey = "down";
      }
      else if (controller.GetButtonDown("A"))
      {
        tempKey = "jump";
      }
      else if (controller.GetButtonDown("B"))
      {
        tempKey = "interact";
      }
      else if (controller.GetButtonDown("X"))
      {
        tempKey = "melee";
      }
      else if (controller.GetButtonDown("Y"))
      {
        tempKey = "switch";
      }
      else if (controller.GetButtonDown("Start"))
      {
        tempKey = "menu";
      }
      else if (controller.GetButtonDown("Back"))
      {
        tempKey = "options";
      }
      else if (controller.GetTriggerTapLeft())
      {
        tempKey = "walk";
      }
      else if (controller.GetTriggerTapRight())
      {
        tempKey = "shoot";
      }
      else if (controller.GetButtonDown("lBumper"))
      {
        tempKey = "";
      }
      else if (controller.GetButtonDown("rBumper"))
      {
        tempKey = "";
      }
      else if (controller.GetButtonDown("L3"))
      {
        tempKey = "";
      }
      else if (controller.GetButtonDown("R3"))
      {
        tempKey = "";
      }
      else
        tempKey = "";
    }
    #endregion

    if(Input.anyKeyDown){
      CheatCode(tempKey);
      tempKey = "";
    }
  } //End Update Function



  void CheatCode(string keypress) {
    //////////////////////////////////////////////////////////////////////
    //TODO: make sure cheat combos are not interfering with one another /
    ////////////////////////////////////////////////////////////////////
    foreach (CheatSetup code in cheats) {
    //for (int i = 0; i < cheats.Length; i++) {
      //cheats[i].sequenceMatched = false;
      Debug.Log(code.nameOfCheat);
      if (Time.time > code.cheatDelay + timeLastButtonPressed) {
        code.currentPosition = 0;
      }
      if (code.currentPosition < code.cheatSequence.Length) {
        Debug.Log("code " + code.cheatSequence[code.currentPosition]);
        if (code.cheatSequence[code.currentPosition] == keypress) {
          timeLastButtonPressed = Time.time;

          code.currentPosition++;

          if (code.currentPosition == code.cheatSequence.Length) {
            if (controller.isConnected) {
              controller.AddRumble(1f, new Vector2(0.5f, 0.5f), 0.2f);
            }
            
            code.currentPosition = 0;
            code.sequenceMatched = true;
            if (code.sequenceMatched == true){
                code.CheatFunction.Invoke();
                code.sequenceMatched = false;
                //return;
            }

                    } //End if for being at the end of the cheat
        }  //End if the button matched the cheat index


      }  //End if position is still within sequence length

    

    }
    //} //End foreach loop
  }
}
