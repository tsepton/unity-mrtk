using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public class HandTracking : MonoBehaviour {

    public GameObject sphereMarker;

        private MixedRealityPose pose;
    private GameObject thumbObject;
    
    // Start is called before the first frame update
    void Start() {
        thumbObject = Instantiate(sphereMarker, this.transform);
        

    }

    // Update is called once per frame
    void Update() {
        thumbObject.GetComponent<Renderer>().enabled = false;
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, Handedness.Right, out pose)) {
            thumbObject.GetComponent<Renderer>().enabled = true;
            thumbObject.transform.position = pose.Position;
        }
    }
}
