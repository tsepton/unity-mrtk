using System;
using System.Collections;
using JetBrains.Annotations;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class HandTrackingController : MonoBehaviour {
  [Header("Gesture Settings")] public Handedness handedness = Handedness.Right;
  public Gestures openEvent = Gestures.HandWideOpen;
  public Gestures closeEvent = Gestures.HandItalian;
  public Gestures selectionEvent = Gestures.HandSelectionWithFinger;

  [Header("Gesture registration")] public UnityEvent open;
  public UnityEvent close;
  public GazeTargetEvent selection;

  [Header("Debugger Settings")] public bool useDebug;
  public GameObject debugUi;
  public Debugger.DebugMode debugMode = Debugger.DebugMode.Curl;
  private Debugger _debugger;

  private bool _constrainerState;

  public bool ConstrainerState {
    set => _constrainerState = value;
  }

  void Start() {
    if (useDebug) _debugger = new Debugger(debugUi, transform, handedness);
    StartCoroutine(nameof(HandleHandGestureRecognition));
  }

  void Update() {
    if (useDebug) _debugger.DisplayDebug(debugMode);
  }

  IEnumerator HandleHandGestureRecognition() {
    while (true) {
        if (_constrainerState && GetGesture(openEvent).IsOccuring()) open.Invoke();
        if (_constrainerState && GetGesture(closeEvent).IsOccuring()) close.Invoke();
        if (GetGesture(selectionEvent).IsOccuring()) {
          // FIXME : GazeTarget is not updated when leaving gameobject
          // If object too far away, not working too
          selection.Invoke(CoreServices.InputSystem.EyeGazeProvider.GazeTarget);
          yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.05f);
    }
  }

  [System.Serializable]
  public class GazeTargetEvent : UnityEvent<GameObject> { }

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
          case DebugMode.Current:
            text = CurrentGestureText();
            break;
          case DebugMode.Curl:
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
    HandItalian = 1,
    HandSelectionWithFinger = 2
  }

  [CanBeNull]
  CustomGesture GetGesture(Gestures id) {
    switch (id) {
      case Gestures.HandWideOpen:
        return HandWideOpen.Instance(handedness);
      case Gestures.HandItalian:
        return HandItalian.Instance(handedness);
      case Gestures.HandSelectionWithFinger:
        return HandSelectionWithFinger.Instance(handedness);
    }

    return null;
  }
}