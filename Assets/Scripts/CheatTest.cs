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


    if (Input.GetButtonDown("Jump")) {
      tempKey = "jump";
    } else if (Input.GetButtonDown("Switch")) {
      tempKey = "switch";
    } else if (Input.GetButtonDown("Reload")) {
      tempKey = "reload";
    } else if (Input.GetButtonDown("Melee")) {
      tempKey = "melee";
    }




    if (Input.anyKeyDown){
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
