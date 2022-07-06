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
    Vector2 lastPos;
    int lastShotAmount;

    //exposed parameters
    public float MOVE_FORCE_MULTIPLIER = 200;

    public delegate void PlayerMoves();
    public PlayerMoves playerMove;

    void Start() {
        line = this.GetComponent<LineRenderer>();
        rb = this.GetComponent<Rigidbody2D>();

        playerMove += GameManager.Instance.Null;

        GameObject.FindGameObjectWithTag("RetryButton").GetComponent<Button>().onClick.AddListener(delegate {Retry();});
    }

    public void Move(Vector2 input) {
        if (CanShoot() && rb.velocity.magnitude == 0) {
            if (input != new Vector2(0,0)) {
                lastPos = new Vector2(transform.position.x, transform.position.y);
                lastShotAmount = GameManager.Instance.shots;

                rb.AddForce(input*MOVE_FORCE_MULTIPLIER);
                rb.angularVelocity = Random.Range(-360, 360);
                GameManager.Instance.AddShot(-1);

                playerMove();
            }
        }
    }

    bool CanShoot() {
        if (GameManager.Instance.shots > 0) return true;
        else return false;
    }

    public void Retry() {  
        if (lastPos != new Vector2(transform.position.x, transform.position.y)) {
            rb.velocity = new Vector2(0,0);
            transform.position = new Vector3(lastPos.x, lastPos.y, transform.position.z);
            GameManager.Instance.SetShots(lastShotAmount);  
        }
    }

    void Update() {
        float velocity = rb.velocity.magnitude;
        if (velocity < 0.1) {
            rb.velocity = new Vector2(0,0);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.GetComponent<IPowerUp>() != null) collider.GetComponent<IPowerUp>().PowerUp();
    }

    public void DrawLine(Vector2 start, Vector2 end) {
        if (CanShoot() && rb.velocity.magnitude == 0) {
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
