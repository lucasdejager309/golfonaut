using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopOnTouch : MonoBehaviour
{
    public float STOP_ON = 3f;

    void OnCollisionEnter2D(Collision2D col) {
        Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();

        if (rb.velocity.magnitude < STOP_ON) {
            if (col.gameObject.GetComponent<PlayerScript>()) {
                col.gameObject.GetComponent<PlayerScript>().gameManager.EndOfShot();
            } else {
                rb.velocity = new Vector2(0, 0);
                rb.angularVelocity = 0;
            }
            
            GetComponentInChildren<Gravity>().Toggle(false);
            
        }
    }
}
