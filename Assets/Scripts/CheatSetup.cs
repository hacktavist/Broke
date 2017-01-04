using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
[Serializable]
public class CheatSetup {

  public string nameOfCheat;
  public string [] cheatSequence;
  public int currentPosition;
  //public KeyCode[] cheatSequence;
  public float cheatDelay;
  public UnityEvent CheatFunction;
  [NonSerialized]
  public bool sequenceMatched;

  
}
