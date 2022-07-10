using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed;

    [Header("LimitedRotation")]
    public bool rotatecircles;
    public float minRotation;
    public float maxRotation;

    void FixedUpdate() {
        if (rotatecircles) {    
            transform.Rotate(0, 0, rotationSpeed, Space.Self);
        } else {
            //TODO: lerp rotation
        }
    }
}
