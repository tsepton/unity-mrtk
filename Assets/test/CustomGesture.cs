using System;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using UnityEngine.Events;

public interface CustomGesture {
  public bool IsOccuring();
}

[Serializable]
public class HandWideOpen : UnityEvent, CustomGesture {
  public float ThumbCurl => 0;
  public float IndexCurl => 0;
  public float MiddleCurl => 0;
  public float RingCurl => 0;
  public float PinkyCurl => 0;

  private float _threshold = 0.05f;
  private Handedness _handedness;

  public bool IsOccuring() {
    if (HandJointUtils.FindHand(_handedness) == null) return false;

    bool answer = ThumbCurl <= 0.4f;
    answer = answer && Math.Abs(HandPoseUtils.IndexFingerCurl(_handedness) - IndexCurl) <= _threshold;
    answer = answer && Math.Abs(HandPoseUtils.MiddleFingerCurl(_handedness) - MiddleCurl) <= _threshold;
    answer = answer && Math.Abs(HandPoseUtils.RingFingerCurl(_handedness) - RingCurl) <= _threshold;
    answer = answer && Math.Abs(HandPoseUtils.PinkyFingerCurl(_handedness) - PinkyCurl) <= _threshold;

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
  public float ThumbCurl => 0.7f;
  public float IndexCurl => 0.35f;
  public float MiddleCurl => 0.35f;
  public float RingCurl => 0.35f;
  public float PinkyCurl => 0.3f;

  private float _threshold = 0.2f;
  private Handedness _handedness;

  public bool IsOccuring() {
    if (HandJointUtils.FindHand(_handedness) == null) return false;

    bool answer = Math.Abs(HandPoseUtils.ThumbFingerCurl(_handedness) - ThumbCurl) <= _threshold;
    answer = answer && Math.Abs(HandPoseUtils.IndexFingerCurl(_handedness) - IndexCurl) <= _threshold;
    answer = answer && Math.Abs(HandPoseUtils.MiddleFingerCurl(_handedness) - MiddleCurl) <= _threshold;
    answer = answer && Math.Abs(HandPoseUtils.RingFingerCurl(_handedness) - RingCurl) <= _threshold;
    answer = answer && Math.Abs(HandPoseUtils.PinkyFingerCurl(_handedness) - PinkyCurl) <= _threshold;
    answer = answer && IsNotDefaultPosition();

    return answer;
  }

  /// <summary>
  /// Differentiate between default hand position and italian hand gesture.
  /// </summary>
  /// <returns>true if thumb tip is closer to all other finger tips than to its base.</returns>
  public bool IsNotDefaultPosition() {
    if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, _handedness, out var thumbTipPose) &&
        HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyTip, _handedness, out var pinkyTipPose) &&
        HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbProximalJoint, _handedness, out var thumbMiddlePose)) {
      Vector3 thumbTipToMetacarpal = thumbTipPose.Position - thumbMiddlePose.Position;
      Vector3 pinkyTipToThumbTip = pinkyTipPose.Position - thumbTipPose.Position;
      return pinkyTipToThumbTip.sqrMagnitude <= thumbTipToMetacarpal.sqrMagnitude;
    }

    return false;
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

public class HandSelectionWithFinger : UnityEvent, CustomGesture {
  private Handedness _handedness;

  public bool IsOccuring() {
    if (HandJointUtils.FindHand(_handedness) == null) return false;
    // Second part of statement is to make sure user is not doing a HandItalian
    return IsFingerPartTouchingThumb(TrackedHandJoint.IndexTip) && !IsFingerPartTouchingThumb(TrackedHandJoint.RingTip);
  }

  private bool IsFingerPartTouchingThumb(TrackedHandJoint joint) {
    if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, _handedness, out var thumbTipPose) &&
        HandJointUtils.TryGetJointPose(joint, _handedness, out var fingerPose)) {
      float poseToThumbTip = Vector3.Distance(fingerPose.Position, thumbTipPose.Position);
      return poseToThumbTip <= 0.015f;
    }

    return false;
  }

  // /////////////////////////
  // Static fields and methods
  // FIXME : how to implement this in the interface instead of repeating this in every class implementing it?
  // /////////////////////////

  private HandSelectionWithFinger(Handedness handedness) {
    _handedness = handedness;
  }

  private static List<HandSelectionWithFinger> _instances = new List<HandSelectionWithFinger>();

  public static HandSelectionWithFinger Instance(Handedness handedness) {
    HandSelectionWithFinger maybeInstance = _instances.Find(i => i._handedness == handedness);
    if (maybeInstance == null) {
      HandSelectionWithFinger instance = new HandSelectionWithFinger(handedness);
      _instances.Add(instance);
      return instance;
    }

    return maybeInstance;
  }
}