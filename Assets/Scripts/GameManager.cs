using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Keeping this static so MazeGenerator can read it across scenes
    public static int currentLevel = 1;
    public TextMeshProUGUI levelText;

    void Start()
    {
        UpdateUI();
    }
    
    void UpdateUI()
    {
        // Subtract 1 if you want it to show "Levels Cleared" 
        // (If they are on Level 1, they have cleared 0)
        if (levelText != null) 
            levelText.text = "LEVELS CLEARED: " + "\n" + (currentLevel - 1);
    }
    
}
