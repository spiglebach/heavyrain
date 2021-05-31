using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadNextLevel() {
        Time.timeScale = 1f;
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextLevelIndex);
    }

    public void LoadMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void ReloadCurrentLevel() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
