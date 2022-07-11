using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
    public float MOVE_FORCE_MULTIPLIER;
    public float SPIN_SPEED;

    public bool isMoving;
    public bool inGravField;

    public Vector2 lastPos;
    public Shots shots;

    LineRenderer line;
    Rigidbody2D rb;
    public GameManager gameManager;

    public delegate void PlayerMoves();
    public PlayerMoves playerMove;

    public delegate void PlayerResets();
    public PlayerResets playerReset;

    public delegate void PlayerStops();
    public PlayerStops playerStop;

    void Start() {
        line = this.GetComponent<LineRenderer>();
        rb = this.GetComponent<Rigidbody2D>();

        playerMove += delegate {};
        playerReset += delegate {};
        playerStop += delegate {};
    }

    void FixedUpdate() {
        float velocity = rb.velocity.magnitude;
        if (velocity < 0.1 && velocity > 0 && !inGravField) {
            gameManager.EndOfShot();
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.GetComponent<IPowerUp>() != null) collider.GetComponent<IPowerUp>().PowerUp();
    }

    public void Move(Vector2 input) {
        if (input != new Vector2 (0,0)) {
            lastPos = new Vector2(transform.position.x, transform.position.y);

            rb.AddForce(input*MOVE_FORCE_MULTIPLIER);
            rb.angularVelocity = (input*MOVE_FORCE_MULTIPLIER).magnitude*SPIN_SPEED;

            shots.Shoot();

            playerMove();
        }
    }

    public void StopBall() {
        playerStop();
        rb.velocity = new Vector2(0, 0);
        rb.angularVelocity = 0;
        isMoving = false;
    }

    public bool CanShoot() {
        if (shots.CanShoot() && gameManager.takeInput && !isMoving) return true;
        else return false;
    }

    public void GoToLastPos() {
        transform.position = new Vector3(lastPos.x, lastPos.y, transform.position.z);
        playerReset();
    }

    public void DrawLine(Vector2 start, Vector2 end) {
        if (shots.currentShots > 0 && !isMoving && gameManager.takeInput) {
            Vector2 lineVector = end - start;
            line.SetPosition(0, (Vector2)transform.position);
            line.SetPosition(1, (Vector2)transform.position-lineVector);
        }
    }

    public void HideLine() {
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);
    }
}