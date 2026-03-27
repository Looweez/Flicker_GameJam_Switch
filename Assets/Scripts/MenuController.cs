using System.Collections;
using TMPro;
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
        if (AudioManager.instance != null)
            AudioManager.instance.PlaySFX(AudioManager.instance.buttonClick);

        yield return new WaitForSecondsRealtime(0.2f);

        // CRITICAL FIX: Reset the static level back to 1
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