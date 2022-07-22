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

    public bool randomDirection;

    Vector2 origin;

    int index = 0;

    Task t;

    void Start() {
        origin = transform.position;

        Random.InitState((int)System.DateTime.Now.Ticks);
        if (randomDirection) {
            int doRandom = Mathf.FloorToInt(FindObjectOfType<LevelGen>().GetRandomRange(0,2));
            if (doRandom == 1) {
                System.Array.Reverse(points);
            }
        }

        t = new Task(MoveTo(points[index], moveSpeed));
    }

    public void Stop () {
        t.Stop();
    }

    IEnumerator MoveTo(Vector2 endPos, float speed) {
        if (this != null) {
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
}
