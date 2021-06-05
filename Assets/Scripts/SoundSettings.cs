using UnityEngine;

public class SoundSettings : MonoBehaviour {
    private float masterVolume = 1f;
    private float musicVolume = 0.3f;
    private float sfxVolume = 1f;

    public const string MASTER_VOLUME_KEY = "MASTER_VOLUME";
    public const string MUSIC_VOLUME_KEY = "MUSIC_VOLUME";
    public const string SFX_VOLUME_KEY = "SFX_VOLUME";

    private void Awake() {
        if (FindObjectsOfType<SoundSettings>().Length > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() {
        masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, masterVolume);
        musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, musicVolume);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, sfxVolume);
    }

    public float GetCompositeMusicVolume() {
        return masterVolume * musicVolume;
    }
    
    public float GetMasterVolume() {
        return masterVolume;
    }
    
    public float GetMusicVolume() {
        return musicVolume;
    }
    
    public float GetSfxVolume() {
        return sfxVolume;
    }

    public float GetCompositeSfxVolume() {
        return masterVolume * sfxVolume;
    }

    public void SetMasterVolume(float newVolume) {
        masterVolume = newVolume;
    }
    
    public void SetMusicVolume(float newVolume) {
        musicVolume = newVolume;
    }
    
    public void SetSfxVolume(float newVolume) {
        sfxVolume = newVolume;
    }

    public void Persist() {
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, masterVolume);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, masterVolume);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, masterVolume);
        PlayerPrefs.Save();
    }
}
