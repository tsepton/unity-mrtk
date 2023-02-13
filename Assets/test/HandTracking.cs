using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class HandTracking : MonoBehaviour {
  [Header("Gesture Settings")] 
  public Handedness handedness = Handedness.Right;
  public Gestures openEvent = Gestures.HandWideOpen;
  public Gestures closeEvent = Gestures.HandItalian;
  
  [Header("Gesture registration")] 
  public UnityEvent open;
  public UnityEvent close;

  [Header("Debugger Settings")] public bool useDebug;
  public GameObject debugUi;
  public Debugger.DebugMode debugMode = Debugger.DebugMode.Curl;
  private Debugger _debugger;

  void Start() {
    if (useDebug) _debugger = new Debugger(debugUi, transform, handedness);
  }

  void Update() {
    if (useDebug) _debugger.DisplayDebug(debugMode);
    
    if (GetGesture(openEvent).IsOccuring()) open.Invoke();
    if (GetGesture(closeEvent).IsOccuring()) close.Invoke();
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

  public enum Gestures {
    HandWideOpen = 0,
    HandItalian = 1
  }

  [CanBeNull]
  CustomGesture GetGesture(Gestures id) {
    switch (id) {
      case Gestures.HandWideOpen:
        return HandWideOpen.Instance;
      case Gestures.HandItalian:
        return HandItalian.Instance;
    }

    return null;
  }
}