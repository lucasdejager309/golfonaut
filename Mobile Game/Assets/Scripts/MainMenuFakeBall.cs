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

    void OnCollisionEnter2D(Collision2D collision) {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (!collision.otherCollider.isTrigger && rb.velocity.magnitude > 0.3f) FindObjectOfType<AudioManager>().PlayFromGroup("bounce");
    }
}
