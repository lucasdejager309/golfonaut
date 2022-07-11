using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuFakeBall : MonoBehaviour
{
    [SerializeField] float teleportY;
    [SerializeField] float teleportTo;

    void Update() {
        if (transform.position.y < teleportY) {
            transform.position = new Vector2(transform.position.x, teleportTo);
        }
    }
}
