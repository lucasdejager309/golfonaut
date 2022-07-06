using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightSideUp : MonoBehaviour
{
    public bool DoUpdates = false;

    void FixedUpdate() {
        if (DoUpdates) {
            Fix();
        }
    }

    public void Fix() {
        Transform parent = transform.parent;
        transform.SetParent(null);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.SetParent(parent);
    }
}
