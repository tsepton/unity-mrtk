using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtUser : MonoBehaviour {
    void Update() {
        transform.LookAt(Camera.current.transform);
    }
}
