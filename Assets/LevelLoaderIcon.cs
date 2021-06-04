using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoaderIcon : MonoBehaviour {
    [SerializeField] private int levelIndex = 1;
    private LevelLoader _levelLoader;
    private LevelProgress _levelProgress;
    private Button _button;
    private TMP_Text _text;
    private LevelStatus status;

    private void Start() {
        _levelLoader = FindObjectOfType<LevelLoader>();
        _levelProgress = FindObjectOfType<LevelProgress>();
        _button = GetComponent<Button>();
        _text = GetComponentInChildren<TMP_Text>();
        _text.text = levelIndex.ToString();
        status = _levelProgress.GetLevelStatus(levelIndex);
        var buttonColors = _button.colors;
        Debug.Log($"Button{levelIndex.ToString()}.status: {status.ToString()}");
        if (status == LevelStatus.COMPLETE) {
            buttonColors.normalColor = Color.green;
        }
        if (status == LevelStatus.CURRENT) {
            buttonColors.normalColor = new Color(1f, 0.9f, 0);
        }
        if (status == LevelStatus.LOCKED) {
            buttonColors.normalColor = Color.gray;
            _button.interactable = false;
        }
        _button.colors = buttonColors;
    }

    public void LoadLevelIfUnlocked() {
        if (!_levelLoader || status == LevelStatus.LOCKED) return;
        _levelLoader.LoadLevelIfUnlocked(levelIndex);
    }
}
