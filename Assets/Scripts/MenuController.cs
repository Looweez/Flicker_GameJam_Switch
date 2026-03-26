using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject gameOverPanel; // Only assign this in the GameScene

    // --- Main Menu Functions ---
    public void StartGame()
    {
        SceneManager.LoadScene(1); // Loads the first game level
    }

    public void QuitGame()
    {
        Debug.Log("Game Exited"); // Only visible in the console
        Application.Quit();
    }

    // --- Lose Screen Functions ---
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Freezes the game world
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Unfreeze
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Unfreeze
        SceneManager.LoadScene(0);
    }
}