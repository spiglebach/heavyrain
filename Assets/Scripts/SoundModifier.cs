using UnityEngine;
using UnityEngine.UI;

public class SoundModifier : MonoBehaviour {
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private AudioClip[] sampleClips;
    private SoundSettings _soundSettings;
    private MusicPlayer _musicPlayer;

    void Start() {
        _soundSettings = FindObjectOfType<SoundSettings>();
        _musicPlayer = FindObjectOfType<MusicPlayer>();
        masterVolumeSlider.value = _soundSettings.GetMasterVolume();
        musicVolumeSlider.value = _soundSettings.GetMusicVolume();
        sfxVolumeSlider.value = _soundSettings.GetSfxVolume();
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
    }

    public void OnMasterVolumeChanged(float newValue) {
        _soundSettings.SetMasterVolume(newValue);
        AdjustMusicVolume();
        PreviewSfxVolume();
    }

    public void OnMusicVolumeChanged(float newValue) {
        _soundSettings.SetMusicVolume(newValue);
        AdjustMusicVolume();
    }

    public void OnSfxVolumeChanged(float newValue) {
        _soundSettings.SetSfxVolume(newValue);
        PreviewSfxVolume();
    }

    private void PreviewSfxVolume() {
        if (sampleClips.Length > 0) {
            var clip = sampleClips[Random.Range(0, sampleClips.Length)];
            AudioSource.PlayClipAtPoint(clip, Vector3.zero, _soundSettings.GetCompositeSfxVolume());
        }
    }

    private void AdjustMusicVolume() {
        _musicPlayer.AdjustVolume(_soundSettings.GetCompositeMusicVolume());
    }
}
