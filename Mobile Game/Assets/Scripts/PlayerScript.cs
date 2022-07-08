using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    //references to components
    LineRenderer line;
    Rigidbody2D rb;

    //variables
    public Vector2 lastPos;
    public Shots shots;

    //exposed parameters
    public float MOVE_FORCE_MULTIPLIER = 200;

    public bool isMoving;
    public bool inGravField;

    public delegate void PlayerMoves();
    public PlayerMoves playerMove;

    public GameManager gameManager;

    void Start() {
        line = this.GetComponent<LineRenderer>();
        rb = this.GetComponent<Rigidbody2D>();

        gameManager = FindObjectOfType<GameManager>();

        playerMove += gameManager.Null;
    }

    public void Move(Vector2 input) {
        if (shots.Shoot() && gameManager.takeInput && rb.velocity.magnitude == 0) {
            if (input != new Vector2(0,0)) {
                lastPos = new Vector2(transform.position.x, transform.position.y);

                rb.AddForce(input*MOVE_FORCE_MULTIPLIER);
                rb.angularVelocity = Random.Range(-360, 360);

                playerMove();
                isMoving = true;
            }
        }
    }
    

    public void Retry() {
        if (shots.currentRetries > 0 && (Vector2)transform.position != lastPos) {
            shots.ResetToLast(true);
            GoToLastPos();
        }

        Gravity[] gravs = FindObjectsOfType<Gravity>();
        foreach (Gravity grav in gravs) grav.Exit();
    }

    public void Die() {
        shots.ResetToLast(false);
        GoToLastPos();
        StopBall();
    }

    public void GoToLastPos() {
        transform.position = new Vector3(lastPos.x, lastPos.y, transform.position.z);
    }

    void FixedUpdate() {
        float velocity = rb.velocity.magnitude;
        if (velocity < 0.1 && velocity > 0 && !inGravField) {
            StopBall();
        }
    }

    public void StopBall() {
        rb.velocity = new Vector2(0,0);
        isMoving = false;
        if (shots.currentShots <= 0 && shots.currentRetries <= 0) gameManager.GameOver();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.GetComponent<IPowerUp>() != null) collider.GetComponent<IPowerUp>().PowerUp();
    }

    public void DrawLine(Vector2 start, Vector2 end) {
        if (shots.currentShots > 0 && !isMoving) {
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
