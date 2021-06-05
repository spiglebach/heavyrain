using System;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    private AudioSource _audioSource;
    
    private void Awake() {
        if (FindObjectsOfType<MusicPlayer>().Length > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        _audioSource = GetComponent<AudioSource>();
        var masterVolume = PlayerPrefs.GetFloat(SoundSettings.MASTER_VOLUME_KEY, 1f);
        var musicVolume = PlayerPrefs.GetFloat(SoundSettings.MUSIC_VOLUME_KEY, .3f);
        _audioSource.volume = masterVolume * musicVolume;
    }

    public void AdjustVolume(float newVolume) {
        _audioSource.volume = newVolume;
    }
}
