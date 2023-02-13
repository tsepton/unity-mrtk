using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using Unity.VisualScripting;

public class HandTracking : MonoBehaviour {
  
  [Header("Gesture Settings")]
  public Handedness handedness = Handedness.Right;
  
  [Header("Debugger Settings")]
  public bool useDebug;
  public GameObject debugUi;
  public Debugger.DebugMode debugMode = Debugger.DebugMode.Curl;
  private Debugger _debugger;


  void Start() { 
    if (useDebug) _debugger = new Debugger(debugUi, transform, handedness);
  }

  void Update() {
    if (useDebug) _debugger.DisplayDebug(debugMode);
  }

  private class CustomGesture { 
    private readonly float _thumbCurl;
    private readonly float _indexCurl;
    private readonly float _middleCurl;
    private readonly float _ringCurl;
    private readonly float _pinkyCurl;

    private readonly float _threshold; // 0 <= threshold < 1

    public CustomGesture(float thumbCurl, float indexCurl, float middleCurl, float ringCurl, float pinkyCurl,
      float threshold) {
      _thumbCurl = thumbCurl;
      _indexCurl = indexCurl;
      _middleCurl = middleCurl;
      _ringCurl = ringCurl;
      _pinkyCurl = pinkyCurl;
      _threshold = threshold;
    }

    public bool IsOccuring() {
      bool answer = Math.Abs(HandPoseUtils.ThumbFingerCurl(Handedness.Any) - _thumbCurl) <= _threshold;
      answer = answer && Math.Abs(HandPoseUtils.IndexFingerCurl(Handedness.Any) - _indexCurl) <= _threshold;
      answer = answer && Math.Abs(HandPoseUtils.MiddleFingerCurl(Handedness.Any) - _middleCurl) <= _threshold;
      answer = answer && Math.Abs(HandPoseUtils.RingFingerCurl(Handedness.Any) - _ringCurl) <= _threshold;
      answer = answer && Math.Abs(HandPoseUtils.PinkyFingerCurl(Handedness.Any) - _pinkyCurl) <= _threshold;

      return answer;
    }
  }

  public class Debugger {
    private readonly GameObject _uiObject;
    private MixedRealityPose _wristPose;
    private Handedness _handedness;
    
    public Debugger(GameObject ui, Transform transform, Handedness handedness) {
      _uiObject = Instantiate(ui, transform);
      _handedness = handedness;
    }
    
    public void DisplayDebug(DebugMode mode) {
      _uiObject.SetActive(false);
      if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, _handedness, out _wristPose)) {
        _uiObject.SetActive(true);
        _uiObject.transform.position = _wristPose.Position;

        string text = "";
        switch (mode) {
          case DebugMode.Curl:
            text = CurrentGestureText();
            break;
          case DebugMode.Current:
            text = CurlText();
            break;
        }
        TextMesh uiText = _uiObject.GetComponentInChildren<TextMesh>();
        uiText.text = text;
      }
    }

    private string CurlText() {
      var thumbCurl = $"thumb : {HandPoseUtils.ThumbFingerCurl(_handedness):0.00}";
      var indexCurl = $"index : {HandPoseUtils.IndexFingerCurl(_handedness):0.00}";
      var middleCurl = $"middle : {HandPoseUtils.MiddleFingerCurl(_handedness):0.00}";
      var ringCurl = $"ring : {HandPoseUtils.RingFingerCurl(_handedness):0.00}";
      var pinkyCurl = $"pinky : {HandPoseUtils.PinkyFingerCurl(_handedness):0.00}";

      return $"{thumbCurl} \n{indexCurl} \n{middleCurl} \n{ringCurl} \n{pinkyCurl}";
    }
    
    private string CurrentGestureText() {
        return "???";
    }

    public enum DebugMode {
      Current, 
      Curl
    }
  }

  enum Gestures {
    HandWideOpen = 0,
    HandItalian = 1
  }

  [CanBeNull]
  CustomGesture GetGesture(Gestures id) {
    switch (id) {
      case Gestures.HandWideOpen:
        return new CustomGesture(0, 0, 0, 0, 0, 0.25f);
      case Gestures.HandItalian:
        return new CustomGesture(0.7f, 0.35f, 0.35f, 0.35f, 0.3f, 0.25f);
    }

    return null;
  }

}