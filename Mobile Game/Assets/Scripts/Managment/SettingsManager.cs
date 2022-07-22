using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider soundSlider;
    public Slider musicSlider;

    void Start() {
        SaveManager saveManager = new SaveManager();
        Save save = saveManager.GetSave();

        soundSlider.value = save.soundVolume;
        musicSlider.value = save.musicVolume;

        soundSlider.onValueChanged.AddListener(delegate {FindObjectOfType<AudioManager>().Setvolume(soundType.SOUNDEFFECT, soundSlider.value);});
        musicSlider.onValueChanged.AddListener(delegate {FindObjectOfType<AudioManager>().Setvolume(soundType.MUSIC, musicSlider.value);});

        GameObject.FindGameObjectWithTag("StartButton").GetComponent<Button>().onClick.AddListener(delegate {
            saveManager.SaveSettings(soundSlider.value, musicSlider.value);
        });
    }  
}
