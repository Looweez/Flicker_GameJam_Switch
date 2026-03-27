using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject gameOverPanel; // Only assign this in the GameScene
    public static int currentLevel = 1;

    // --- Main Menu Functions ---
    public void StartGame()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        SceneManager.LoadScene(1); // Loads the first game level
    }

    public void QuitGame()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
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
        StartCoroutine(RestartRoutine());
    }
    
    IEnumerator RestartRoutine()
    {
        // 1. Play the click sound immediately
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.buttonClick);
        }

        // 2. Wait for 0.2 seconds (RealTime because the game is paused!)
        // This gives the sound enough time to play its initial "pop"
        yield return new WaitForSecondsRealtime(0.2f);

        // 3. Reset and Load
        currentLevel = 1;
        Time.timeScale = 1f; 
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Unfreeze
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        SceneManager.LoadScene(0);
    }
}