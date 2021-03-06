using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementVariation : MonoBehaviour
{   
    [Header("Horizontal Variation")]
    public float minHorVar;
    public float maxHorVar;


    [Header("Rotation Variation")]
    public bool rotate;
    public float[] rotationVariation;

    [Header("Mirror Variation")]
    public bool mirrorHorVar;
    public bool mirrorVerVar;

    [Header("Mirrored?")]
    public bool didMirrorHorVar = false;
    public bool didMirrorVerVar = false;

    public delegate void VariateDelegate();
    public VariateDelegate variateDelegate;

    LevelGen gen;

    void Awake() {
        if (FindObjectOfType<GameManager>()) variateDelegate += delegate{};
    }

    public void Variate() {
        gen = FindObjectOfType<LevelGen>();

        //HORIZONTAL TRANSLATION
        Random.InitState((int)System.DateTime.Now.Ticks);
        transform.GetChild(0).position = new Vector2(transform.GetChild(0).position.x+gen.GetRandomRange(minHorVar, maxHorVar),
        transform.GetChild(0).position.y);

        //ROTATION
        float randomRot = 0;
        if (rotate) {
            Random.InitState((int)System.DateTime.Now.Ticks);
            randomRot = rotationVariation[(int)gen.GetRandomRange(0,rotationVariation.Length)];
        }
        transform.GetChild(0).Rotate(0, 0, randomRot, Space.Self);
    
        //HORIZONTAL MIRRORING
        Random.InitState((int)System.DateTime.Now.Ticks);
        if (gen.GetRandomRange(0, 2) == 1 && mirrorHorVar) {
            transform.GetChild(0).Rotate(0, 180, 0, Space.Self);
            didMirrorHorVar = true;
        }

        //VERTICAL MIRRORING
        Random.InitState((int)System.DateTime.Now.Ticks);
        if (gen.GetRandomRange(0, 2) == 1 && mirrorVerVar) {
            transform.GetChild(0).Rotate(180, 0, 0, Space.Self);
            didMirrorVerVar = true;
        }

        variateDelegate();
    }
}
