
using System;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using UnityEngine.Events;

public interface CustomGesture {
  float ThumbCurl { get; } 
  float IndexCurl { get; } 
  float MiddleCurl { get; } 
  float RingCurl { get; } 
  float PinkyCurl { get; }

  public bool IsOccuring();
}

[Serializable] 
public class HandWideOpen: UnityEvent, CustomGesture {
  public float ThumbCurl { get; } = 0;

  public float IndexCurl { get; } = 0;

  public float MiddleCurl { get; } = 0;

  public float RingCurl { get; } = 0;

  public float PinkyCurl { get; } = 0;

  private float _threshold = 0.05f;
  
  private Handedness _handedness;
  
  public bool IsOccuring() {
    if (HandJointUtils.FindHand(Handedness.Right) == null) return false;
    
    bool answer = ThumbCurl <= 0.4f;
    answer = answer && Math.Abs(HandPoseUtils.IndexFingerCurl(Handedness.Any) - IndexCurl) <= _threshold;
    answer = answer && Math.Abs(HandPoseUtils.MiddleFingerCurl(Handedness.Any) - MiddleCurl) <= _threshold;
    answer = answer && Math.Abs(HandPoseUtils.RingFingerCurl(Handedness.Any) - RingCurl) <= _threshold;
    answer = answer && Math.Abs(HandPoseUtils.PinkyFingerCurl(Handedness.Any) - PinkyCurl) <= _threshold;
    
    if (answer) Debug.Log("HandWideOpen is currently occuring");
    return answer;
  }
  
  private HandWideOpen(Handedness handedness) {
    _handedness = handedness;
  }
  
  // /////////////////////////
  // Static fields and methods
  // FIXME : how to implement this in the interface instead of repeating this in every class implementing it?
  // /////////////////////////
  
  private static List<HandWideOpen> _instances = new List<HandWideOpen>();

  public static HandWideOpen Instance(Handedness handedness) {
    HandWideOpen maybeInstance = _instances.Find(i => i._handedness == handedness);
    if (maybeInstance == null) {
      HandWideOpen instance = new HandWideOpen(handedness);
      _instances.Add(instance);
      return instance;
    }
    return maybeInstance;
  }

}

[Serializable] 
public class HandItalian : UnityEvent, CustomGesture {
  public float ThumbCurl { get; } = 0.7f;

  public float IndexCurl { get; } = 0.35f;

  public float MiddleCurl { get; } = 0.35f;

  public float RingCurl { get; } = 0.35f;

  public float PinkyCurl { get; } = 0.3f;
  
  private float _threshold = 0.2f;
  
  private Handedness _handedness;
  
  public bool IsOccuring() {
    if (HandJointUtils.FindHand(Handedness.Right) == null) return false;

    bool answer = Math.Abs(HandPoseUtils.ThumbFingerCurl(Handedness.Any) - ThumbCurl) <= _threshold;
    answer = answer && Math.Abs(HandPoseUtils.IndexFingerCurl(Handedness.Any) - IndexCurl) <= _threshold;
    answer = answer && Math.Abs(HandPoseUtils.MiddleFingerCurl(Handedness.Any) - MiddleCurl) <= _threshold;
    answer = answer && Math.Abs(HandPoseUtils.RingFingerCurl(Handedness.Any) - RingCurl) <= _threshold;
    answer = answer && Math.Abs(HandPoseUtils.PinkyFingerCurl(Handedness.Any) - PinkyCurl) <= _threshold;
    
    if (answer) Debug.Log("HandItalian is currently occuring");
    return answer;
  }

  private HandItalian(Handedness handedness) {
    _handedness = handedness;
  }

  // /////////////////////////
  // Static fields and methods
  // FIXME : how to implement this in the interface instead of repeating this in every class implementing it?
  // /////////////////////////
  
  private static List<HandItalian> _instances = new List<HandItalian>();

  public static HandItalian Instance(Handedness handedness) {
    HandItalian maybeInstance = _instances.Find(i => i._handedness == handedness);
    if (maybeInstance == null) {
      HandItalian instance = new HandItalian(handedness);
      _instances.Add(instance);
      return instance;
    }
    return maybeInstance;
  }

}

