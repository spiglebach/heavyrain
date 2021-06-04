using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
    private float masterVolume = 1f;
    private float musicVolume = 0.3f;
    private float sfxVolume = 1f;

    private const string MASTER_VOLUME = "MASTER_VOLUME";
    private const string MUSIC_VOLUME = "MUSIC_VOLUME";
    private const string SFX_VOLUME = "SFX_VOLUME";

    private void Awake() {
        if (FindObjectsOfType<SoundController>().Length > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() {
        masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME, masterVolume);
        musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME, musicVolume);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME, sfxVolume);
    }

    public float GetMasterVolume() {
        return masterVolume;
    }

    public void SetMasterVolume(float newVolume) {
        masterVolume = newVolume;
    }
}
