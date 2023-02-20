using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public void ApplyRandomColor(GameObject go) {
        Color newColor = new Color( Random.value, Random.value, Random.value, 1.0f );
        go.GetComponent<Renderer>().material.color = newColor;
    }
}
