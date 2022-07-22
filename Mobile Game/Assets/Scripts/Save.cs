using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{   
    public int highScore;

    public float musicVolume;
    public float soundVolume;

    public Save() {
        highScore = 0;
        musicVolume = 0.15f;
        soundVolume = 0.5f;
    }
}
