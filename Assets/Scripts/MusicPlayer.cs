using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    private void Awake() {
        if (FindObjectsOfType<MusicPlayer>().Length > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }
}
