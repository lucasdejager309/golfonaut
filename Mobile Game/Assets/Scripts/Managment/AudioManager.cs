using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum soundType {
    SOUNDEFFECT,
    MUSIC
}

[System.Serializable]
public class SoundEffect {
    public soundType type;
    public string name;
    public AudioClip audioClip;
}

[System.Serializable]
public class SoundEffectGroup {
    public soundType type;
    public string name;
    public AudioClip[] audioClips;

    public AudioClip GetClip() {
        return audioClips[Random.Range(0, audioClips.Length)];
    }
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] SoundEffect[] soundEffects;
    [SerializeField] SoundEffectGroup[] soundGroups;
    private AudioSource audioSource;

    [SerializeField] float soundVolume;
    [SerializeField] float musicVolume;

    private List<AudioSource> soundSources = new List<AudioSource>();
    private List<AudioSource> musicSources = new List<AudioSource>();

    void Awake() {
        DontDestroyOnLoad(this);
        SaveManager saveManager = new SaveManager();
        Save save = saveManager.GetSave();
        soundVolume = save.soundVolume;
        musicVolume = save.musicVolume;
    }

    public void Setvolume(soundType type, float value) {
        switch (type) {
            case soundType.SOUNDEFFECT:
                soundVolume = value;
                foreach(AudioSource source in soundSources) {
                    if (source != null) SetVolumeBySource(source, value);
                }
                break;
            case soundType.MUSIC:
                musicVolume = value;
                foreach(AudioSource source in musicSources) {
                    if (source != null) SetVolumeBySource(source, value);
                }
                break;
        }
    }

    void SetVolumeBySource(AudioSource source, float value) {
        source.volume = value;
    }

    public void DestroyAll() {
        soundSources.Clear();
        musicSources.Clear();
        
        GameObject[] objects = GameObject.FindGameObjectsWithTag("AudioSource");
        foreach (GameObject obj in objects) {
            Destroy(obj);
        }
    }

    public void PlaySound(string name, bool loop = false) {
        foreach (SoundEffect sound in soundEffects) {
            if (sound.name == name) {
                PlayClip(sound.audioClip, sound.type, loop);
            }
        }
    }

    public void PlayFromGroup(string groupname) {
        foreach (SoundEffectGroup group in soundGroups) {
            if (group.name == groupname) {
                PlayClip(group.GetClip(), group.type, false);
            }
        }
    }

    void PlayClip(AudioClip clip, soundType type, bool loop) {
        GameObject source = new GameObject("audioSource");
        DontDestroyOnLoad(source);
        source.tag = "AudioSource";
        audioSource = source.AddComponent<AudioSource>();

        switch (type) {
            case soundType.SOUNDEFFECT:
                SetVolumeBySource(audioSource, soundVolume);
                soundSources.Add(audioSource);
                break;
            case soundType.MUSIC:
                SetVolumeBySource(audioSource, musicVolume);
                musicSources.Add(audioSource);
                break;
        }

        Task t = new Task(PlayFromSource(audioSource, clip, loop));
        if (!loop) t.Finished += delegate {
                if (soundSources.Contains(audioSource)) soundSources.Remove(audioSource);
                if (musicSources.Contains(audioSource)) musicSources.Remove(audioSource);

                Destroy(source);
            };
    }

    IEnumerator PlayFromSource(AudioSource source, AudioClip clip, bool loop) {
        if (!loop) {
            source.PlayOneShot(clip);
        } else {
            source.loop = true;
            source.clip = clip;
            source.Play();
        }
        
        
        float time = 0;
        while (time < clip.length) {
            time += Time.deltaTime;
            yield return null;
        }
    }
}