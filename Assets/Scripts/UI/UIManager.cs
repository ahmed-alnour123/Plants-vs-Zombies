// using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    public void ReturnToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadNextLevel() {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex + 1);
    }

    public void ExitGame() {
        // EditorApplication.ExitPlaymode();
        Application.Quit();
    }
}
