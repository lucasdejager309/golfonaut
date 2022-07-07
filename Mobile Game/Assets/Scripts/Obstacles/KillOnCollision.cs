using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col) {
        PlayerScript player;
        if (player = col.GetComponent<PlayerScript>()) {
            player.Die();
        }
    }
}
