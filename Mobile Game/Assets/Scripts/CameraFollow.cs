using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float CAMERA_MOVE_SPEED = 0.5f;

    Transform player;
    bool followPlayerDown = false;

    void Start() {
        player = GameObject.FindObjectOfType<PlayerScript>().gameObject.transform;
    }

    void Update() {
        if (player.transform.position.y > transform.position.y) {
            followPlayerDown = true;
            transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }

        if (player.transform.position.y < transform.position.y && followPlayerDown) {
            transform.position = new Vector3(-transform.position.x, player.transform.position.y, transform.position.z);
        }
    }
}
