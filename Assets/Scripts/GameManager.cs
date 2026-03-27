using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Keeping this static so MazeGenerator can read it across scenes
    public static int currentLevel = 1;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        UpdateUI();
    }
    
    void UpdateUI()
    {
        // 1. Update Current Level Display
        if (levelText != null) 
            levelText.text = "LEVELS CLEARED: " + "\n" + (currentLevel - 1);

        // 2. Handle High Score
        int savedHighScore = PlayerPrefs.GetInt("FlickerHighScore", 0);
        
        // If the player just beat their record, save it immediately
        int levelsCleared = currentLevel - 1;
        if (levelsCleared > savedHighScore)
        {
            savedHighScore = levelsCleared;
            PlayerPrefs.SetInt("FlickerHighScore", savedHighScore);
            PlayerPrefs.Save();
        }

        if (highScoreText != null)
            highScoreText.text = "HIGH SCORE: " + "\n"+ savedHighScore;
    }
    
}
