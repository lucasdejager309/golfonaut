using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TranslateMode {
    DoReverse,
    Teleport
}

public class Translate : MonoBehaviour
{
    public float moveSpeed;
    public TranslateMode mode;
    public Vector2[] points;

    Vector2 origin;

    int index = 0;

    Task t;

    void Start() {
        origin = transform.position;
        transform.position = origin-points[0];
        t = new Task(MoveTo(points[index], moveSpeed));
    }

    IEnumerator MoveTo(Vector2 endPos, float speed) {
        if (mode == TranslateMode.Teleport && index == points.Length-1) {
            transform.position = origin+points[index];
            index = 0;

        } else if (mode == TranslateMode.DoReverse && index == points.Length-1) {
            index = 0;
        } else {
            index++;
        }

        t.Finished += delegate {t = new Task(MoveTo(points[index], moveSpeed));};

        if (mode == TranslateMode.Teleport && index == 0) yield break;
        
        float timeElapsed = 0;
        Vector2 startPos = (Vector2)transform.position;
        float actualSpeed = Vector2.Distance(startPos-origin, endPos)/speed;
    
        while (timeElapsed < actualSpeed) {
            timeElapsed += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, origin+endPos, timeElapsed/actualSpeed);

            yield return null;
        }

        transform.position = origin+endPos;
    }
}
