using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadNextLevel() {
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextLevelIndex);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene("Main Menu");
    }

    public void ReloadCurrentLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
