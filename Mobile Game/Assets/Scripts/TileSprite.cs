using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Facing {
    left,
    right,
    up,
    down
}

public class TileSprite : MonoBehaviour
{
    public Facing facing = Facing.up;
    Sprite sprite;
    
    void Start() {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        Vector2 size = col.bounds.size;

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (facing == Facing.left || facing == Facing.right) {
            spriteRenderer.size = new Vector2(size.y, size.x);
        } else spriteRenderer.size = size;
    }
}
