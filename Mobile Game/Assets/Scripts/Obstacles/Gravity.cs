using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GravDir {
    point,
    left,
    right,
    down,
    up
}

public class Gravity : MonoBehaviour
{
    [Header("Parameters")]

    public GravDir GRAV_DIR = GravDir.point;
    public float GRAVITY_STRENGTH = 9;

    PlacementVariation var;
    SpriteRenderer arrows;

    bool doGravity = true;

    public void Toggle(bool state) {
        doGravity = state;
    }

    void Awake() {
        FindObjectOfType<PlayerScript>().playerMove += delegate {Toggle(true); };
        FindObjectOfType<PlayerScript>().playerReset += Exit;
        if (GRAV_DIR != GravDir.point) {
            arrows = transform.GetChild(0).GetComponent<SpriteRenderer>();
            ScaleArrows();
        }
        if ((var = transform.GetComponentInParent<PlacementVariation>()) && GRAV_DIR != GravDir.point) {
            var.variateDelegate += FixDirection;
        }
    }

    void OnTriggerStay2D(Collider2D col) {
        PlayerScript player;
        if (player = col.GetComponent<PlayerScript>()) {
            player.inGravField = true;
        }

        if (col.GetComponent<Rigidbody2D>() && doGravity) {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();

            Vector2 gravPos = transform.position;
            if (GRAV_DIR != GravDir.point) {
                switch (GRAV_DIR) {
                    case (GravDir.left):
                        gravPos = new Vector2(gravPos.x-10, gravPos.y);
                        break;
                    case (GravDir.right):
                        gravPos = new Vector2(gravPos.x+10, gravPos.y);
                        break;
                    case (GravDir.down):
                        gravPos = new Vector2(gravPos.x, gravPos.y-10);
                        break;
                    case (GravDir.up):
                        gravPos = new Vector2(gravPos.x, gravPos.y+10);
                        break;
                    default:
                        break;
                }
            }

            Vector2 direction = (gravPos - (Vector2)rb.transform.position).normalized;

            rb.AddForce(direction*GRAVITY_STRENGTH);
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.GetComponent<PlayerScript>()) Exit();
    }

    public void Exit() {
        FindObjectOfType<PlayerScript>().inGravField = false;
    }

    void ScaleArrows() {
        Collider2D col = GetComponent<Collider2D>();

        Vector2 scale = arrows.transform.localScale;
        switch (GRAV_DIR) {
            case (GravDir.left):
                scale = new Vector2(-scale.x, scale.y);
                break;
            case (GravDir.right):
                scale = new Vector2(scale.x, scale.y);
                break;
            case (GravDir.down):
                scale = new Vector2(-scale.x, scale.y);
                break;
            case (GravDir.up):
                scale = new Vector2(scale.x, scale.y);
                break;
            default:
                break;
        }
        arrows.transform.localScale = scale;

        if (GRAV_DIR == GravDir.up || GRAV_DIR == GravDir.down) {
            arrows.transform.Rotate(new Vector3(0, 0, 90), Space.Self);
            arrows.size = new Vector2(col.bounds.size.y, col.bounds.size.x);
        } else {
            arrows.size = new Vector2(col.bounds.size.x, col.bounds.size.y);
        }
    }

    void FixDirection() {
        Vector2 scale = arrows.transform.localScale;
        switch (GRAV_DIR) {
            case (GravDir.left):
                if (var.didMirrorHorVar) {
                    GRAV_DIR = GravDir.right;
                    scale = new Vector2(scale.x, scale.y);
                }
                break;
            case (GravDir.right):
                if (var.didMirrorHorVar) {
                    GRAV_DIR = GravDir.left;
                    scale = new Vector2(scale.x, scale.y);
                }
                break;
            case (GravDir.down):
                if (var.didMirrorVerVar) {
                    GRAV_DIR = GravDir.up;
                }
                break;
            case (GravDir.up):
                if (var.didMirrorVerVar) {
                    GRAV_DIR = GravDir.down;
                }
                break;
            default:
                break;
        }
        arrows.transform.localScale = scale;
    }
}
