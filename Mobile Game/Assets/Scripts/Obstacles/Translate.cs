using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour
{
    public float moveSpeed;
    public Vector2 relativePos1;
    public Vector2 relativePos2;

    Vector2 origin;

    void Start() {
        origin = transform.position;
        MoveToPos1();
    }

    void MoveToPos1() {
        Task t = new Task(MoveTo(relativePos1, moveSpeed));
        t.Finished += delegate {MoveToPos2();};
    }

    void MoveToPos2() {
        Task t = new Task(MoveTo(relativePos2, moveSpeed));
        t.Finished += delegate {MoveToPos1();};
    }

    IEnumerator MoveTo(Vector2 endPos, float speed) {
        float timeElapsed = 0;

        Vector2 startPos = transform.position;

        float actualSpeed = Vector2.Distance(startPos, endPos)/speed;

        while (timeElapsed < actualSpeed) {
            timeElapsed += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, origin+endPos, timeElapsed/actualSpeed);

            yield return null;
        }

        transform.position = origin+endPos;
    }
}
