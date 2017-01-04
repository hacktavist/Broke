using UnityEngine;
using XInputDotNetPure;
using System.Collections.Generic;
public struct Button {
  public ButtonState prevState;
  public ButtonState currentState;
}
public struct TriggerState {
  public float prevVal;
  public float currentVal;
}
class Rumble {
  public float timer;
  public float duration;
  public Vector2 power;

  public void UpdateTimer() {
    this.timer -= Time.deltaTime;
  }
}

public class GamePadInput {
  GamePadState prevState;
  GamePadState currentState;
  int gamepadIndex;
  PlayerIndex pIndex;
  List<Rumble> rEvents;
  Dictionary<string, Button> inputMap;
  Button A, B, X, Y;
  Button dUp, dDown, dLeft, dRight;
  Button Home;
  Button Start, Back;
  Button L3, R3;
  Button lBumper, rBumper;
  TriggerState lTrigger, rTrigger;

  public GamePadInput(int index) {
    gamepadIndex = index - 1;
    pIndex = (PlayerIndex)gamepadIndex;
    rEvents = new List<Rumble>();
    inputMap = new Dictionary<string, Button>();
  }

  void InputUpdate() {
    inputMap["A"] = A;
    inputMap["B"] = B;
    inputMap["X"] = X;
    inputMap["Y"] = Y;
    inputMap["dUp"] = dUp;
    inputMap["dDown"] = dDown;
    inputMap["dLeft"] = dLeft;
    inputMap["dRight"] = dRight;
    inputMap["L3"] = L3;
    inputMap["R3"] = R3;
    inputMap["lBumper"] = lBumper;
    inputMap["rBumper"] = rBumper;
    
    inputMap["Start"] = Start;
    inputMap["Back"] = Back;
    inputMap["Home"] = Home;
  } //End of InputUpdate()
  public void Update() {
    currentState = GamePad.GetState(pIndex);

    if (currentState.IsConnected) {
      A.currentState = currentState.Buttons.A;
      B.currentState = currentState.Buttons.B;
      X.currentState = currentState.Buttons.X;
      Y.currentState = currentState.Buttons.Y;
      dUp.currentState = currentState.DPad.Up;
      dDown.currentState = currentState.DPad.Down;
      dLeft.currentState = currentState.DPad.Left;
      dRight.currentState = currentState.DPad.Right;
      L3.currentState = currentState.Buttons.LeftStick;
      R3.currentState = currentState.Buttons.RightStick;
      lBumper.currentState = currentState.Buttons.LeftShoulder;
      rBumper.currentState = currentState.Buttons.RightShoulder;
      lTrigger.currentVal = currentState.Triggers.Left;
      rTrigger.currentVal = currentState.Triggers.Right;
      Start.currentState = currentState.Buttons.Start;
      Back.currentState = currentState.Buttons.Back;
      Home.currentState = currentState.Buttons.Guide;
      InputUpdate();
      HandleRumble();
    }
  } //End of Update()

  public void Refresh() {
    prevState = currentState;

    if (currentState.IsConnected) {
      A.prevState = prevState.Buttons.A;
      B.prevState = prevState.Buttons.B;
      X.prevState = prevState.Buttons.X;
      Y.prevState = prevState.Buttons.Y;
      dUp.prevState = prevState.DPad.Up;
      dDown.prevState = prevState.DPad.Down;
      dLeft.prevState = prevState.DPad.Left;
      dRight.prevState = prevState.DPad.Right;
      L3.prevState = prevState.Buttons.LeftStick;
      R3.prevState = prevState.Buttons.RightStick;
      lBumper.prevState = prevState.Buttons.LeftShoulder;
      rBumper.prevState = prevState.Buttons.RightShoulder;
      lTrigger.prevVal = prevState.Triggers.Left;
      rTrigger.prevVal = prevState.Triggers.Right;
      Start.prevState = prevState.Buttons.Start;
      Back.prevState = prevState.Buttons.Back;
      Home.prevState = prevState.Buttons.Guide;
      InputUpdate();
    }
  } //End of Refresh()

  void HandleRumble() {
    if (rEvents.Count > 0) {
      Vector2 cp = new Vector2(0, 0);
      for (int i = 0; i < rEvents.Count; ++i) {
        rEvents[i].UpdateTimer();
        if (rEvents[i].timer > 0) {
          float remaining = Mathf.Clamp(rEvents[i].timer / rEvents[i].duration, 0f, 1f);
          cp = new Vector2(Mathf.Max(rEvents[i].power.x * remaining, cp.x),
                           Mathf.Max(rEvents[i].power.y * remaining, cp.y));

          GamePad.SetVibration(pIndex, cp.x, cp.y);
        } //End if for fading rumble
      } //End for loop updating rumble timer
    } //End if checking for rumble event
  } //End HandleRumble()

  public void AddRumble(float timer, Vector2 power, float duration) {
    Rumble rumble = new Rumble();

    rumble.timer = timer;
    rumble.power = power;
    rumble.duration = duration;

    rEvents.Add(rumble);
  } //End AddRumble()

  public GamePadThumbSticks.StickValue GetLeftStick() {
    return currentState.ThumbSticks.Left;
  } //End GetLeftStick()
  public GamePadThumbSticks.StickValue GetRightStick() {
    return currentState.ThumbSticks.Right;
  } //End GetRightStick()
  public float GetLeftTrigger() {
    return currentState.Triggers.Left;
  } //End GetLeftTrigger()
  public float GetRightTrigger() {
    return currentState.Triggers.Right;
  } //End GetRightTrigger()

  public bool GetTriggerTapLeft() {
    return lTrigger.prevVal == 0f && lTrigger.currentVal >= .1f ? true : false;
  } //End GetTriggerTapLeft()

  public bool GetTriggerTapRight() {
    return rTrigger.prevVal == 0f && rTrigger.currentVal >= .1f ? true : false;
  } //End GetTriggerTapRight()

  public int Index { get { return gamepadIndex; } }

  public bool isConnected { get { return currentState.IsConnected; } }

  public bool GetButton(string buttonPress) {
    return inputMap[buttonPress].currentState == ButtonState.Pressed ?true:false;
  }

  public bool GetButtonDown(string buttonPress) {
    return inputMap[buttonPress].prevState == ButtonState.Released &&
           inputMap[buttonPress].currentState == ButtonState.Pressed ? true : false;
  }
}


