using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int currentLevel = 1;
    public static int highScore = 1;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        // Load the high score from memory (default to 1 if it doesn't exist)
        highScore = PlayerPrefs.GetInt("HighScore", 1);
        UpdateUI();
    }
    
    void UpdateUI()
    {
        if (levelText != null) levelText.text = "LEVELS CLEARED " + "\n" + currentLevel;
        if (highScoreText != null) highScoreText.text = "Best: " + highScore;
    }

    // Call this when the player dies
    public static void ResetGame()
    {
        // Check if we set a new record before resetting
        if (currentLevel > highScore)
        {
            highScore = currentLevel;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
        currentLevel = 1;
    }
}
